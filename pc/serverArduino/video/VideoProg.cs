using System;
using System.Collections.Generic;
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
using AForge.Imaging.Filters;
using Emgu.CV.UI;
namespace video
{
    //TODO: Suppression cube
    public class VideoProg : IDisposable
    {
        public class Cub
        {
            public Rectangle rec;
            public int Color;
            public Cub(Rectangle rectangl,int col)
            {
                rec = rectangl;
                Color = col;
            } 
        }
        private class ObjColor
        {
            public Rectangle contour;
            public int count;
            public DateTime DerniereVisualisation;
            public int Identifiant;
            public int Color;

            public ObjColor(Rectangle res, int Col,int idC)
            {
                Identifiant = idC;
                contour = res;
                count = 0;
                Color = Col;
                DerniereVisualisation = DateTime.Now;
            }
            public void Update(Rectangle res)
            {
                contour = res;
                count++;
                DerniereVisualisation = DateTime.Now;
            }
            public int GetSize()
            {
                return contour.Width * contour.Height;
            }
            public void setIdentifiant(int id)
            {
                Identifiant = id;
            }
            public Boolean isInclude(IntPoint milieu)
            {
                if (contour.X > milieu.X && contour.Right < milieu.X && contour.Y > milieu.Y && contour.Bottom < milieu.Y)
                    return true;
                return false;
            }
        }
        private List<IntPoint> LimiteTerrain = new List<IntPoint>();
        public static int tailleGlyph = 5;
        private FilterInfoCollection VideoCaptureDevices;

        //private VideoCaptureDevice FinalVideo = null;

        private ulong nbImageCapture = 0;
        private ulong imageShow = 0;
        private int lastIdCube = 0;
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
        private List<PositionCube> LstCubeColor = new List<PositionCube>();
        private List<HSLFiltering> LstHslFiltering = new List<HSLFiltering>();
        private List<ObjColor> LstCube = new List<ObjColor>();

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
        /* Fonction Robot */
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
                        if (p.DistanceTo(itmp) > 5) //  Sueil a 2 cm
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
        /* fonction cubes */
        private void mergePosition(List<Cub> lst)
        {
            if (lst.Count == 0)
                return;
          
            List<bool> tab = new List<bool>();
            for (int i = 0; i < LstCube.Count; i++)
                tab.Add(false);
            // Parcours de la liste des cubes recu par l'image
            for (int i = 0; i < lst.Count; i++)
            {
                bool trouv = false;
                // Si il y a déjà des cubes dans la liste
                if (LstCube.Count > 0)
                {
                    for (int j = 0; j < LstCube.Count; j++)
                    {
                        // Verification de la couleur
                        if (LstCube[j].Color == lst[i].Color)
                        {
                            IntPoint milieu = new IntPoint(lst[i].rec.X + lst[i].rec.Width / 2, lst[i].rec.Y + lst[i].rec.Height / 2);
                            float distance = milieu.DistanceTo(new IntPoint(LstCube[j].contour.X + LstCube[j].contour.Width / 2, LstCube[j].contour.Y + LstCube[j].contour.Height / 2));
                            if (distance < ( LstCube[j].contour.Width + LstCube[j].contour.Height) * 1.1) // Verification de la proximite
                            {
                                if (tab[j] == false)
                                {
                                    LstCube[j].Update(lst[i].rec);
                                    tab[j] = true;
                                }
                                trouv = true;
                                break;
                            }
                        }
                    }
                    
                }
                if (trouv == false) // Aucun cube correspondant => AJOUT DU CUBE
                {
                    LstCube.Add(new ObjColor(lst[i].rec, lst[i].Color, lastIdCube++));
                }
            }
            
            // Preparation de la liste pour envoie
            if (LstCube.Count > 0)
            {
                List<int> delete = new List<int>();
                List<PositionCube> lstpos = new List<PositionCube>();
                for (int i = 0; i < LstCube.Count; i++)
                {
                    PositionCube po = new PositionCube();
                    po.ID = LstCube[i].Identifiant;
                    po.IDZone = LstCube[i].Color;
                    po.Position = new PositionElement();
                    // Position en pixel
                    IntPoint milieu = new IntPoint(LstCube[i].contour.X + LstCube[i].contour.Width / 2, LstCube[i].contour.Y + LstCube[i].contour.Height / 2);
                    TimeSpan t = DateTime.Now - LstCube[i].DerniereVisualisation;
                    if (t.Seconds > 3)
                    {
                        po.Position.X = -1;
                        po.Position.Y = -1;
                        delete.Add(i);
                    }
                    else
                    {
                        po.Position.X = milieu.X;
                        po.Position.Y = milieu.Y;
                    }
                    
                    
                    lstpos.Add(po);
                }
                envoieListe(lstpos);
                if (delete.Count > 0)
                {
                    for(int i = delete.Count -1; i>=0;i--)
                    {
                        LstCube.RemoveAt(delete[i]);
                    }
                    Logger.GlobalLogger.debug("Suppression de " + delete.Count + " Cubes");
                }
            }
        }
        /* Fonction zone de travail */
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
                Image<Emgu.CV.Structure.Bgr, Byte> tmp = _capture.RetrieveBgrFrame();
                imageDebug.Image = tmp ;
                
