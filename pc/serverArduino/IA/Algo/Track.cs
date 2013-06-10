using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using utils.Events;

namespace IA.Algo
{
    class Track
    {
        private List<PositionElement> _Positions;
        public List<PositionElement> Positions
        {
            get { return _Positions; }
        }

        public Track()
        {
            _Positions = new List<PositionElement>();
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
