using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    public class AStar
    {
        // Positions pour le calcul
        private PositionElement     _positionDepartRobot;
        private PositionElement     _positionArriveeRobot;

        // Autres cubes a eviter
        private PositionElement     _positionCubeAEviter;
        // Eviter de passer dans les zones
        private PositionZone        _positionZoneAEviter;

        // Zone à ne pas dépasser
        private PositionZoneTravail _zoneTravail;

        // Trajectoire calculée
        private Track               _trajectoire;


    }
}
