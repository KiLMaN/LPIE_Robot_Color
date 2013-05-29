using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge;
using AForge.Video.DirectShow;
using System.Threading;
using utils;
using AForge.Video;
using System.Windows.Forms;
using utils.Events;
using System.Drawing;

namespace video
{
    public class VideoProg : IDisposable
    {
        private List<IntPoint> LimiteTerrain = new List<IntPoint>();
        public static int tailleGlyph = 5;
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private ulong nbImageCapture = 0;
        private ulong imageShow = 0;

        private const int nbThread = 1;
        private int lastThread = 0;
        private Thread[] ListeThread = new Thread[nbThread];
        private ImgWebCam[] ListeImage = new ImgWebCam[nbThread];

        private Logger gLogger = new Logger();
        private Thread ThreadClean;
        double millLastPic = 0;

        private BibliotequeGlyph Bibliotheque = new BibliotequeGlyph(tailleGlyph);

        private PictureBox imgReel = null;
        private PictureBox imgContour = null;
        private NumericUpDown numericUpDown1 = null;
        private Label FPS = null;
        private Label Debug = null;

        #region ##### Initialisation #####
        public VideoProg(PictureBox imgR, PictureBox img2, NumericUpDown Filtre, Label fps, Label d)
        {
            this.imgReel = imgR;
            this.imgReel.MouseDown += new MouseEventHandler(ImageReel_MouseDown);
            this.imgContour = img2;
            this.FPS = fps;
            this.numericUpDown1 = Filtre;
            this.Debug = d;
            Logger.GlobalLogger = gLogger;
            Bibliotheque.chargementListeGlyph();
        }
        #endregion
        #region ##### Communication #####
        public event UpdatePositionRobotEventHandler OnUpdatePositionRobots;
        public event UpdatePositionCubesEventHandler OnUpdatePositionCubes;
        public event UpdatePositionZonesEventHandler OnUpdatePositionZones;
        public event UpdatePositionZoneTravailEventHandler OnUpdatePositionZoneTravail;

        public void onDrawPolyline(object sender, DrawPolylineEventArgs s)
        {

        }
        private void envoieListe(List<PositionRobot> lst)
        {
            UpdatePositionRobotEventArgs a = new UpdatePositionRobotEventArgs(lst);
            OnUpdatePositionRobots(this, a);
        }
        private void envoieListe(List<PositionElement> lst)
        {
            UpdatePositionCubesEventArgs a = new UpdatePositionCubesEventArgs(lst);
            OnUpdatePositionCubes(this, a);
        }
        private void envoieListe(List<PositionZone> lst)
        {
            UpdatePositionZonesEventArgs a = new UpdatePositionZonesEventArgs(lst);
            OnUpdatePositionZones(this, a);
        }
        private void envoieListe(PositionZoneTravail PositionTravail)
        {
            UpdatePositionZoneTravailEventArgs a = new UpdatePositionZoneTravailEventArgs(PositionTravail);
            OnUpdatePositionZoneTravail(this, a);
        }
        #endregion

        #region ##### Bibliotheque Glyphs #####
        protected void cleanBibliothequeGlyph()
        {
            /* Nettoye les glyphs disparut de la bibliotheque de Glyphs */
            // TODO: Netoyer la bibliotheque de glyph
            Thread.Sleep(2000);
        }
        #endregion

        #region  ##### WebCam #####

        public void ListerWebCam(ComboBox lstWebCam, ComboBox LstResolution)
        {
            /* Retourne la liste des webcams connecté en usb */
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            lstWebCam.Items.Clear();
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                lstWebCam.Items.Add(VideoCaptureDevice.Name);
            }

            if (lstWebCam.Items.Count > 0)
            {
                lstWebCam.SelectedIndex = 0;
                chargementListeResolution(0, LstResolution);
            }
        }
        public void chargementListeResolution(int NomCamera, ComboBox Resolution)
        {
            /* Charge la liste des résolutions disponibles en fonction de la WebCam selectionnée */
            VideoCaptureDevice tmpVideo = new VideoCaptureDevice(VideoCaptureDevices[NomCamera].MonikerString);
            Resolution.Items.Clear();
            foreach (VideoCapabilities Cap in tmpVideo.VideoCapabilities)
            {
                Resolution.Items.Add("" + Cap.FrameSize.Width + " * " + Cap.FrameSize.Height + " (" + Cap.FrameRate.ToString() + " FPS)");
            }
            if (Resolution.Items.Count > 0)
                Resolution.SelectedIndex = 0;

        }
        protected Boolean openWebCam(int NomCamera, int Resolution)
        {
            LimiteTerrain.Clear();
            /* Ouvre le flux vidéo et initialise le EventHandler */

            // Creation de la source vidéo
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[NomCamera].MonikerString);
            FinalVideo.DesiredFrameRate = FinalVideo.VideoCapabilities[Resolution].FrameRate;
            FinalVideo.DesiredFrameSize = FinalVideo.VideoCapabilities[Resolution].FrameSize;

