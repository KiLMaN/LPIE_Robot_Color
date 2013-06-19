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
using AForge.Imaging;

using Emgu.CV;
using Emgu;
using Emgu.Util;
using AForge.Imaging.Filters;
using Emgu.CV.UI;
using Emgu.CV.Structure;
namespace video
{
    public class VideoProg : IDisposable
    {


        private List<IntPoint> LimiteTerrain = new List<IntPoint>();
        public static int tailleGlyph = 5;
        private FilterInfoCollection VideoCaptureDevices;
<<<<<<< HEAD
=======
        private VideoCaptureDevice FinalVideo = null;
>>>>>>> 6987d622693357d52a22998b1df6b54a938d0e39
        private ulong nbImageCapture = 0;
        private ulong imageShow = 0;
        private double[] ratioCmParPixel;

        private const int nbThread = 1;
        private int lastThread = 0;
        private Thread[] ListeThread = new Thread[nbThread];
        private ImgWebCam[] ListeImage = new ImgWebCam[nbThread];
        private Thread ThreadColor;
        private Thread ThreadClean;
        double millLastPic = 0;

        private BibliotequeGlyph Bibliotheque = new BibliotequeGlyph(tailleGlyph);
        private List<PolyligneDessin> polyline = new List<PolyligneDessin>();
        private List<PositionRobot> LstRobot = new List<PositionRobot>();
        private List<HSLFiltering> LstHslFiltering = new List<HSLFiltering>();
        private List<Rectangle> LstCube = new List<Rectangle>();

        private PictureBox imgReel = null;
        private PictureBox imgContour = null;
        private NumericUpDown numericUpDown1 = null;
        private Label FPS = null;


        public ImageBox imageDebug;

        private Capture _capture;

        #region ##### Initialisation #####
        public VideoProg(PictureBox imgR, PictureBox img2, NumericUpDown Filtre, Label fps)
        {
            this.imgReel = imgR;
            this.imgReel.MouseDown += new MouseEventHandler(ClicImage);
            this.imgContour = img2;
            this.FPS = fps;
            this.numericUpDown1 = Filtre;
            if(Logger.GlobalLogger == null)
                Logger.GlobalLogger = new Logger();
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
            polyline = s.ListPolyligne;
        }
        private void envoieListe(List<PositionRobot> lst)
        {
            if (OnUpdatePositionRobots == null)
                return;
            UpdatePositionRobotEventArgs a = new UpdatePositionRobotEventArgs(lst);
            OnUpdatePositionRobots(this, a);
        }
        private void envoieListe(List<PositionCube> lst)
        {
            if (OnUpdatePositionCubes == null)
                return;
            UpdatePositionCubesEventArgs a = new UpdatePositionCubesEventArgs(lst);
            OnUpdatePositionCubes(this, a);
        }
        private void envoieListe(List<PositionZone> lst)
        {
            if (OnUpdatePositionZones == null)
                return;
            UpdatePositionZonesEventArgs a = new UpdatePositionZonesEventArgs(lst);
            OnUpdatePositionZones(this, a);
        }
        private void envoieListe(PositionZoneTravail PositionTravail)
        {
            if (OnUpdatePositionZoneTravail == null)
                return;
            UpdatePositionZoneTravailEventArgs a = new UpdatePositionZoneTravailEventArgs(PositionTravail);
            OnUpdatePositionZoneTravail(this, a);
        }
        private void mergePosition(List<PositionRobot> LstTmp)
        {
            if (LstTmp.Count == 0)
                return;
            List<PositionRobot> ListEnvoi = new List<PositionRobot>();
            for (int i = 0; i < LstTmp.Count; i++)
            {
                bool Trouve = false;
                PositionRobot tmp = LstTmp[i];
                tmp.Position.X = (int)(ratioCmParPixel[0] * tmp.Position.X);
                tmp.Position.Y = (int)(ratioCmParPixel[1] * tmp.Position.Y);
                for (int j = 0; j < LstRobot.Count; j++)
                {
                    if (LstRobot[j].Identifiant == LstTmp[i].Identifiant)
                    {
                        IntPoint itmp = new IntPoint((int)(tmp.Position.X), (int)(tmp.Position.Y));
                        IntPoint p = new IntPoint(LstRobot[j].Position.X, LstRobot[j].Position.Y);
                        if (p.DistanceTo(itmp) > 2) //  Sueil a 2 cm
                        {
                            LstRobot[j] = tmp;
                            ListEnvoi.Add(tmp);
                        }
                        tmp.DerniereModification = DateTime.Now;
                        Trouve = true;
                        break;
                    }
                }
                if (Trouve == false)
                {
                    tmp.DerniereModification = DateTime.Now;
                    ListEnvoi.Add(tmp);
                    LstRobot.Add(tmp);
                }
            }
            if (ListEnvoi.Count > 0)
            {
                Logger.GlobalLogger.debug("" + ListEnvoi.Count);
                envoieListe(ListEnvoi);
            }
        }
        private void UpdateTailleTerain(int x, int y)
        {
            PositionZoneTravail Pzt = new PositionZoneTravail();
            Pzt.A.X = 0;
            Pzt.A.Y = 0;
            Pzt.B.X = (int)(x * ratioCmParPixel[0]);
            Pzt.B.Y = (int)(y * ratioCmParPixel[1]);
            envoieListe(Pzt);
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
        protected Boolean openWebCam(int NomCamera, int indexResolution)
        {
            LimiteTerrain.Clear();
            ratioCmParPixel = new double[2] { 1, 1 };
            /* Ouvre le flux vidéo et initialise le EventHandler */

            // TODO : selection de la caméra
            _capture = new Capture(); // Utiliser la webcam de base
            
            // Evenement lors de la reception d'une image
            _capture.ImageGrabbed += ProcessFrame;

            // Passage en MPG
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FOURCC, CvInvoke.CV_FOURCC('M', 'J', 'P', 'G'));
            // Resolution
            VideoCaptureDevice tmpVideo = new VideoCaptureDevice(VideoCaptureDevices[NomCamera].MonikerString);
 
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, tmpVideo.VideoCapabilities[indexResolution].FrameSize.Width);
            _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, tmpVideo.VideoCapabilities[indexResolution].FrameSize.Height);

