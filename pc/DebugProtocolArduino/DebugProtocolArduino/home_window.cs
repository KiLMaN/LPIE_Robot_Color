using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;




namespace DebugProtocolArduino
{
    public partial class Form1 : Form
    {
        int BAUD_RATE = 115200;

        ushort g_ProtocolCpt = 0;
        private Protocol g_Protocol = new Protocol();
        public Logger g_Logger = new Logger(); // Logger global
        

        public Form1()
        {

            InitializeComponent();
            g_Logger.attachToRTB(txtbox_debug_log);
            Logger.GlobalLogger = g_Logger; // Met a la disposition le logger
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
                Logger.GlobalLogger.error("idCapteur  :" + idCapteur);
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
                Logger.GlobalLogger.info("Aucun port serie n'est disponible !");
            }
        }

        /** Ouvre le port série séléctionné */
        private void openSerialPort()
        {
            /* Déjà ouvert */
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
        }

        private void sendMoveMessage(bool Sens)
        {
            PCtoEMBMessageMove Message = new PCtoEMBMessageMove();
            Message.headerMess = (byte)PCtoEMBmessHeads.MOVE;
            Message.sens = (byte)((Sens) ? 0x01 : 0x00);
            Message.speed = (byte)0xF0;
            Message.distance = (byte)0xF0;

            byte[] data = Message.getBytes();
            byte Dst = (byte)Convert.ToUInt16(txt_idDst.Text);
            byte Src = (byte)Convert.ToUInt16(txt_idSrc.Text);

            TrameProtocole pTrame = g_Protocol.MakeTrame(Src, Dst, g_ProtocolCpt, data);
        }
        private void sendTurnMessage(bool Sens)
        {
            PCtoEMBMessageTurn Message = new PCtoEMBMessageTurn();
            Message.headerMess = (byte)PCtoEMBmessHeads.TURN;
            Message.direction = (byte)((Sens) ? 0x01 : 0x00);
            Message.angle = (byte)0x5A;


            byte[] data = Message.getBytes();
            byte Dst = (byte)Convert.ToUInt16(txt_idDst.Text);
            byte Src = (byte)Convert.ToUInt16(txt_idSrc.Text);
            TrameProtocole pTrame = g_Protocol.MakeTrame(Src, Dst, g_ProtocolCpt, data);
            g_Logger.debug(pTrame.data.ToString());
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

        private void button1_Click(object sender, EventArgs e)
        {
            // button de test
        }

        

       
    }
}
