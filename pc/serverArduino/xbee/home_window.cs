﻿using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using utils;
using xbee.Communication;
using xbee.Communication.Events;
using System.Threading;

namespace xbee
{

    public partial class Form1 : Form
    {
        //private SerialXbee g_Serial = new SerialXbee();
        //private messageBuilder g_MessageBuilder;
        public Logger g_Logger = new Logger(); // Logger global

        private ArduinoManagerComm _ArduinoManager;
        private AutomateCommunication _AutomateComm;

        private Thread _ThreadSendSensorAsk;
        private byte _CurrentArduinoId = 0;

        public Form1()
        {
             
            InitializeComponent();
            g_Logger.attachToRTB(txtbox_debug_log);
            g_Logger.levelDebug = 1; // Enlever le gros debug
            Logger.GlobalLogger = g_Logger; // Met a la disposition le logger
            getListePortSerie();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_close);

            _ArduinoManager = new ArduinoManagerComm();
            _AutomateComm = new AutomateCommunication("COM0", true, _ArduinoManager); // Initialise l'automate sur le port 0
            _AutomateComm.OnNewTrameArduinoReceived += new AutomateCommunication.NewTrameArduinoReceivedEventHandler(_OnNewTrameArduinoReceived);
            _AutomateComm.OnArduinoTimeout +=new AutomateCommunication.ArduinoTimeoutEventHandler(_OnArduinoTimeout);
            
            /* Set de la source */
            _AutomateComm.IdPc = 0xFE;
            MessageBuilder.src = 0xFE;
        }

        void _OnNewTrameArduinoReceived(object sender, NewTrameArduinoReceveidEventArgs e)
        {
            Logger.GlobalLogger.debug("Nouvelle trame recus ! :" + e.Message.ToString());
            if (e.Source.Connected)
            {
                if (!_listeArduinoConn.Items.Contains(e.Source.id))
                    Invoke(new d_addBotToList(addBotToList), "" + e.Source.id);
                    //_listeArduinoConn.Items.Add(e.Source.id);
            }

            if (e.Source.id == _CurrentArduinoId)
            {
                Invoke(new d_updateStateRobot(updateStateRobot), e.Source.Connected);
            }

            if (e.Message.headerMess == (byte)EMBtoPCmessHeads.RESP_SENSOR)
            {
                EMBtoPCMessageRespSensor mess = ((EMBtoPCMessageRespSensor)e.Message);
                // Met a jour la valeur des capteur sur L'IHM
                Invoke(new d_updateStateCapteur(updateStateCapteur), new object[]{mess.idSensor,mess.valueSensor });
            }
        }
        void _OnArduinoTimeout(object sender, ArduinoTimeoutEventArgs e)
        {
            Logger.GlobalLogger.debug("Arduino déconnecté ! :" + e.Bot.ToString(),1);
            if (e.Bot.id == _CurrentArduinoId)
            {
                Invoke(new d_updateStateRobot(updateStateRobot), e.Bot.Connected);
            }
        }

        void Form1_close(object e, FormClosingEventArgs arg)
        {
            if(_ThreadSendSensorAsk != null)
                _ThreadSendSensorAsk.Abort();

            _AutomateComm.Dispose();
            _ArduinoManager = null;
            //g_Serial.StopListenSerial();
        }

