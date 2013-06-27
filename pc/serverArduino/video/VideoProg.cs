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

        private ulong nbImageCapture = 0;
        private ulong imageShow = 0;
        private int lastIdCube = 0;
        private double[] ratioCmParPixel;

        public int[] col = null;
        private int paire = 0;
        private const int nbImgColorTraiter = 2;
        private const int nbThread = 2;
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
        private List<PositionZone> LstZone = new List<PositionZone>();

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
            polyline = new List<PolyligneDessin>();
            //foreach (PolyligneDessin p in s.ListPolyligne)
            for (int a = 0 ; a < s.ListPolyligne.Count ; a++)
            {
                PolyligneDessin p = s.ListPolyligne[a];
                for (int i = 0; i < p.ListePoint.Count; i++)
                {
                    PointDessin pdtmp = p.ListePoint[i];
                    pdtmp.X = (int)(pdtmp.X  / ratioCmParPixel[0]);
                    pdtmp.Y = (int)(pdtmp.Y / ratioCmParPixel[1]);

                    p.ListePoint[i] = pdtmp;
                }
                polyline.Add(p);
            }
            //polyline = s.ListPolyligne;
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
            List<PositionZone> tmp = new List<PositionZone>();
            foreach (PositionZone p in lst)
            {
                PositionZone pp = p;
                pp.A.X = (int)(pp.A.X*ratioCmParPixel[0]);
                pp.A.Y = (int)(pp.A.Y* ratioCmParPixel[1]);

                pp.B.X = (int)(pp.B.X * ratioCmParPixel[0]);
                pp.B.Y = (int)(pp.B.Y * ratioCmParPixel[1]);

                pp.C.X = (int)(pp.C.X * ratioCmParPixel[0]);
                pp.C.Y = (int)(pp.C.Y * ratioCmParPixel[1]);

                pp.D.X = (int)(pp.D.X * ratioCmParPixel[0]);
                pp.D.Y = (int)(pp.D.Y * ratioCmParPixel[1]);
                tmp.Add(pp);
            }
            UpdatePositionZonesEventArgs a = new UpdatePositionZonesEventArgs(tmp);
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
                //Logger.GlobalLogger.debug("" + ListEnvoi.Count);
                envoieListe(ListEnvoi);
            }
            for(int i = LstRobot.Count -1; i>=0;i--)
            {
                if (LstRobot[i].Position.X == -1)
                    LstRobot.RemoveAt(i);
            }
        }
        /* fonction cubes */
        private void mergePosition(List<Cub> lst)
        {
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
                            //milieu.X = (int)(milieu.X * ratioCmParPixel[0]);
                            //milieu.Y = (int)(milieu.Y * ratioCmParPixel[1]);
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
                    tab.Add(true);
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
                        po.Position.X = (int)(milieu.X * ratioCmParPixel[0]);
                        po.Position.Y = (int)(milieu.Y * ratioCmParPixel[1]);
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
            Pzt.B.X = x ;
            Pzt.B.Y = y;
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
                Thread.Sleep(15);
            }
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (paire == 0)
            {
                Image<Emgu.CV.Structure.Bgr, Byte> tmp = _capture.RetrieveBgrFrame();
                imageDebug.Image = tmp;
                afficheImage(this, new NewFrameEventArgs(tmp.ToBitmap()));
            }
                paire= (paire + 1) % 2;

        }

        private void afficheImage(object sender, NewFrameEventArgs eventArgs)
        {
            /* Affiche l'image recu par la WebCam */

            // Instancie un Thread
            if (ListeImage[lastThread] == null)
            {
                ListeImage[lastThread] = new ImgWebCam((Bitmap)eventArgs.Frame.Clone(), nbImageCapture, tailleGlyph);
                lastThread++;
                lastThread %= nbThread;
            }
            
            //imgTraitment(new ImgWebCam((Bitmap)eventArgs.Frame.Clone(), nbImageCapture, tailleGlyph));
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
            /* Homographie du terrain et detection contours */
            if (imageShow % nbImgColorTraiter == 0 && LstHslFiltering.Count > 0)
                img.homographie(LimiteTerrain, true);
            else
                img.homographie(LimiteTerrain, false);
            
            if (col !=null)
                addcouleur(img.getUnImgReel().ToManagedImage());
            
            img.ColeurVersNB();
            img.DetectionContour((short)numericUpDown1.Value);

            /* Reconnaissance des glyphs et taille du terrain si besoin */
            if (imageShow < img.getNumeroImg() && (ratioCmParPixel[0] == 1 || ratioCmParPixel[1] == 1))
            {
                double[] tmp = img.detectionGlyph(true);
                if (tmp != null)
                {
                    ratioCmParPixel = tmp;
                    int[] TailleTerain = img.getTailleTerrain(tmp[0], tmp[1]);
                    UpdateTailleTerain(TailleTerain[0], TailleTerain[1]);
                    Logger.GlobalLogger.info("Taille terrain : " + TailleTerain[0] + " x " + TailleTerain[1] + " cm");
                }
            }
            else if ( imageShow < img.getNumeroImg())
            {
                img.detectionGlyph(false);
            }
            
            /* Merge des positions et dessin sur les lignes */
            if (imageShow < img.getNumeroImg())
                {
                    mergePosition(img.getLstRobot());
                    if (imageShow % nbImgColorTraiter == 0)
                        mergePosition(img.getImageColor(LstHslFiltering));
                    
                    if (polyline !=null && polyline.Count > 0)
                    {
                        img.dessinePolyline(polyline);
                    }
                    
                    if(LstCube.Count > 0 )
                         img.dessineRectangle(getRectCube(), Color.White);
                    imageShow = img.getNumeroImg();

                    /*
                     * PointDessin p;
                     * for (int i = 0; i < LstZone.Count; i++)
                    {
                        List<PolyligneDessin> lst = new List<PolyligneDessin>();
                        PolyligneDessin poly = new PolyligneDessin(Color.Green);
                        if (LstZone[i].A.X != 0 || LstZone[i].A.Y != 0)
                        {
                            p = new PointDessin();
                            p.X = LstZone[i].A.X;
                            p.Y = LstZone[i].A.Y;
                            poly.addPoint(p);
                        }
                        if (LstZone[i].B.X != 0 || LstZone[i].B.Y != 0)
                        {
                            p = new PointDessin();
                            p.X = LstZone[i].B.X;
                            p.Y = LstZone[i].B.Y;
                            poly.addPoint(p);
                        }
                        if (LstZone[i].C.X != 0 || LstZone[i].C.Y != 0)
                        {
                            p = new PointDessin();
                            p.X = LstZone[i].C.X;
                            p.Y = LstZone[i].C.Y;
                            poly.addPoint(p);
                        }
                        if (LstZone[i].D.X != 0 || LstZone[i].D.Y != 0)
                        {
                            p = new PointDessin();
                            p.X = LstZone[i].D.X;
                            p.Y = LstZone[i].D.Y;
                            poly.addPoint(p);
                            p = new PointDessin();
                            p.X = LstZone[i].A.X;
                            p.Y = LstZone[i].A.Y;
                            poly.addPoint(p);
                        }
                        lst.Add(poly);
                        img.dessinePolyline(lst);
                    }
                    if (imageShow % 2 == 0)
                    {
                        ThreadColor = new Thread(detectionColor);
                        ThreadColor.Start(img);
                    }
                    */
                    if (imgReel != null)
                    {
                        imgReel.Invoke((affichageImg)imgAffiche, img.getUnImgReel().ToManagedImage(), imgReel);
                    }
                }
                Thread.Sleep(25);
        }
        private void detectionColor(Object s)
        {
            //TODO: REACTIVER FONCTION
            //mergePosition(((ImgWebCam)s).getImageColor(LstHslFiltering));
            
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
                int ord = (int)(((float)w_i * (float)((float)e.X / (float)w_c)));
                int abs = (int)(((float)h_i * (float)((float)e.Y / (float)h_c)));

                LimiteTerrain.Add(new IntPoint(ord,abs));
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
                col = new int[] {(int)e.X, (int)e.Y};
            }
            else if (e.Button == MouseButtons.Middle)
            {
                if(LstZone.Count > 0 && LstZone[LstZone.Count - 1].ID == -1)
                {
                    int w_i = ((PictureBox)sender).Image.Width;
                    int h_i = ((PictureBox)sender).Image.Height;
                    int w_c = ((PictureBox)sender).Width;
                    int h_c = ((PictureBox)sender).Height;
                    float ord = ((float)w_i * (float)((float)e.X / (float)w_c));
                    float abs = ((float)h_i * (float)((float)e.Y / (float)h_c));

                    PositionZone zn = LstZone[LstZone.Count - 1];
                    if (zn.A.X == 0 && zn.A.Y == 0)
                    {
                        zn.A.X = (int)ord;
                        zn.A.Y = (int)abs;
                    }
                    else if(zn.B.X == 0 && zn.B.Y == 0)
                    {
                        zn.B.X = (int)ord;
                        zn.B.Y = (int)abs;
                    }
                    else if(zn.C.X == 0 && zn.C.Y == 0)
                    {
                        zn.C.X = (int)ord;
                        zn.C.Y = (int)abs;
                    }
                    else
                    {
                        zn.D.X = (int)ord;
                        zn.D.Y = (int)abs;
                        zn.ID = LstZone.Count - 1;
                    }
                    LstZone[LstZone.Count - 1] = zn;
                    if (LstZone[LstZone.Count - 1].ID > -1)
                        envoieListe(LstZone);
                }
                
            }
        }
        public void addcouleur(Bitmap bm)
        {
            if (LstZone.Count == 0 || LstZone[LstZone.Count - 1].ID != -1 && col !=null)
            {
                int w_i = bm.Width;
                int h_i = bm.Height;
                int w_c = imgReel.Width;
                int h_c = imgReel.Height;
                float ord = ((float)w_i * (float)((float)this.col[0] / (float)w_c));
                float abs = ((float)h_i * (float)((float)this.col[1] / (float)h_c));
                this.col = null;
                int R = 0, G = 0 , B = 0, A = 0;
                int count = 0;
                // Moyenne des pixels
                for (int i = (int)(ord - 1); i < (int)(ord + 1); i++)
                {
                    for (int j = (int)(abs - 1); j < (int)(abs + 1); j++)
                    {
                        count++;
                        Color c = bm.GetPixel(i,j);
                        R += c.R;
                        G += c.G;
                        B += c.B;
                        A += c.A;
                    }
                }
                RGB tmp = new RGB(((byte)(R / count)), ((byte)(G / count)), ((byte)(B / count)), ((byte)(A / count)));
                HSL colo = HSL.FromRGB(tmp);
                Logger.GlobalLogger.debug("Couleur ajoutée : " + tmp.ToString() + " HLS : " + colo.Hue + " " + colo.Luminance + " " + colo.Saturation);

                HSLFiltering Filter = new HSLFiltering();
                Filter.Hue = new IntRange((colo.Hue + 340) % 360, (colo.Hue + 20) % 360);
                Filter.Saturation = new Range(0.6f, 1f);
                Filter.Luminance = new Range(0.1f, 1f);
                LstHslFiltering.Add(Filter);
                PositionZone ZoneTmp = new PositionZone();
                ZoneTmp.ID = -1;
                LstZone.Add(ZoneTmp);
                MessageBox.Show("Ajouter une zone de dépose avec le click milieu");
            }
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
                        PositionRobot ta = tmp;
                        ta.Position.X = -1;
                        ta.Position.Y = -1;
                        pos.Add(ta);
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
                   // Logger.GlobalLogger.debug("Suppression de " + i + " Glyphs");
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
