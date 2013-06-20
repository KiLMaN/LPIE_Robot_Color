using System;
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

namespace IA
{
    public class IntelArt : IDisposable
    {
        #region #### Evenements ####
        // Evenement pour le dessins sur l'image 
        public event DrawPolylineEventHandler DrawPolylineEvent;

        public void OnPositionUpdateRobots(object sender, UpdatePositionRobotEventArgs args)
        {
            _Follower.UpdatePositionRobots(args.Robots);
        }
        public void OnPositionUpdateCubes(object sender, UpdatePositionCubesEventArgs args)
        {
            _Follower.UpdatePositionCubes(args.Cubes);
        }
        public void OnPositionUpdateZones(object sender, UpdatePositionZonesEventArgs args)
        {
            _Follower.UpdatePositionZones(args.Zones);
        }
        public void OnPositionUpdateZoneTravail(object sender, UpdatePositionZoneTravailEventArgs args)
        {
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

        // Thread de mise a jour de la liste
        public Thread _tUpdate = null;

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
                    master.SubItems.Add(RobotComm.Connected + "");
                    master.SubItems.Add(RobotComm.stateComm + "");
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
        private void _threadUpdate()
        {
            while (true)
            {
                if (listAffichageArduino != null)
                {
                    listAffichageArduino.Invoke((dUpdateListAffichage)UpdateListAffichage);
                }
                Thread.Sleep(500);
            }
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
