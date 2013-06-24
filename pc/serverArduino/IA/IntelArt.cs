﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;
using IA.Algo;
using System.Windows.Forms;
using xbee.Communication.Events;
using System.Threading;
using utils;
using System.Drawing;
using IA.Algo.AStarAlgo;

namespace IA
{
    public class IntelArt : IDisposable
    {
        #region #### Evenements ####
        // Evenement pour le dessins sur l'image 
        public event DrawPolylineEventHandler DrawPolylineEvent;

        public void OnPositionUpdateRobots(object sender, UpdatePositionRobotEventArgs args)
        {
            if(_Follower != null)
            _Follower.UpdatePositionRobots(args.Robots);
        }
        public void OnPositionUpdateCubes(object sender, UpdatePositionCubesEventArgs args)
        {
            if (_Follower != null)
            _Follower.UpdatePositionCubes(args.Cubes);
        }
        public void OnPositionUpdateZones(object sender, UpdatePositionZonesEventArgs args)
        {
            if (_Follower != null)
            _Follower.UpdatePositionZones(args.Zones);
        }
        public void OnPositionUpdateZoneTravail(object sender, UpdatePositionZoneTravailEventArgs args)
        {
            if (_Follower != null)
            _Follower.UpdatePositionZoneTravail(args.Zone);
        }

        #endregion

        /* Liste des Arduinos */
        ArduinoManagerComm _ArduinoManager;
        /* Automate pour la communication avec les robots */
        AutomateCommunication _AutomateComm;
        // Autotmate pour le calul/ suivi d'itinéraire
        Follower _Follower;

        // Listes pour l'affichage des robots, zones et cubes
        public ListView listAffichageArduino;
        public ListView listAffichageCubes;
        public ListView listAffichageZones;

        // Image de débuggage (tracé et positions )
        public PictureBox imageDebug;

        // Thread de mise a jour de la liste
        private Thread _tUpdate = null;

        #region #### Constructeurs / Destructeurs ####
        public IntelArt()
        {
        }
        ~IntelArt()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (_tUpdate != null)
                _tUpdate.Abort();
            _tUpdate = null;

            _Follower = null;
            _ArduinoManager = null;
            if (_AutomateComm != null)
                _AutomateComm.Dispose();
            _AutomateComm = null;
        }
        #endregion

