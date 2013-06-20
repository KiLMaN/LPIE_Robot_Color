﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IA;
using video;
using utils;
using System.IO.Ports;

namespace application
{
    public partial class Form1 : Form
    {
        IntelArt IA; // IA
        VideoProg video; // Video

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(ClosingForm); // Fermeture de la fenetre

            // Debug
            Logger Log = new Logger();
            Logger.GlobalLogger = Log;
            Log.attachToRTB(RTB_LOG);
            Log.levelDebug = 1; // Afichage de tout les niveaux de debug

            // Port Serie
            getListePortSerie(ctlListePorts);

            // Instanciation des composants 
            IA = new IntelArt();
            IA.listAffichage = ListArduino;
            video = new VideoProg(ImageReel, ImgContour, numericUpDown1, LblFPS);

            #region #### Liens Composants ####
            // Liens entre les composants
            // IA => Video
            IA.DrawPolylineEvent += video.onDrawPolyline;
            // Video => IA
            video.OnUpdatePositionCubes += IA.OnPositionUpdateCubes;
            video.OnUpdatePositionRobots += IA.OnPositionUpdateRobots;
            video.OnUpdatePositionZones += IA.OnPositionUpdateZones;
            video.OnUpdatePositionZoneTravail += IA.OnPositionUpdateZoneTravail;
            #endregion



        }

        public void ClosingForm(object sender, FormClosingEventArgs e)
        {
            if (video != null)
            {
                video.closeVideoFlux();
                video.Dispose();
            }
            video = null;
            if (IA != null)
            {
                IA.StopIA();
                IA.CloseSerialPort();
            }
            IA = null;
        }


        #region #### IA ####
        private void BTN_STOP_IA_Click(object sender, EventArgs e)
        {
            IA.StopIA();
            BTN_START_IA.Enabled = true;
            BTN_STOP_IA.Enabled = false;
        }

        private void BTN_START_IA_Click(object sender, EventArgs e)
        {
            if (IA.StartIA())
            {
                BTN_START_IA.Enabled = false;
                BTN_STOP_IA.Enabled = true;
            }
        }
        #endregion

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

        /** Ouvre le port série séléctionné */
        private void switchSerialPort()
        {
            // Déjà ouvert //
            if (IA.IsSerialPortOpen())
            {
                Logger.GlobalLogger.info("Fermeture du port serie !");
                IA.CloseSerialPort();
                btn_connection.Text = "Connection";
                ctlListePorts.Enabled = true;
                btn_ActualiserListePortSerie.Enabled = true;
            }
            else
            {
                try
                {
                    Logger.GlobalLogger.info("Ouverture du port : " + (string)ctlListePorts.SelectedItem);
                    IA.OpenSerialPort((string)ctlListePorts.SelectedItem);
                    if (IA.IsSerialPortOpen())
                    {
                        IA.SetXbeeApiMode(CB_Xbee.Checked);
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

        /* Actualiser la liste */
        private void btn_ActualiserListePortSerie_Click(object sender, EventArgs e)
        {
            getListePortSerie(ctlListePorts);
        }
        // Click sur le bouton de connexion au port serie
        private void btn_connection_Click(object sender, EventArgs e)
        {
            switchSerialPort();
        }
        // Changement du mode Xbee
        private void CB_Xbee_CheckedChanged(object sender, EventArgs e)
        {
            IA.SetXbeeApiMode(CB_Xbee.Checked);
        }
        #endregion
    }
}