                afficheImage(this, new NewFrameEventArgs(tmp.ToBitmap()));
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
                    //TODO: SUPPRIMER LA FONCTION
                   // mergePosition(img.getImageColor(LstHslFiltering));
                    if(LstCube.Count > 0 )
                         img.dessineRectangle(getRectCube(), Color.White);
                    imageShow = img.getNumeroImg();
                    if (imageShow % 3 == 0)
                    {
                        ThreadColor = new Thread(detectionColor);
                        ThreadColor.Start(img);
                    }
                    if (imgReel != null)
                    {
                        imgReel.Invoke((affichageImg)imgAffiche, img.getUnImgReel().ToManagedImage(), imgReel);
                    }
                }
        }
        private void detectionColor(Object s)
        {
            //TODO: REACTIVER FONCTION
            mergePosition(((ImgWebCam)s).getImageColor(LstHslFiltering));
            
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
        public int getPosition(int index)
        {
            for (int i = 0; i < LstCubeColor.Count; i++)
            {
                if (LstCubeColor[i].ID == index)
                {
                    return i;
                }
            }
            return -1;
        }
        public List<Rectangle> getRectCube()
        {
            if ( LstCube.Count == 0 )
                return null;
            List<Rectangle> tmp = new List<Rectangle>();
            foreach (ObjColor obj in LstCube)
            {
                tmp.Add(obj.contour);
            }
            return tmp;
        }
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
            float ord = ((float)w_i * (float)((float)e.X / (float) w_c));
            float abs = ((float)h_i * (float)((float)e.Y / (float)h_c));


            Bitmap bm = new Bitmap(((PictureBox)sender).Image);
            Color tmp = bm.GetPixel((int)ord, (int)abs);
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
            if (ThreadColor != null && ThreadColor.IsAlive)
                ThreadColor.Abort();

            if (_capture != null)
            {
                if (_capture.GrabProcessState == ThreadState.Running)
                    _capture.Stop();

                _capture.Dispose();
            }
            for (int i = 0; i < nbThread; i++)
            {
                if(ListeThread[i] != null)
                    ListeThread[i].Abort();
                ListeImage[i] = null;
            }
            if(ThreadClean !=null)
                ThreadClean.Abort();
            nbImageCapture = 0;
            imageShow = 0;
        }

        protected void clean()
        {
            List<PositionRobot> pos = new List<PositionRobot>();
            List<ObjColor> obj = new List<ObjColor>();
            while (true)
            {
                pos.Clear();
                obj.Clear();
                int i = 0;
                TimeSpan t;
                /* Nettoye les glyphs disparut de la bibliotheque de Glyphs */
                foreach (PositionRobot tmp in LstRobot)
                {
                    t =  DateTime.Now - tmp.DerniereModification;
                    if (t.Seconds > 3)
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
                Thread.Sleep(3000);
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
                    if (ListeThread[i] != null && ListeThread[i].IsAlive)
                        ListeThread[i].Abort();
                }
                catch { };
            }
        }
        #endregion
    }
}
