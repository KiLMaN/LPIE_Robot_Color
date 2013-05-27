using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils.Events
{
    public class DrawPolylineEventArgs
    {
        private PolyligneDessin _polyligne;

        public DrawPolylineEventArgs(PolyligneDessin Polyligne)
        {
            _polyligne = Polyligne;
        }

        public PolyligneDessin Polyligne
        {get { return _polyligne; }}
    }
}
