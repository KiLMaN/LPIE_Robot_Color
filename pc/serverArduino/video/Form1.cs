using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using utils;

using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge;
using System.Collections.Generic;
using utils.Events;


namespace video
{
    public partial class Form1 : Form
    {
        private VideoProg VP;
        public Form1()
        {
            InitializeComponent();
            Logger l = new Logger();
            l.attachToRTB(Log);
            Logger.GlobalLogger = l;
            VP = new VideoProg(ImageReel, ImgContour, numericUpDown1,LblFPS);
            VP.imageDebug = imageBox1;
            VP.ListerWebCam(ListeWebCam,Resolution);
            this.FormClosing += new FormClosingEventHandler(Form1Close);
            
        }
        public void Form1Close(object e, FormClosingEventArgs s)
        {
            VP.Dispose();
        }
        #region ##### Gestions des actions de la fenêtre #####
        private void ValideCamera_Click(object sender, EventArgs e)
        {
            VP.openVideoFlux(ListeWebCam.SelectedIndex,Resolution.SelectedIndex);
        }
        private void BtnStop_Click(object sender, EventArgs e)
        {
            VP.closeVideoFlux();
        }
        private void ListeWebCam_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* Chargement des nouvelles résolutions de la caméra sélectionnée */
            VP.chargementListeResolution(ListeWebCam.SelectedIndex,Resolution);
        }
        #endregion 
       

    }
}
