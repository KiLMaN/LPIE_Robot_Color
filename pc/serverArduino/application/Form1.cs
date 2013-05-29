using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IA;
using video;

namespace application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Instanciation des composants 
            IntelArt IA = new IntelArt();
            VideoProg video = new VideoProg(ImageReel, ImgContour, numericUpDown1, LblFPS,Blobs);

            #region #### Liens Composants ####
            // Liens entre les composants
            // IA => Video
            IA.DrawPolylineEvent                += video.onDrawPolyline;
            // Video => IA
            video.OnUpdatePositionCubes         += IA.OnPositionUpdateCubes;
            video.OnUpdatePositionRobots        += IA.OnPositionUpdateRobots;
            video.OnUpdatePositionZones         += IA.OnPositionUpdateZones;
            video.OnUpdatePositionZoneTravail   += IA.OnPositionUpdateZoneTravail;
            #endregion

        }
    }
}
