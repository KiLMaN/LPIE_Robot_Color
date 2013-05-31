using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using utils;
using System.IO.Ports;
using xbee.Communication;
using xbee.Communication.Events;

namespace IA
{
    public partial class HomeIA : Form
    {
        /* Logger */
        Logger g_Logger;

        /* Toute l'IA */
        IntelArt _IntelArt;

        public HomeIA()
        {
            // Formulaire
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Form1_close);

            // Debugger 
            g_Logger = new Logger();
            g_Logger.attachToRTB(RTB_log);
            g_Logger.levelDebug = 1;
            Logger.GlobalLogger = g_Logger;
            // IA
            _IntelArt = new IntelArt();
            

            getListePortSerie(ctlListePorts);
        }

        void Form1_close(object e, FormClosingEventArgs arg)
        {
            _IntelArt.Dispose();
            _IntelArt = null;
            //g_Serial.StopListenSerial();
        }

        #region #### Port Serie ####
        /* Remplis la liste de type ComboBox avec la liste des ports Série dispo */
        private void getListePortSerie(ComboBox ComboARemplir)
        {
            string[] listePort = SerialPort.GetPortNames();

            ComboARemplir.Items.Clear();

            foreach (string port in listePort)
                ComboARemplir.Items.Add(port);
            try
            {
                ComboARemplir.SelectedIndex = 0;
            }
            catch (ArgumentOutOfRangeException)
            {
                // Pas de port Serie de disponible 
                MessageBox.Show("Aucun port serie n'est disponible !");
                Logger.GlobalLogger.info("Aucun port serie n'est disponible !");
            }
        }
        /* Actualiser la liste */
        private void btn_ActualiserListePortSerie_Click(object sender, EventArgs e)
        {
            getListePortSerie(ctlListePorts);
        }
        /** Ouvre le port série séléctionné */
        private void switchSerialPort()
        {
            // Déjà ouvert //
            if (_IntelArt.IsSerialPortOpen())
            {
                Logger.GlobalLogger.info("Fermeture du port serie !");
                _IntelArt.CloseSerialPort();
                btn_connection.Text = "Connection";
                ctlListePorts.Enabled = true;
                btn_ActualiserListePortSerie.Enabled = true;
            }
            else
            {
                try
                {
                    Logger.GlobalLogger.info("Ouverture du port : " + (string)ctlListePorts.SelectedItem);
                    _IntelArt.OpenSerialPort((string)ctlListePorts.SelectedItem);
                    if (_IntelArt.IsSerialPortOpen())
                    {
                        btn_connection.Text = "Fermeture";
                        ctlListePorts.Enabled = false;
                        btn_ActualiserListePortSerie.Enabled = false;
                    }
                    else
                    {
                        Logger.GlobalLogger.error("Erreur lors de l'ouverture du port série !");
                        btn_connection.Text = "Connection";
                        ctlListePorts.Enabled = true;
                        btn_ActualiserListePortSerie.Enabled = true;
                    }
                }
                catch (Exception E)
                {
                    Logger.GlobalLogger.error(E.Message.ToString());
                    ctlListePorts.Enabled = true;
                    btn_ActualiserListePortSerie.Enabled = true;
                }
            }
        }

        private void btn_connection_Click(object sender, EventArgs e)
        {
            switchSerialPort();
        }

        private void CB_Xbee_CheckedChanged(object sender, EventArgs e)
        {
            _IntelArt.SetXbeeApiMode(CB_Xbee.Checked);
        }
        #endregion
    }
}
