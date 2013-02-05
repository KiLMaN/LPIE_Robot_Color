using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;



namespace DebugProtocolArduino
{
    public partial class Form1 : Form
    {
        int BAUD_RATE = 115200;

        ushort g_ProtocolCpt = 0;
        private Protocol g_Protocol = new Protocol();

        public Form1()
        {
            InitializeComponent();
            Log.attachTo(txtbox_debug_log);
            getListePortSerie();

            //pictBoxEtatConn.BackColor = Color.Red;  
        }

        #region #### Delegate ####
        delegate void d_updateStateRobot(Boolean state);
        void updateStateRobot(Boolean state)
        {
            if(state == true)
                pictBoxEtatConn.BackColor = Color.Green; 
            else
                pictBoxEtatConn.BackColor = Color.Red; 
        }

        delegate void d_updateStateCapteur(byte idCapteur, int valCapteur);
        void updateStateCapteur(byte idCapteur, int valCapteur)
        {
            if (idCapteur == (byte)IDSensorsArduino.IR) // InfraRouge
            {
                lbl_IR_Sensor.Text = valCapteur.ToString();
                proBar_IrSensor.Value = (int)(valCapteur / 2.55);
            }
            else if (idCapteur == (byte)IDSensorsArduino.UltraSon) // UltraSon
            {
                lbl_UltraSon.Text = valCapteur.ToString();
                proBar_UltraSon.Value = (int)(valCapteur / 2.55);
            }
            else
            {
                Log.log("Error 'updateStateCapteur' idCapteur  :"+idCapteur);
            } 
        }
        #endregion



        /** Recupere la liste des ports séries disponibles sur la machine et l'affiche dans la liste **/
        private void getListePortSerie()
        {
            /* Protège contre l'actualisation apres ouverture du port */
            if (gPortSerie.IsOpen)
                return;

            /* Trouve les ports */
            string[] listePort = SerialPort.GetPortNames();


            /* Vide la liste */
            liste_portSerie.Items.Clear();
            /* Remplie la liste */
            foreach (string port in listePort)
                liste_portSerie.Items.Add(port);
            /* Selectionne le premier de la liste */
            try
            {
                liste_portSerie.SelectedIndex = 0;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Pas de port Serie de disponible 
                MessageBox.Show("Aucun port serie n'est disponible !");
                Log.log("Aucun port serie n'est disponible !");
            }
        }

        /** Ouvre le port série séléctionné */
        private void openSerialPort()
        {
            /* Déjà ouvert */
            if (gPortSerie.IsOpen)
            {
                Log.log("Fermeture du port : " + gPortSerie.PortName);
                gPortSerie.Close();
                btn_connection.Text = "Connection";
                liste_portSerie.Enabled = true;
                btn_ActualiserListePortSerie.Enabled = true;
            }
            else
            {
                try
                {
                    Log.log("Ouverture du port : " + (string)liste_portSerie.SelectedItem);
                    gPortSerie.PortName = (string)liste_portSerie.SelectedItem;
                    gPortSerie.BaudRate = BAUD_RATE;
                    gPortSerie.Open();
                    btn_connection.Text = "Fermeture";
                    liste_portSerie.Enabled = false;
                    btn_ActualiserListePortSerie.Enabled = false;
                }
                catch (Exception E)
                {
                    Log.log(E.Message.ToString());
                    liste_portSerie.Enabled = true;
                    btn_ActualiserListePortSerie.Enabled = true;
                }
            }
        }

        private void sendMoveMessage(bool Sens)
        {
            PCtoEMBMessageMove Message = new PCtoEMBMessageMove();
            Message.headerMess = (byte)PCtoEMBmessHeads.MOVE;
            Message.sens = (byte)((Sens) ? 0x01 : 0x00);
            Message.speed = (byte)0xF0;
            Message.distance = (byte)0xF0;
            byte[] data = Message.getBytes();

           /* Messages.PCtoEMBMessageMove pMessage;
            pMessage.headerMess =   (byte)Messages.PCtoEMBmess.MOVE;
            pMessage.sens =         (byte)((Sens) ? 0x01 : 0x00);
            pMessage.speed =        (byte)0xF0;
            pMessage.distance =     (byte)0xF0;
 
            
           
            //getBytes*/

            byte Dst = (byte)Convert.ToUInt16(txt_idDst.Text);
            byte Src = (byte)Convert.ToUInt16(txt_idSrc.Text);

            TrameProtocole pTrame = g_Protocol.MakeTrame(Src, Dst, g_ProtocolCpt, data);
        }
        private void sendTurnMessage(bool Sens)
        {
          /*  Messages.PCtoEMBMessageTurn pMessage;
            pMessage.headerMess = Messages.PCtoEMBmess.TURN;
            pMessage.direction = (byte)((Sens) ? 0x01 : 0x00);
            pMessage.angle = 0x5A;*/
        }


        #region #### Buttons ####
        /* Appui sur le bouton Connection */
        private void btn_connection_Click(object sender, EventArgs e)
        {
            openSerialPort();
        }

        /* Appui sur le bouton Actualiser */
        private void btn_ActualiserListePortSerie_Click(object sender, EventArgs e)
        {
            getListePortSerie();
        }


        /* Bouton Mouvement UP / DOWN */
        private void btn_up_Click(object sender, EventArgs e)
        {
            sendMoveMessage(true);
        }
        private void btn_down_Click(object sender, EventArgs e)
        {
            sendMoveMessage(false);
        }


        /* Bouton Mouvement LEFT /  RIGHT */
        private void btn_left_Click(object sender, EventArgs e)
        {
            sendTurnMessage(true);
        }
        private void btn_right_Click(object sender, EventArgs e)
        {
            sendTurnMessage(false);
        }

    #endregion 

        

       
    }
}
