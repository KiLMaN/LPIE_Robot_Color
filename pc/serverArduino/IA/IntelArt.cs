using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;

namespace IA
{
    public class IntelArt : IDisposable
    {
        #region #### Evenements ####
        // Evenement pour le dessins sur l'image 
        public event DrawPolylineEventHandler DrawPolylineEvent;

        public void OnPositionUpdateRobots(object sender,UpdatePositionRobotEventArgs args)
        {
        }
        public void OnPositionUpdateCubes(object sender, UpdatePositionCubesEventArgs args)
        {
        }
        public void OnPositionUpdateZones(object sender, UpdatePositionZonesEventArgs args)
        {
        }
        public void OnPositionUpdateZoneTravail(object sender, UpdatePositionZoneTravailEventArgs args)
        {
        }

        #endregion

        /* Liste des Arduinos */
        ArduinoManager _ArduinoManager;
        /* Automate pour la communication avec les robots */
        AutomateCommunication _AutomateComm;

        #region #### Constructeurs / Destructeurs ####
        public IntelArt()
        {
            _ArduinoManager = new ArduinoManager();
            _AutomateComm = new AutomateCommunication("COM0", true, _ArduinoManager);

        }
        ~IntelArt()
        {
            Dispose();
        }
        public void Dispose()
        {
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
    }
}
