using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils.Events
{
    #region #### Evenement ####
    public delegate void DrawPolylineEventHandler(object sender, DrawPolylineEventArgs e);
    #endregion

    public class DrawPolylineEventArgs
    {
        private List<PolyligneDessin> _ListPolylignes;

        public DrawPolylineEventArgs(List<PolyligneDessin> ListPolyligne)
        {
            _ListPolylignes = ListPolyligne;
        }

        public List<PolyligneDessin> ListPolyligne
        { get { return _ListPolylignes; } }
    }
}
