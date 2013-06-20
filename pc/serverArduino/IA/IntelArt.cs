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

namespace IA
{
    public class IntelArt : IDisposable
    {
        #region #### Evenements ####
        // Evenement pour le dessins sur l'image 
        public event DrawPolylineEventHandler DrawPolylineEvent;// TODO : DO OR NOT

        public void OnPositionUpdateRobots(object sender,UpdatePositionRobotEventArgs args)
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

        // Liste pour l'affichage des arduino
        public ListView listAffichage;

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

            if(_Follower != null)
                _Follower.Stop();

            _Follower = null;
            _ArduinoManager = null;
            if(_AutomateComm != null)
                _AutomateComm.Dispose();
            _AutomateComm = null;
        }
        #endregion

        public delegate void dUpdateListAffichage();
        public void UpdateListAffichage()
        {
            listAffichage.Items.Clear();
            foreach (ArduinoBotIA Robot in _Follower.ListArduino)
            {
                if (_ArduinoManager != null)
                {
                    ArduinoBotComm RobotComm = _ArduinoManager.getArduinoBotById(Robot.ID);
                    ListViewItem master = new ListViewItem(Robot.ID + "");
                    master.SubItems.Add(RobotComm.Connected + "");
                    master.SubItems.Add(RobotComm.stateComm + "");
                    master.SubItems.Add(RobotComm.stateBot + "");
                    master.SubItems.Add(Robot.Position.X + "");
                    master.SubItems.Add(Robot.Position.Y + "");

                    listAffichage.Items.Add(master);
                }

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
                if (listAffichage != null)
                {
                    listAffichage.Invoke((dUpdateListAffichage)UpdateListAffichage);
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
            if(_AutomateComm != null)
                _AutomateComm.setXbeeApiMode(Mode);
        }
        #endregion
        
        #region #### IA ####
        public bool StartIA()
        {
           // StopIA();

            if (_Follower == null)
                return false;

            return _Follower.Start();
        }
        public void StopIA()
        {
            if (_Follower != null)
                _Follower.Stop();
            _Follower = null;
        }
        #endregion
    }
}
