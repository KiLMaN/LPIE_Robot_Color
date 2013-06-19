using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    public class Track
    {
        private List<PositionElement> _Positions;
        private bool _valide;
        public List<PositionElement> Positions
        {
            get { return _Positions; }
        }
        public bool Valide
        {
            get { return _valide; }
            set { _valide = value; }
        }

        public Track()
        {
            _valide = false;
            _Positions = new List<PositionElement>();
        }
        public Track(List<PositionElement> points, bool valide)
            :this()
        {
            _valide = valide;
            _Positions = points;
        }


        public void ajouterPoint(PositionElement point)
        {
            _Positions.Add(point);
        }
        public void ajouterPoints(List<PositionElement> points)
        {
            _Positions.AddRange(points);
        }

        public void nettoyerTrajectoire()
        {
            int dirX = 0, dirY = 0, oldDirX = 0, oldDirY = 0; // Directions sur les axes X et Y
            List<PositionElement> Sortie = new List<PositionElement>();
            foreach (PositionElement p in _Positions)
            {
                if (Sortie.Count == 0 )
                    Sortie.Add(p);
                else
                {
                    dirX = Math.Sign(p.X - Sortie.Last().X);
                    dirY = Math.Sign(p.Y - Sortie.Last().Y);
                    if (dirX != oldDirX || dirY != oldDirY)
                    {
                        oldDirX = dirX;
                        oldDirY = dirY;
                        Sortie.Add(p);
                    }
                    else
                    {
                        PositionElement p2 = Sortie[Sortie.Count-1];
                        p2.X = p.X;
                        p2.Y = p.Y;
                        Sortie[Sortie.Count - 1] = p2;
                    }
                }
            }
            _Positions = Sortie;
        }

        public void removeBefore(PositionElement p)
        {
            this.Positions.RemoveRange(0, this.Positions.IndexOf(p) );
        }
    }
}
