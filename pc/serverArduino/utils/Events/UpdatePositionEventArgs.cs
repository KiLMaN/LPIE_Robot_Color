using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils.Events
{
    #region #### Evennements ####
    public delegate void UpdatePositionRobotEventHandler(object sender, UpdatePositionRobotEventArgs e);
    public delegate void UpdatePositionCubesEventHandler(object sender, UpdatePositionCubesEventArgs e);
    public delegate void UpdatePositionZonesEventHandler(object sender, UpdatePositionZonesEventArgs e);
    public delegate void UpdatePositionZoneTravailEventHandler(object sender, UpdatePositionZoneTravailEventArgs e);
    #endregion

    public struct PositionElement
    {
        // Positions en milimetres par rapport au coin Haut / Gauche de l'image
        public int X;
        public int Y;

        public static implicit operator System.Drawing.Point(PositionElement d)  // Conversion Implicite vers Point pour affichage 
        {
            return new System.Drawing.Point(d.X, d.Y);
        }
        public override string ToString()
        {
            return "[" + X + ";" + Y + "]";
        }
    }

    #region #### Positions Robots ####
    public struct PositionRobot
    {
        // Identifiants du robots
        public int Identifiant;
        // Index de la dernière image ou il est present
        public DateTime DerniereModification;
        // Positions en milimetres par rapport au coin Haut / Gauche de l'image
        public PositionElement Position;
        // Angle en degré par rapport Vertical Nord de l'image (-180 Anti horaire , +180 Horaire)
        public float Angle;
    }

    public class UpdatePositionRobotEventArgs : EventArgs
    {
        private List<PositionRobot> _listeRobot;

        public UpdatePositionRobotEventArgs(List<PositionRobot> Robots)
        {
            _listeRobot = Robots;
        }

        public List<PositionRobot> Robots
        {
            get { return _listeRobot; }
        }
    }
    #endregion

    #region #### Positions Cubes ####
    public struct PositionCube
    {
        // Identifiant du cube
        public int ID;
        // Identifiant de la zone de dépose ascociée
        public int IDZone;
        // Positions de chacun des points du contour de la zone
        public PositionElement Position;

    }
    public class UpdatePositionCubesEventArgs : EventArgs
    {

        private List<PositionCube> _listeCube;

        public UpdatePositionCubesEventArgs(List<PositionCube> Cubes)
        {
            _listeCube = Cubes;
        }

        public List<PositionCube> Cubes
        {
            get { return _listeCube; }
        }
    }
    #endregion

    #region #### Positions Zones ####
    public struct PositionZone
    {
        // Identifiant de la zone
        public int ID;
        // Positions de chacun des points du contour de la zone
        public PositionElement A;
        public PositionElement B;
        public PositionElement C;
        public PositionElement D;
    }

    public class UpdatePositionZonesEventArgs : EventArgs
    {
        private List<PositionZone> _listeZone;

        public UpdatePositionZonesEventArgs(List<PositionZone> Zones)
        {
            _listeZone = Zones;
        }

        public List<PositionZone> Zones
        {
            get { return _listeZone; }
        }
    }
    #endregion

    #region #### Zone de Travail ####
    public struct PositionZoneTravail
    {
        // Positions de chacun des points du contour de la zone
        public PositionElement A;
        public PositionElement B;
    }

    public class UpdatePositionZoneTravailEventArgs : EventArgs
    {
        private PositionZoneTravail _Zone;

        public UpdatePositionZoneTravailEventArgs(PositionZoneTravail Zone)
        {
            _Zone = Zone;
        }

        public PositionZoneTravail Zone
        {
            get { return _Zone; }
        }
    }
    #endregion
}