            // Création du Eventhandler
            FinalVideo.NewFrame += new NewFrameEventHandler(afficheImage);
            FinalVideo.Start();

            // TODO: Test MJPEG

            if (FinalVideo.IsRunning == false)
            {
                MessageBox.Show("Erreur Ouverture camera");
                return false;
            }

            return true;
        }
        #endregion

        #region ##### Gestions des images  #####
        public void TraitementThread(object id)
        {
            while (true)
            {
                // Traitement et Affichage des images  
                try
                {
                    if (ListeImage[(int)id] != null)
                    {
                        imgTraitment(ListeImage[(int)id]);
                        ListeImage[(int)id] = null;
                    }
                }
                catch { }
                Thread.Sleep(10);
            }

        }
        private void afficheImage(object sender, NewFrameEventArgs eventArgs)
        {
            /* Affiche l'image recu par la WebCam */

            // Instancie un Thread
            ListeImage[lastThread] = new ImgWebCam((Bitmap)eventArgs.Frame.Clone(), nbImageCapture, tailleGlyph);
            lastThread++;
            lastThread %= nbThread;

            // Traitement et Affichage des images   
            try
            {
                if ((nbImageCapture % 10) == 0)
                {
                    FPS.Invoke((UpdateFPS)affichageFPS);
                   
                }
            }
            catch { }


            nbImageCapture++;
        }
        public delegate void Deb(string chaine);
        public void Deb2(string chaine)
        {
            Debug.Text = chaine;
        }
        public delegate void TraitementImg(ImgWebCam img);
        public void imgTraitment(ImgWebCam img)
        {
            img.homographie(LimiteTerrain);
            img.ColeurVersNB();
            img.DetectionContour((short)numericUpDown1.Value);
            Debug.Invoke((Deb)Deb2, "" + img.detectionGlyph());

            try
            {
                if (imageShow < img.getNumeroImg())
                {
                    imageShow = img.getNumeroImg();

                   if(imgContour !=null)
                        imgContour.Invoke((affichageImg)imgAffiche, img.getImageContour().ToManagedImage(), imgContour);
                   if(imgReel != null)
                        imgReel.Invoke((affichageImg)imgAffiche, img.getUnImgReel().ToManagedImage(), imgReel);
                }

            }
            catch { }
        }

        public delegate void affichageImg(Bitmap img, PictureBox box);
        public void imgAffiche(Bitmap img, PictureBox box)
        {
            /* Affichage de l'image dans la PictureBox*/
            box.Image = img;
        }
        #endregion

        #region ##### Définition terrain #####

        private void ImageReel_MouseDown(object sender, MouseEventArgs e)
        {
            if (LimiteTerrain.Count < 4)
            {
                int abs = (e.Location.X * ((PictureBox)sender).Image.Width) / ((PictureBox)sender).Width;
                int ord = (e.Location.Y * ((PictureBox)sender).Image.Height) / ((PictureBox)sender).Height;

                LimiteTerrain.Add(new IntPoint(abs, ord));
            }
            else
                LimiteTerrain.Clear();
        }

        #endregion

        #region ##### Gestions des FPS #####
        public delegate void UpdateFPS();
        public void affichageFPS()
        {
            double delaisImage = DateTime.Now.TimeOfDay.TotalMilliseconds - millLastPic;
            millLastPic = DateTime.Now.TimeOfDay.TotalMilliseconds;

            double FPS2 = 1 / delaisImage * 1000 * 10 + 1;

            FPS.Text = FPS2 + " FPS";
        }
        #endregion

        #region #### A definir #####
        public void openVideoFlux(int indexCam, int IndexResolution)
        {
            int i;

            /* Demande le démarage du flux video de la WebCam */
            if (FinalVideo != null && FinalVideo.IsRunning)
                FinalVideo.SignalToStop();

           openWebCam(indexCam,IndexResolution);

            // Initialisation des threads
            for (i = 0; i < nbThread; i++)
            {
                ListeThread[i] = new Thread(TraitementThread);
                ListeThread[i].Start(i);
            }

            // Initialisatio du Thread de nettoyage
            ThreadClean = new Thread(cleanBibliothequeGlyph);
            ThreadClean.Start();
        }
        public void closeVideoFlux()
        {
            if (FinalVideo != null && FinalVideo.IsRunning)
            {
                FinalVideo.SignalToStop();
                for (int i = 0; i < nbThread; i++)
                {
                    ListeThread[i].Abort();
                    ListeImage[i] = null;
                }
                ThreadClean.Abort();
                nbImageCapture = 0;
                imageShow = 0;
            }
        }
        public void Dispose()
        {
            if (FinalVideo != null && FinalVideo.IsRunning == true)
                FinalVideo.SignalToStop();

            for (int i = 0; i < ListeThread.Length; i++)
            {
                try
                {
                    if (ListeThread[i].IsAlive)
                        ListeThread[i].Abort();
                }
                catch { };
            }
        }
        #endregion
    }
}
