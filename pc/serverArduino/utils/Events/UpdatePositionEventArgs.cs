﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils.Events
{
    public struct PositionElement
    {
        // Positions en milimetres par rapport au coin Haut / Gauche de l'image
        public int X;
        public int Y;
    }

    #region #### Positions Robots ####
    public struct PositionRobot
    {
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
    public class UpdatePositionCubesEventArgs : EventArgs
    {

        private List<PositionElement> _listeCube;

        public UpdatePositionCubesEventArgs(List<PositionElement> Cubes)
        {
            _listeCube = Cubes;
        }

        public List<PositionElement> Cubes
        {
            get { return _listeCube; }
        }
    }
    #endregion

    #region #### Positions Zones ####
    public struct PositionZone
    {
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