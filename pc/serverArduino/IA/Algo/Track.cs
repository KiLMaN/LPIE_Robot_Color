using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo.AStar
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
    }
}