            _capture.Start();

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
                catch (Exception e)
                {
                    Logger.GlobalLogger.error(e.Message);
                }
                Thread.Sleep(10);
            }

        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            //try
            {
                Image<Emgu.CV.Structure.Bgr, Byte> tmp = _capture.RetrieveBgrFrame();
                imageDebug.Image = tmp ;
                
                afficheImage(this, new NewFrameEventArgs(tmp.ToBitmap()));
            }
            /*catch (Exception e)
            {
                Logger.GlobalLogger.error(e.Message);
            }*/
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
        public delegate void TraitementImg(ImgWebCam img);
        public void imgTraitment(ImgWebCam img)
        {
            img.homographie(LimiteTerrain);
            img.ColeurVersNB();
            img.DetectionContour((short)numericUpDown1.Value);

            if (ratioCmParPixel[0] == 1 || ratioCmParPixel[1] == 1)
            {
                double[] tmp = img.detectionGlyph(true);
                if (tmp != null)
                {
                    ratioCmParPixel = tmp;
                    int[] TailleTerain = img.getTailleTerrain(tmp[0], tmp[1]);
                    Logger.GlobalLogger.info("Taille terrain : " + TailleTerain[0] + " x " + TailleTerain[1] + " cm");
                }
            }
            else
            {
                img.detectionGlyph(false);
            }
                if (imageShow < img.getNumeroImg())
                {
                    mergePosition(img.getLstRobot());
                    if (polyline !=null && polyline.Count > 0)
                    {
                        img.dessinePolyline(polyline);
                    }
                    if(LstCube.Count > 0 )
                         img.dessineRectangle(LstCube, Color.Yellow);
                    imageShow = img.getNumeroImg();
                    if (imageShow % 3 == 0)
                    {
                        ThreadColor = new Thread(detectionColor);
                        ThreadColor.Start(img);
                    }
                    if (imgReel != null)
                    {
                        //imageDebug.Image = img.getUnImgReel().ToManagedImage();
                        // imageDebug.Image = new Emgu.CV.Image<Bgr, Byte>(img.getUnImgReel().ToManagedImage());
                        imgReel.Invoke((affichageImg)imgAffiche, img.getUnImgReel().ToManagedImage(), imgReel);
                    }
                }
        }
        private void detectionColor(Object s)
        {
            LstCube = ((ImgWebCam)s).getImageColor(LstHslFiltering);
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
                int w_i = ((PictureBox)sender).Image.Width;
                int h_i = ((PictureBox)sender).Image.Height;
                int w_c = ((PictureBox)sender).Width;
                int h_c = ((PictureBox)sender).Height;
                float imageRatio = w_i / (float)h_i; // image W:H ratio
                float containerRatio = w_c / (float)h_c; // container W:H ratio
                int ord, abs;

                if (imageRatio >= containerRatio)
                {
                    // horizontal image
                    float scaleFactor = w_c / (float)w_i;
                    float scaledHeight = h_i * scaleFactor;
                    // calculate gap between top of container and top of image
                    float filler = Math.Abs(h_c - scaledHeight) / 2;
                    abs = (int)(e.X / scaleFactor);
                    ord = (int)((e.Y - filler) / scaleFactor);
                }
                else
                {
                    // vertical image
                    float scaleFactor = h_c / (float)h_i;
                    float scaledWidth = w_i * scaleFactor;
                    float filler = Math.Abs(w_c - scaledWidth) / 2;
                    abs = (int)((e.X - filler) / scaleFactor);
                    ord = (int)(e.Y / scaleFactor);
                }

                LimiteTerrain.Add(new IntPoint(abs, ord));
                if (LimiteTerrain.Count == 4)
                {
                    ratioCmParPixel[0] = 1;
                    ratioCmParPixel[1] = 1;
                }
                
            }
            else
            {
                LimiteTerrain.Clear();
                ratioCmParPixel[0] = 1;
                ratioCmParPixel[1] = 1;
            }
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
        public void ClicImage(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ImageReel_MouseDown( sender, e);
            }
            else if (e.Button == MouseButtons.Left)
            {
                addcouleur(sender, e);
            }
        }
        public void addcouleur(object sender, MouseEventArgs e)
        {
            int w_i = ((PictureBox)sender).Image.Width;
            int h_i = ((PictureBox)sender).Image.Height;
            int w_c = ((PictureBox)sender).Width;
            int h_c = ((PictureBox)sender).Height;
            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio
            int ord, abs;

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                abs = (int)(e.X / scaleFactor);
                ord = (int)((e.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                abs = (int)((e.X - filler) / scaleFactor);
                ord = (int)(e.Y / scaleFactor);
            }

            Bitmap bm = new Bitmap(((PictureBox)sender).Image);
            Color tmp = bm.GetPixel(abs, ord);
            
            HSL col = HSL.FromRGB(new RGB(tmp));
            Logger.GlobalLogger.debug("Couleur ajoutée : " + tmp.ToString() + " HLS : " + col.Hue + " " + col.Luminance + " " + col.Saturation);

            HSLFiltering Filter = new HSLFiltering();
            Filter.Hue = new IntRange( (col.Hue + 340) % 360, (col.Hue + 20)%360 );
            Filter.Saturation = new Range(0.6f, 1f);
            Filter.Luminance = new Range(0.1f, 1f);
            LstHslFiltering.Add(Filter);
        }
        public void openVideoFlux(int indexCam, int IndexResolution)
        {
            LstCube.Clear();
            if( _capture != null)
                _capture.Stop();
            /* Demande le démarage du flux video de la WebCam */
            if (_capture != null)
                _capture.Stop();

           openWebCam(indexCam,IndexResolution);

            // Initialisation des threads
            for (int i = 0; i < nbThread; i++)
            {
                ListeThread[i] = new Thread(TraitementThread);
                ListeThread[i].Start(i);
            }

            // Initialisation du Thread de nettoyage
            ThreadClean = new Thread(clean);
            ThreadClean.Start();
        }
        public void closeVideoFlux()
        {
            if( ThreadColor.IsAlive )
                ThreadColor.Abort();
            _capture.Stop();
            for (int i = 0; i < nbThread; i++)
            {
                ListeThread[i].Abort();
                ListeImage[i] = null;
            }
            ThreadClean.Abort();
            nbImageCapture = 0;
            imageShow = 0;
        }

        protected void clean()
        {
            List<PositionRobot> pos = new List<PositionRobot>();
            while (true)
            {
                pos.Clear();
                int i = 0;
                TimeSpan t;
                /* Nettoye les glyphs disparut de la bibliotheque de Glyphs */
                foreach (PositionRobot tmp in LstRobot)
                {
                    t =  DateTime.Now - tmp.DerniereModification;
                    if (t.Seconds > 10)
                    {
                        i++;
                    }
                    else
                    {
                        pos.Add(tmp);
                    }
                }
                if (i > 0)
                {
                    LstRobot = pos;
                    Logger.GlobalLogger.debug("Suppression de " + i + " Glyphs");
                }
                
                Thread.Sleep(10000);
            }
        }
        public void Dispose()
        {
            if (_capture != null)
                _capture.Stop();

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
