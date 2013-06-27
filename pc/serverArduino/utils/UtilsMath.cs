using System;
using utils.Events;
using System.Drawing;

namespace utils
{
    public class UtilsMath
    {
        // Calcul la plus petite distance entre un segment et un point 
        static public double FindDistanceToSegment(PositionElement pt, PositionElement p1, PositionElement p2)
        {
            PositionElement closest;

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
        // Calcul de la distance Euclidienne SQRT(X²+Y²)
        static public double DistanceEuclidienne(PositionElement p1, PositionElement p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        // Calcul de la distance Manhattan (X+Y)
        static public double DistanceManhattan(PositionElement p1, PositionElement p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }
        // Calcul le centre d'un rectangle de dépose
        static public PositionElement CentreRectangle(PositionZone Zone)
        {
            int X = (Zone.A.X + Zone.C.X) / 2;
            int Y = (Zone.A.Y + Zone.C.Y) / 2;
           PositionElement p =  new PositionElement(X,Y);
            return p;
        }
        // Calcul de l'angle entre deux vecteurs (StartA,StopA) et (StartB,StopB) (retourne [-180,180])
        static public double AngleBetweenVectors(PointF StartA, PointF StopA, PointF StartB, PointF StopB)
        {
            PointF DeltaA = new PointF();
            PointF DeltaB = new PointF();

            double angle;

            // Calcul des deltas
            DeltaA.X = StartA.X - StopA.X;
            DeltaA.Y = StartA.Y - StopA.Y;

            DeltaB.X = StartB.X - StopB.X;
            DeltaB.Y = StartB.Y - StopB.Y;
            // Calcul du Cos avec le produit scalaire
            double cosa = (double)((DeltaA.X * DeltaB.X + DeltaA.Y * DeltaB.Y)) / (Math.Sqrt(DeltaA.X * DeltaA.X + DeltaA.Y * DeltaA.Y) * Math.Sqrt(DeltaB.X * DeltaB.X + DeltaB.Y * DeltaB.Y));

            // angle en degré
            angle = ((180.0 / Math.PI) * Math.Acos(cosa));

            // Signe de l'angle

            int sign = (DeltaA.X * DeltaB.Y - DeltaA.Y * DeltaB.X) > 0 ? 1 : -1;

            return angle * sign;
        }
        // Calcul de l'angle entre deux vecteurs (StartA,StopA) et (StartB,StopB) (retourne [0,360])
        static public double TrueAngleBetweenVectors(PointF StartA, PointF StopA, PointF StartB, PointF StopB)
        {
            double a = AngleBetweenVectors(StartA, StopA, StartB, StopB);
            a = (a + 360.0) % 360.0;
            return a;
        }
    }
}
