using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;
using xbee.Communication;

namespace IA.Algo
{
    public class Follower
    {
        // Fabrication des tracés 
        private TrackMaker _TrackMaker;

        // Liste des robots
        private ArduinoManagerComm _ArduinoManager;

        public Follower(ArduinoManagerComm AM)
        {
            this._ArduinoManager = AM;
            _TrackMaker = new TrackMaker(AM);
        }

        public void checkPositionArduino(ArduinoBotComm robot)
        {
            if (robot == null)
                return;

            
        }


        // Calculate the distance between
        // point pt and the segment p1 --> p2.
        private double FindDistanceToSegment(PositionElement pt, PositionElement p1, PositionElement p2, out PositionElement closest)
        {
            int dx = p2.X - p1.X;
            int dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a point in the middle.
            if (t < 0)
            {
                closest = new PositionElement();
                closest.X = p1.X;
                closest.Y = p1.Y;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PositionElement();
                closest.X = p2.X;
                closest.Y = p2.Y;
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PositionElement();
                closest.X = (int)(p1.X + t * dx);
                closest.Y = (int)(p1.Y + t * dy);
                //closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
