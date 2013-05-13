using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using utils;
using xbee.Communication;






namespace xbee
{

    public partial class Form1 : Form
    {



       


        //private SerialXbee g_Serial = new SerialXbee();
        //private messageBuilder g_MessageBuilder;
        public Logger g_Logger = new Logger(); // Logger global
        

        public Form1()
        {
             
            InitializeComponent();
            g_Logger.attachToRTB(txtbox_debug_log);
            Logger.GlobalLogger = g_Logger; // Met a la disposition le logger
            getListePortSerie();

            //g_MessageBuilder = new messageBuilder();
            //g_MessageBuilder.source = Convert.ToByte(txt_idSrc.Text);
            //g_MessageBuilder.destination = Convert.ToByte(txt_idDst.Text);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_close);

            
        }

        

        void Form1_close(object e, FormClosingEventArgs arg)
        {
            //g_Serial.StopListenSerial();
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
                Logger.GlobalLogger.error("idCapteur  :" + idCapteur);
            } 
        }
        #endregion



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
       /* private void openSerialPort()
        {
            // Déjà ouvert //
            if (gPortSerie.IsOpen)
            {
                Logger.GlobalLogger.debug("Fermeture du port : " + gPortSerie.PortName);
                gPortSerie.Close();
                btn_connection.Text = "Connection";
                liste_portSerie.Enabled = true;
                btn_ActualiserListePortSerie.Enabled = true;
            }
            else
            {
                try
                {
                    Logger.GlobalLogger.debug("Ouverture du port : " + (string)liste_portSerie.SelectedItem);
                    gPortSerie.PortName = (string)liste_portSerie.SelectedItem;
                    gPortSerie.BaudRate = BAUD_RATE;
                    gPortSerie.Open();
                    btn_connection.Text = "Fermeture";
                    liste_portSerie.Enabled = false;
                    btn_ActualiserListePortSerie.Enabled = false;
                }
                catch (Exception E)
                {
                    Logger.GlobalLogger.error(E.Message.ToString());
                    liste_portSerie.Enabled = true;
                    btn_ActualiserListePortSerie.Enabled = true;
                }
            }
        }*/


        #region #### Buttons ####
        /* Appui sur le bouton Connection */
        private void btn_connection_Click(object sender, EventArgs e)
        {
            
        }

        /* Appui sur le bouton Actualiser */
        private void btn_ActualiserListePortSerie_Click(object sender, EventArgs e)
        {
            getListePortSerie();
        }


        /* Bouton Mouvement UP / DOWN */
        private void btn_up_Click(object sender, EventArgs e)
        {
            //g_Serial.addMessageToSend(g_MessageBuilder.createMoveMessage(true,0x50,0x50));
        }
        private void btn_down_Click(object sender, EventArgs e)
        {
            //g_Serial.addMessageToSend(g_MessageBuilder.createMoveMessage(false, 0x50, 0x50));
        }


        /* Bouton Mouvement LEFT /  RIGHT */
        private void btn_left_Click(object sender, EventArgs e)
        {
            //g_Serial.addMessageToSend(g_MessageBuilder.createTurnMessage(true, 0x5A));
        }
        private void btn_right_Click(object sender, EventArgs e)
        {
            //g_Serial.addMessageToSend(g_MessageBuilder.createTurnMessage(false, 0x5A));
        }

        /* Bouton pour la pince */
        private void btn_pince_close_Click(object sender, EventArgs e)
        {
            //g_Serial.addMessageToSend(g_MessageBuilder.createCloseClawMessage());
        }
        private void btn_pince_open_Click(object sender, EventArgs e)
        {
           //g_Serial.addMessageToSend(g_MessageBuilder.createOpenClawMessage());
        }

        #endregion

        private void CB_EnableSensor_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void updateSourceAndDestination()
        {
           // g_MessageBuilder.source = Convert.ToByte(txt_idSrc.Text);
            //g_MessageBuilder.destination = Convert.ToByte(txt_idDst.Text);
        }
        private void txt_idSrc_TextChanged(object sender, EventArgs e)
        {
            updateSourceAndDestination();
        }

        private void txt_idDst_TextChanged(object sender, EventArgs e)
        {
            updateSourceAndDestination();
        }



    }
}