        #region #### Image Debug ####
        private void dessinerLigne(Bitmap bmp, Point a, Point b, Color couleur, int taille = 3)
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
            graphicsObj.FillEllipse(brush, a.X - 5, a.Y - 5, 10, 10);
            graphicsObj.Dispose();
        }
        private void dessinerRectangle(Bitmap bmp, Rectangle a, Brush couleur)
        {
            Graphics graphicsObj;
            graphicsObj = Graphics.FromImage(bmp);
            Pen myPen = new Pen(couleur, 1);
            Brush brush = couleur;
            graphicsObj.FillRectangle(brush, a);
            graphicsObj.Dispose();
        }
        private void dessinerTrack(Bitmap bmp, Track Trace, AStar AS)
        {
            if (Trace == null || Trace.Positions.Count < 2)
                return;
            for (int i = 1; i < Trace.Positions.Count; i++)
            {
                Point a = new Point(Trace.Positions[i - 1].X + AS.UnitCol / 2, Trace.Positions[i - 1].Y + AS.UnitRow / 2);
                Point b = new Point(Trace.Positions[i].X + AS.UnitCol / 2, Trace.Positions[i].Y + AS.UnitRow / 2);
                dessinerPoint(bmp, a, Brushes.BlueViolet);
                dessinerLigne(bmp, a, b, Color.Red);


            }
            try
            {
                Point c = new Point(Trace.Positions[Trace.Positions.Count - 1].X + AS.UnitCol / 2, Trace.Positions[Trace.Positions.Count - 1].Y + AS.UnitRow / 2);
                dessinerPoint(bmp, c, Brushes.BlueViolet);
            }
            catch (Exception)
            {
            }

        }

        public delegate void dUpdateImageDebug();
        public void UpdateImageDebug()
        {
            if (_Follower.TrackMaker != null && _Follower.TrackMaker.ZoneTravail.B.X != 0)
            {
                Bitmap bitmap = new Bitmap(_Follower.TrackMaker.ZoneTravail.B.X, _Follower.TrackMaker.ZoneTravail.B.Y);
                //dessinerTrack(bitmap, tr);

                // Dessiner Cubes
                foreach (Objectif obstacle in _Follower.TrackMaker.Cubes)
                {
                    dessinerPoint(bitmap, obstacle.position, Brushes.Gray);
                }

                foreach (QuadrillageCoord q in _Follower.TrackMaker.CreerAstarQuadriallage().CalculerQuadrillage())
                {
                    dessinerLigne(bitmap, q.A, q.B, Color.Gray, 1);
                }

                imageDebug.Image = bitmap;
            }
        }
        #endregion

        public delegate void dUpdateListAffichage();
        public void UpdateListAffichage()
        {
            // Update Robots
            listAffichageArduino.Items.Clear();
            foreach (ArduinoBotIA Robot in _Follower.ListArduino)
            {
                if (_ArduinoManager != null)
                {
                    ArduinoBotComm RobotComm = _ArduinoManager.getArduinoBotById(Robot.ID);
                    ListViewItem master = new ListViewItem(Robot.ID + "");
                    if (RobotComm != null)
                    {
                        master.SubItems.Add(RobotComm.Connected + "");
                        master.SubItems.Add(RobotComm.stateComm + "");
                    }
                    else
                    {
                        master.SubItems.Add("N/A");
                        master.SubItems.Add("N/A");
                    }
                    
                    
                    master.SubItems.Add(Robot.LastAction + "");
                    master.SubItems.Add(Robot.Position.X + "");
                    master.SubItems.Add(Robot.Position.Y + "");
                    master.SubItems.Add(Robot.Angle + "");

                    listAffichageArduino.Items.Add(master);
                }
            }
            // Update Cubes
            listAffichageCubes.Items.Clear();
            foreach (Objectif cube in _Follower.TrackMaker.Cubes)
            {

                ListViewItem master = new ListViewItem(cube.id + "");
                master.SubItems.Add(cube.idZone + "");
                master.SubItems.Add(cube.Done + "");
                if (cube.Robot != null)
                    master.SubItems.Add(cube.Robot.ID + "");
                else
                    master.SubItems.Add("N/A");
                master.SubItems.Add(cube.position.X + "");
                master.SubItems.Add(cube.position.Y + "");

                listAffichageCubes.Items.Add(master);

            }

            // Update Zones
            listAffichageZones.Items.Clear();
            foreach (Zone zone in _Follower.TrackMaker.ZonesDepose)
            {
                ListViewItem master = new ListViewItem(zone.id + "");
                master.SubItems.Add("A : " + zone.position.A.ToString() + " B : " + zone.position.B.ToString() + " C : " + zone.position.C.ToString() + " D : " + zone.position.D.ToString());
                master.SubItems.Add(UtilsMath.CentreRectangle(zone.position).ToString());

                listAffichageZones.Items.Add(master);
            }
        }
        private void _threadUpdate()
        {
            while (true)
            {
                if (listAffichageArduino != null)
                {
                    // Mettre a jour l'affichage 
                    listAffichageArduino.Invoke((dUpdateListAffichage)UpdateListAffichage);
                }

                if (imageDebug != null)
                {
                    imageDebug.Invoke((dUpdateImageDebug)UpdateImageDebug);
                }
                Thread.Sleep(250);
            }
        }

        #region #### Communication Arduino ####
        public void OpenSerialPort(string SerialName)
        {
            _ArduinoManager = new ArduinoManagerComm();
            _AutomateComm = new AutomateCommunication(SerialName, true, _ArduinoManager);

            _AutomateComm.OpenSerialPort(SerialName);

            _Follower = new Follower(_ArduinoManager, _AutomateComm);

            _tUpdate = new Thread(new ThreadStart(_threadUpdate));
            _tUpdate.Start();
            //_AutomateComm.OnNewTrameArduinoReceived += new AutomateCommunication.NewTrameArduinoReceivedEventHandler(_AutomateComm_OnNewTrameArduinoReceived);
        }
        public void CloseSerialPort()
        {
            if (_tUpdate != null)
                _tUpdate.Abort();
            _tUpdate = null;

            if (_AutomateComm != null)
                _AutomateComm.CloseSerialPort();
            if (_AutomateComm != null)
                _AutomateComm.Dispose();
            _AutomateComm = null;

            _ArduinoManager = null;
        }
        public bool IsSerialPortOpen()
        {
            if (_AutomateComm == null)
                return false;
            return _AutomateComm.IsSerialPortOpen();
        }
        public void SetXbeeApiMode(bool Mode)
        {
            if (_AutomateComm != null)
                _AutomateComm.setXbeeApiMode(Mode);
        }
        #endregion


       
    }
}
