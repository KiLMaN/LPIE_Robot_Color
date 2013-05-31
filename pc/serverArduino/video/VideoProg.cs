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
        private double[] ratioCmParPixel;

        private const int nbThread = 3;
        private int lastThread = 0;
        private Thread[] ListeThread = new Thread[nbThread];
        private ImgWebCam[] ListeImage = new ImgWebCam[nbThread];

        private Thread ThreadClean;
        double millLastPic = 0;

        private BibliotequeGlyph Bibliotheque = new BibliotequeGlyph(tailleGlyph);
        private List<PolyligneDessin> polyline = new List<PolyligneDessin>();
        private List<PositionRobot> LstRobot = new List<PositionRobot>();
        
        private PictureBox imgReel = null;
        private PictureBox imgContour = null;
        private NumericUpDown numericUpDown1 = null;
        private Label FPS = null;
        private List<NumericUpDown> Param = new List<NumericUpDown>();

        #region ##### Initialisation #####
        public VideoProg(PictureBox imgR, PictureBox img2, NumericUpDown Filtre, Label fps, NumericUpDown a1)
        {

        }
        public VideoProg(PictureBox imgR, PictureBox img2, NumericUpDown Filtre, Label fps, NumericUpDown a1,NumericUpDown a2,NumericUpDown a3,NumericUpDown a4,NumericUpDown a5,NumericUpDown a6)
        {
            Param.Add(a1);
            Param.Add(a2);
            Param.Add(a3);
            Param.Add(a4);
            Param.Add(a5);
            Param.Add(a6);
            this.imgReel = imgR;
            this.imgReel.MouseDown += new MouseEventHandler(ImageReel_MouseDown);
            this.imgReel.MouseDoubleClick += new MouseEventHandler(addcouleur);
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
        private void envoieListe(List<PositionElement> lst)
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
            if (LstTmp == null)
                return;
            List<PositionRobot> ListEnvoi = new List<PositionRobot>();
            for (int i = 0; i < LstTmp.Count; i++)
            {
                bool Trouve = false;
                PositionRobot tmp = LstTmp[i];
                for (int j = 0; j < LstRobot.Count; j++)
                {
                    if (LstRobot[j].Identifiant == LstTmp[i].Identifiant)
                    {
                        IntPoint itmp = new IntPoint((int)(LstTmp[i].Position.X * ratioCmParPixel[0]),(int)(LstTmp[i].Position.Y * ratioCmParPixel[1]));
                        IntPoint p = new IntPoint(LstRobot[j].Position.X, LstRobot[j].Position.Y);
                        if (p.DistanceTo(itmp) > 2) //  Sueil a 2 cm
                        {
                            tmp.DerniereModification = DateTime.Now;
                            tmp.Position.X = itmp.X;
                            tmp.Position.Y = itmp.Y;
                            LstRobot[j] = tmp;
                            ListEnvoi.Add(tmp);
                        }
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
        protected Boolean openWebCam(int NomCamera, int Resolution)
        {
            LimiteTerrain.Clear();
            ratioCmParPixel = new double[2] { 1, 1 };
            /* Ouvre le flux vidéo et initialise le EventHandler */

            // Creation de la source vidéo
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[NomCamera].MonikerString);
            FinalVideo.DesiredFrameRate = FinalVideo.VideoCapabilities[Resolution].FrameRate;
            FinalVideo.DesiredFrameSize = FinalVideo.VideoCapabilities[Resolution].FrameSize;
            FinalVideo.DisplayPropertyPage(IntPtr.Zero);
            // Création du Eventhandler
            FinalVideo.NewFrame += new NewFrameEventHandler(afficheImage);
            FinalVideo.Start();

            // TODO: Test MJPEG

            if (FinalVideo.IsRunning == false)
            {
                MessageBox.Show("Erreur Ouverture camera");
                return false;
            }
            UpdateTailleTerain(FinalVideo.DesiredFrameSize.Width, FinalVideo.DesiredFrameSize.Height);
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


            if (ratioCmParPixel[0] == 1 && ratioCmParPixel[1] == 1)
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

                    imageShow = img.getNumeroImg();

                    if(imgContour !=null)
                        imgContour.Invoke((affichageImg)imgAffiche, img.getImageColor((int)Param[0].Value, (int)Param[1].Value, (int)Param[2].Value, (int)Param[3].Value, (int)Param[4].Value, (int)Param[5].Value).ToManagedImage(), imgContour);
                    if(imgReel != null)
                        imgReel.Invoke((affichageImg)imgAffiche, img.getUnImgReel().ToManagedImage(), imgReel);
                }
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
        public void addcouleur(object sender, MouseEventArgs e)
        {
           // int abs = (e.Location.X * ((PictureBox)sender).Image.Width) / ((PictureBox)sender).Width;
           // int ord = (e.Location.Y * ((PictureBox)sender).Image.Height) / ((PictureBox)sender).Height;
           // Color pixelColor = GetClickedPixel(e.Location);

           // Bitmap bitmap = (Bitmap)picturebox1.Image;
           // return bitmap.GetPixel(abs, ord);
        }
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
            ThreadClean = new Thread(clean);
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