        #region #### Delegate ####
        delegate void d_updateStateRobot(Boolean state);
        delegate void d_updateStateCapteur(byte idCapteur, int valCapteur);
        delegate void d_addBotToList(string txt);

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
                Logger.GlobalLogger.error("idCapteur  :" + idCapteur);
            } 
        }
        void updateStateRobot(Boolean state)
        {
            if(state == true)
                pictBoxEtatConn.BackColor = Color.Green; 
            else
                pictBoxEtatConn.BackColor = Color.Red; 
        }
        void addBotToList(string txt)
        {
            _listeArduinoConn.Items.Add(txt);
        }
        #endregion

        #region #### Port Série ####
        /** Recupere la liste des ports séries disponibles sur la machine et l'affiche dans la liste **/
        private void getListePortSerie()
        {
            // Protège contre l'actualisation apres ouverture du port /
            /*if (gPortSerie.IsOpen)
                return;*/
            
            // Trouve les ports /
            string[] listePort = SerialPort.GetPortNames();
       

            // Vide la liste //
            liste_portSerie.Items.Clear();
            // Remplie la liste //

            foreach (string port in listePort)
                liste_portSerie.Items.Add(port);
            // Selectionne le premier de la liste //
            try
            {
                liste_portSerie.SelectedIndex = 0;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Pas de port Serie de disponible 
                MessageBox.Show("Aucun port serie n'est disponible !");
                Logger.GlobalLogger.info("Aucun port serie n'est disponible !");
            }
        }

        /** Ouvre le port série séléctionné */
        private void switchSerialPort()
        {
            // Déjà ouvert //
            if (_AutomateComm.IsSerialPortOpen())
            {
                Logger.GlobalLogger.debug("Fermeture du port serie !",1);
                _AutomateComm.CloseSerialPort();
                btn_connection.Text = "Connection";
                liste_portSerie.Enabled = true;
                btn_ActualiserListePortSerie.Enabled = true;
            }
            else
            {
                try
                {
                    Logger.GlobalLogger.debug("Ouverture du port : " + (string)liste_portSerie.SelectedItem,1);
                    _AutomateComm.OpenSerialPort((string)liste_portSerie.SelectedItem);
                    if (_AutomateComm.IsSerialPortOpen())
                    {
                        btn_connection.Text = "Fermeture";
                        liste_portSerie.Enabled = false;
                        btn_ActualiserListePortSerie.Enabled = false;
                    }
                    else
                    {
                        Logger.GlobalLogger.error("Erreur lors de l'ouverture du port série !");
                        btn_connection.Text = "Connection";
                        liste_portSerie.Enabled = true;
                        btn_ActualiserListePortSerie.Enabled = true;
                    }
                }
                catch (Exception E)
                {
                    Logger.GlobalLogger.error(E.Message.ToString());
                    liste_portSerie.Enabled = true;
                    btn_ActualiserListePortSerie.Enabled = true;
                }
            }
        }
        #endregion 

        #region #### Buttons ####
        /* Appui sur le bouton Connection */
        private void btn_connection_Click(object sender, EventArgs e)
        {
            switchSerialPort();
        }

        /* Appui sur le bouton Actualiser */
        private void btn_ActualiserListePortSerie_Click(object sender, EventArgs e)
        {
            getListePortSerie();
        }

        /* Bouton Mouvement UP / DOWN */
        private void btn_up_Click(object sender, EventArgs e)
        {

            _AutomateComm.PushSendMessageToArduino(
                MessageBuilder.createMoveMessage(true, 100, 0xf0),
                _ArduinoManager.getArduinoBotById(_CurrentArduinoId)
                );
            ArduinoBotComm test = _ArduinoManager.getArduinoBotById(_CurrentArduinoId);
            //g_Serial.addMessageToSend(g_MessageBuilder.createMoveMessage(true,0x50,0x50));
        }
        private void btn_down_Click(object sender, EventArgs e)
        {
            _AutomateComm.PushSendMessageToArduino(
                MessageBuilder.createMoveMessage(false,100, 0xf0),
                _ArduinoManager.getArduinoBotById(_CurrentArduinoId)
                );
            //g_Serial.addMessageToSend(g_MessageBuilder.createMoveMessage(false, 0x50, 0x50));
        }

        /* Bouton Mouvement LEFT /  RIGHT */
        private void btn_left_Click(object sender, EventArgs e)
        {
            _AutomateComm.PushSendMessageToArduino(
               MessageBuilder.createTurnMessage(true, 0x5A),
               _ArduinoManager.getArduinoBotById(_CurrentArduinoId)
               );
            //g_Serial.addMessageToSend(g_MessageBuilder.createTurnMessage(true, 0x5A));
        }
        private void btn_right_Click(object sender, EventArgs e)
        {
            _AutomateComm.PushSendMessageToArduino(
               MessageBuilder.createTurnMessage(false, 0x5A),
               _ArduinoManager.getArduinoBotById(_CurrentArduinoId)
               );
            //g_Serial.addMessageToSend(g_MessageBuilder.createTurnMessage(false, 0x5A));
        }

        /* Bouton pour la pince */
        private void btn_pince_close_Click(object sender, EventArgs e)
        {
            _AutomateComm.PushSendMessageToArduino(
               MessageBuilder.createCloseClawMessage(),
               _ArduinoManager.getArduinoBotById(_CurrentArduinoId)
               );
        }
        private void btn_pince_open_Click(object sender, EventArgs e)
        {
            _AutomateComm.PushSendMessageToArduino(
               MessageBuilder.createOpenClawMessage(),
               _ArduinoManager.getArduinoBotById(_CurrentArduinoId)
               );
           //g_Serial.addMessageToSend(g_MessageBuilder.createOpenClawMessage());
        }

        #endregion

        #region #### Thread Verification Capteurs ####
        private void CB_EnableSensor_CheckedChanged(object sender, EventArgs e)
        {
            if(((CheckBox)sender).Checked)
            //if (_ThreadSendSensorAsk == null)
            {
                _ThreadSendSensorAsk = new Thread(new ThreadStart(_ThreadCheckSensorAsk));
                _ThreadSendSensorAsk.Start();
            }
            else
            {
                _ThreadSendSensorAsk.Abort();
                _ThreadSendSensorAsk = null;
            }
        }
        void _ThreadCheckSensorAsk()
        {
            while (true)
            {
                ArduinoBotComm robot = _ArduinoManager.getArduinoBotById(_CurrentArduinoId);
                if (robot != null)
                {
                    if(robot.Connected)
                    {
                        MessageProtocol mess = MessageBuilder.createAskSensorMessage((byte)IDSensorsArduino.IR);
                        _AutomateComm.PushSendMessageToArduino(mess, robot);

                        mess = MessageBuilder.createAskSensorMessage((byte)IDSensorsArduino.UltraSon);
                        _AutomateComm.PushSendMessageToArduino(mess, robot);
                    }
                }

                Thread.Sleep((int)delayThreadSensor.Value*1000);
            }
        }
        #endregion

        private void _listeArduinoConn_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CurrentArduinoId = Convert.ToByte(_listeArduinoConn.SelectedItem);
            Invoke(new d_updateStateRobot(updateStateRobot),_ArduinoManager.getArduinoBotById(_CurrentArduinoId).Connected);
        }

        private void CB_Xbee_CheckedChanged(object sender, EventArgs e)
        {
            _AutomateComm.setXbeeApiMode(CB_Xbee.Checked);
        }
    }
}
