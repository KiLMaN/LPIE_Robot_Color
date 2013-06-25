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
using utils.Events;
using IA.Algo;
using IA.Algo.AStarAlgo;


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
            g_Logger.levelDebug = 0;
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


        private void dessinerLigne(Bitmap bmp,Point a,Point b,Color couleur,int taille = 3)
        {
            Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(bmp);
            Pen myPen = new Pen(couleur, taille);
            graphicsObj.DrawLine(myPen, a, b);
            graphicsObj.Dispose();
        }
        private void dessinerPoint(Bitmap bmp, Point a, Brush couleur)
        {
            Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(bmp);
            Pen myPen = new Pen(couleur, 1);
            Brush brush = couleur;
            graphicsObj.FillEllipse(brush, a.X - 5, a.Y - 5,10, 10);
            graphicsObj.Dispose();
        }

        private void dessinerRectangle(Bitmap bmp,  Rectangle a,Brush couleur)
        {
            Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(bmp);
            Pen myPen = new Pen(couleur, 1);
            Brush brush = couleur;
            graphicsObj.FillRectangle(brush,a );
            graphicsObj.Dispose();
        }

        private void dessinerTrack(Bitmap bmp, Track Trace)
        {
            if (Trace == null || Trace.Positions.Count < 2)
                return;
            for (int i = 1; i < Trace.Positions.Count; i++)
            {
                Point a = new Point(Trace.Positions[i - 1].X + (int)(astar.UnitCol / 2), Trace.Positions[i - 1].Y + (int)(astar.UnitRow / 2));
                Point b = new Point(Trace.Positions[i].X + (int)(astar.UnitCol / 2), Trace.Positions[i].Y + (int)(astar.UnitRow / 2));
                dessinerPoint(bmp, a,Brushes.BlueViolet);
               dessinerLigne(bmp, a, b,Color.Red);
               
               
            }
            try
            {
                Point c = new Point(Trace.Positions[Trace.Positions.Count - 1].X + (int)(astar.UnitCol / 2), Trace.Positions[Trace.Positions.Count - 1].Y + (int)(astar.UnitRow / 2));
                dessinerPoint(bmp, c, Brushes.BlueViolet);
            }
            catch (Exception)
            {
            }

        }

        private void btn_test_Click(object sender, EventArgs e)
        {
            PositionElement pStart = new PositionElement();
            pStart.X = 30; pStart.Y = 550;

            PositionElement pEnd = new PositionElement();
            pEnd.X = 30; pEnd.Y = 30;

            PositionZoneTravail pZone = new PositionZoneTravail();
            pZone.A.X = 0;
            pZone.A.Y = 0;
            pZone.B.X = 800;
            pZone.B.Y = 600;


            //AStar astar = new AStar(pStart, pEnd, pZone);
            //Track tr =  astar.CalculerTrajectoire();


            Bitmap bitmap = new Bitmap(pZone.B.X, pZone.B.Y);
            //dessinerTrack(bitmap, tr.Positions);
            pictureBox1.Image = bitmap;

           /* Point c = new Point(pEnd.X, pEnd.Y);
            dessinerPoint(bitmap, c, Brushes.Red);

            c = new Point(pStart.X, pStart.Y);
            dessinerPoint(bitmap, c, Brushes.Green);*/
            
        }

        PositionElement pStart = new PositionElement();
        PositionElement pEnd = new PositionElement();
        List<PositionElement> pAutre = new List<PositionElement>();
        AStar astar;
        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            int abs = (e.Location.X * ((PictureBox)sender).Image.Width) / ((PictureBox)sender).Width;
            int ord = (e.Location.Y * ((PictureBox)sender).Image.Height) / ((PictureBox)sender).Height;

            //Logger.GlobalLogger.debug("ABS : " + abs + " ORD : " + ord);
            if (e.Button == MouseButtons.Left)
            {
                pEnd.X = abs;
                pEnd.Y = ord;
            }
            else if (e.Button == MouseButtons.Right)
            {
                pStart.X = abs;
                pStart.Y = ord;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                PositionElement pObstacle = new PositionElement();
                pObstacle.X = abs;
                pObstacle.Y = ord;
                pAutre.Add(pObstacle);
            }
     

            PositionZoneTravail pZone = new PositionZoneTravail();
            pZone.A.X = 0;
            pZone.A.Y = 0;
            pZone.B.X = 800;
            pZone.B.Y = 600;


            astar = new AStar(pStart, pEnd, pZone);
            astar.AddObstacles(pAutre);

            DateTime dt1 = DateTime.Now;
            Track tr = astar.CalculerTrajectoire();
            DateTime dt2 = DateTime.Now;
            Logger.GlobalLogger.info("Temps calcul : " + (dt2 - dt1).Milliseconds);
            //Track tr = astar.CalculerTrajectoire();
                tr.nettoyerTrajectoire();
            List<QuadrillageCoord> quad = astar.CalculerQuadrillage();

            


            Bitmap bitmap = new Bitmap(pZone.B.X, pZone.B.Y);
            dessinerTrack(bitmap, tr);
            

            
            Point c = new Point(pEnd.X, pEnd.Y);
            //dessinerPoint(bitmap, c, Brushes.Red);
            dessinerRectangle(bitmap, astar.CalculerRectangle(pEnd), Brushes.Red);
            c = new Point(pStart.X, pStart.Y);
            //dessinerPoint(bitmap, c, Brushes.Green);
            
            dessinerRectangle(bitmap,astar.CalculerRectangle(pStart),Brushes.Green);
            foreach (PositionElement obstacle in pAutre)
            {
                dessinerRectangle(bitmap, astar.CalculerRectangle(obstacle), Brushes.Gray);
                //dessinerPoint(bitmap, c, Brushes.Gray);
            }
            foreach (QuadrillageCoord q in quad)
            {
                dessinerLigne(bitmap, q.A, q.B, Color.Gray, 1);
            }

            pictureBox1.Image = bitmap;
        }
    }
}
