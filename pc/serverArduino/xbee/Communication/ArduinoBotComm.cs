using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils;

namespace xbee.Communication
{
    
    // Etat du protocole de communication du coté PC
    public enum StateArduinoComm : byte
    {
        STATE_COMM_NONE             = 0x00, // Pas en attente
        STATE_COMM_WAIT_ACK         = 0x01, // En attente d'un ack
        STATE_COMM_WAIT_PING        = 0x02, // En attente d'un ping
        STATE_COMM_WAIT_SENSOR      = 0x03 // En attente d'une valeur de capteur
    }


    public class ArduinoBotComm
    {
        /* Permet de retrouver le robot dans les listes */
        public static Predicate<ArduinoBotComm> ById(byte id)
        {
            return delegate(ArduinoBotComm o)
            {
                return o._id == id;
            };
        }

        private ushort _CountSend = 0;
        public ushort CountSend
        {
            set { _CountSend = value; }
            get { return _CountSend; }
        }
        private byte _id;
        private bool _connected;
        private DateTime _dateLastMessageReceived;
        
       // private DateTime _dateLastMessageSend;

        private List<MessageProtocol> _MessageEnAttenteEnvoi;

        public bool Connected
        {
            //set { _connected = value; }
            get { return _connected; }
        }
        #region #### Etats du protocole ####
        private StateArduinoComm _stateCommunication;
        public StateArduinoComm stateComm
        {
            get { return _stateCommunication; }
            set { _stateCommunication = value; }
        }
        #endregion

        public byte id
        {
            get{return _id;}
            set{this._id = value;}
        }
        public DateTime DateLastMessageReceived
        {
            get{return _dateLastMessageReceived;}
            set{this._dateLastMessageReceived = value;}
        }
        /*public DateTime DateLastMessageSend
        {
            get { return _dateLastMessageSend; }
            set { this._dateLastMessageSend = value; }
        }*/


        public ArduinoBotComm(byte id)
        {

            Logger.GlobalLogger.info("Create new ArduinoBots id:" + id);
            _MessageEnAttenteEnvoi = new List<MessageProtocol>();
            _id = id;

            _connected = false;
            
            _stateCommunication = StateArduinoComm.STATE_COMM_NONE;
            //Disconnect(); // Initialisation 
        }
        public void Disconnect()
        {
            Logger.GlobalLogger.info("Deconnection d'un robot " + id + "Supression des messages en attente ");
            _connected = false;
            
            _stateCommunication = StateArduinoComm.STATE_COMM_NONE;
            _MessageEnAttenteEnvoi.Clear();
        }
        public void Connect()
        {
            this._connected = true;
        }

        #region #### Messages à envoyer ####
        public void PushMessageAEnvoyer(MessageProtocol mess)
        {
            
                foreach (MessageProtocol m in _MessageEnAttenteEnvoi)
                {
                    if (m.headerMess == mess.headerMess)
                    {
                        if (mess.headerMess == (byte)PCtoEMBmessHeads.ASK_SENSOR)
                        {
                            if (((PCtoEMBMessageAskSensor)mess).idSensor == ((PCtoEMBMessageAskSensor)m).idSensor)
                            {
                                Logger.GlobalLogger.error("Tentative d'envoi d'un message au robot qui existe déja ");
                                return;
                            }
                        }
                        else
                        {
                            Logger.GlobalLogger.error("Tentative d'envoi d'un message au robot qui existe déja ");
                            return;
                        }
                    }
                }
            
            _MessageEnAttenteEnvoi.Add(mess);
            Logger.GlobalLogger.info("Envoi du message " + mess.ToString()+ " au robot "+ this.id);
                
        }
        public bool IsMessageAEnvoyer()
        {
            return _MessageEnAttenteEnvoi.FindAll(MessageProtocol.MessageAEnvoyer()).Count > 0;
        }
        public MessageProtocol PopMessageAEnvoyer()
        {
            return _MessageEnAttenteEnvoi.Find(MessageProtocol.MessageAEnvoyer());
        }
        #endregion

        #region #### Messages en attente de ACK ####
        public void AckRecu(ushort numMess)
        {
            foreach (MessageProtocol mess in _MessageEnAttenteEnvoi)
            {
                if (mess.numMessEnvoye == numMess)
                {
                    SupprimerMessage(mess);
                    return;
                }
            }
        }
        // Passe le message en attente de ACK
        public void MessageAttenteAck(MessageProtocol mess,ushort num)
        {   
            int index = _MessageEnAttenteEnvoi.IndexOf(mess);
            MessageProtocol tmp = _MessageEnAttenteEnvoi[index];
            tmp.stateMessage = 1;
            tmp.time = DateTime.Now;
            tmp.countRejeu = 0;
            tmp.numMessEnvoye = num;
            _MessageEnAttenteEnvoi[index] = tmp;
        }
        // Suppression d'un message qui a recu son ACK
        public void SupprimerMessage(MessageProtocol mess)
        {   
            //int index = _MessageEnAttenteEnvoi.IndexOf(mess);
            _MessageEnAttenteEnvoi.Remove(mess);
            /*MessageProtocol tmp = _MessageEnAttenteEnvoi[index];
            tmp.state = 1;
            tmp.time = DateTime.Now;
            tmp.countRejeu = 0;
            _MessageEnAttenteEnvoi[index] = tmp;*/
        }
        // Ajout d'un rejeu pour le message
        public int AddRejeuxMessageAttenteAck(MessageProtocol mess)
        {
            int index = _MessageEnAttenteEnvoi.IndexOf(mess);
            MessageProtocol tmp = _MessageEnAttenteEnvoi[index];
            tmp.countRejeu++;
            _MessageEnAttenteEnvoi[index] = tmp;
            return _MessageEnAttenteEnvoi[index].countRejeu;
        }
        // Update Date Envoi
        public void UpdateDateEnvoiMessageAttenteAck(MessageProtocol mess)
        {
            int index = _MessageEnAttenteEnvoi.IndexOf(mess);
            MessageProtocol tmp = _MessageEnAttenteEnvoi[index];
            tmp.time = DateTime.Now;
            _MessageEnAttenteEnvoi[index] = tmp;

        }


        public bool IsMessageAttenteAck()
        {
            return _MessageEnAttenteEnvoi.FindAll(MessageProtocol.MessageAttenteAck()).Count > 0;
        }
        /*public MessageProtocol PopMessageAttenteAck()
        {
            return _MessageEnAttenteEnvoi.Find(MessageProtocol.MessageAttenteAck());
        }*/

        public List<MessageProtocol> ListMessageAttenteAck()
        {
            return _MessageEnAttenteEnvoi.FindAll(MessageProtocol.MessageAttenteAck());
        }
        /* On remet le message dans la liste */
        public void ResendMessageAttenteAck(MessageProtocol mess)
        {
            int index = _MessageEnAttenteEnvoi.IndexOf(mess);
            MessageProtocol tmp = _MessageEnAttenteEnvoi[index];
            tmp.stateMessage = 0;
            _MessageEnAttenteEnvoi[index] = tmp;
        }
        #endregion

    }
}
