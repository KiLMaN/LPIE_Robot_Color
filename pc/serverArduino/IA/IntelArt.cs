using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;
using IA.Algo;

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
            if(_Follower != null)
                _Follower.Stop();

            _Follower = null;
            _ArduinoManager = null;
            if(_AutomateComm != null)
                _AutomateComm.Dispose();
            _AutomateComm = null;
        }
        #endregion

        #region #### Communication Arduino ####
        public void OpenSerialPort(string SerialName)
        {
            _AutomateComm.OpenSerialPort(SerialName);
        }
        public void CloseSerialPort()
        {
            _AutomateComm.CloseSerialPort();
        }
        public bool IsSerialPortOpen()
        {
            return _AutomateComm.IsSerialPortOpen();
        }
        public void SetXbeeApiMode(bool Mode)
        {
            _AutomateComm.setXbeeApiMode(Mode);
        }
        #endregion
        
        #region #### IA ####
        public void StartIA()
        {
            StopIA();

            _ArduinoManager = new ArduinoManagerComm();
            _AutomateComm = new AutomateCommunication("COM0", true, _ArduinoManager);

            _Follower = new Follower(_ArduinoManager, _AutomateComm);
            _Follower.Start();
        }
        public void StopIA()
        {
            _ArduinoManager = null;
            if( _AutomateComm != null)
                _AutomateComm.Dispose();
            _AutomateComm = null;

            if (_Follower != null)
            _Follower.Stop();
        }
        #endregion
    }
}
