using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;


namespace pc_console_robot
{
    public partial class MainForm : Form
    {
        bool DeviceExist = false;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;

        double _mill_last_pic = 0;

        public MainForm()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_close);
        }

        public delegate void ProcessNewImage(UnmanagedImage image, PictureBox dest);
        public void DisplayNewImage(UnmanagedImage image, PictureBox dest)
        {
            dest.Image = image.ToManagedImage();
            //txt_nb_fps.Text = ((int)FPS).ToString();
        }

        public delegate void ProcessLalbelText(string txt, Label dest);
        public void ChangeLabelText(string txt, Label dest)
        {
            dest.Text = txt;
            //txt_nb_fps.Text = ((int)FPS).ToString();
        }

        public delegate void ProcessNewFPS(double FPS);
        public void UpdateNewFPS(double FPS)
        {
            txt_nb_fps.Text = ((int)FPS).ToString();
        }
        public delegate void ProcessNewResolution(string size);
        public void UpdateNewResolution(string size)
        {
            txt_resolution.Text = size;
        }

        void MainForm_close(object e, FormClosingEventArgs arg)
        {

           

            if (videoSource != null)
            {
                if (videoSource.IsRunning)
                {
                     videoSource.SignalToStop(); // Arrète le flux video avant la fermeture du programme 
                }
            }
        }

        private void getWebCamListe()
        {
            try
            {
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                selecteur_camera.Items.Clear();
                if (videoDevices.Count == 0)
                    throw new ApplicationException();

                foreach (FilterInfo device in videoDevices)
                {
                    selecteur_camera.Items.Add(device.Name);
                }
                selecteur_camera.SelectedIndex = 0; //make default to first cam
                DeviceExist = true;
                comboBox1_SelectedIndexChanged(null, null); // Force le rafraichisement de la resolution
            }
            catch (ApplicationException)
            {
                selecteur_camera.Items.Add("Aucune caméra détectée dans votre système !");
                selecteur_camera.SelectedIndex = 0; //make default to first cam
                DeviceExist = false;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            getWebCamListe();
            /*// 1 - grayscaling
            UnmanagedImage grayImage = null;

            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                grayImage = image;
            }
            else
            {
                grayImage = UnmanagedImage.Create(image.Width, image.Height,
                    PixelFormat.Format8bppIndexed);
                Grayscale.CommonAlgorithms.BT709.Apply(image, grayImage);
            }

            // 2 - Edge detection
            DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector();
            UnmanagedImage edgesImage = edgeDetector.Apply(grayImage);

            // 3 - Threshold edges
            Threshold thresholdFilter = new Threshold(40);
            thresholdFilter.ApplyInPlace(edgesImage);*/
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DeviceExist)
            {
                VideoCaptureDevice tmpVideo = new VideoCaptureDevice(videoDevices[selecteur_camera.SelectedIndex].MonikerString);
                selecteur_resolution.Items.Clear();
                foreach (VideoCapabilities Cap in tmpVideo.VideoCapabilities)
                {
                    selecteur_resolution.Items.Add("" + Cap.FrameSize.Width + " * " + Cap.FrameSize.Height + " (" + Cap.FrameRate.ToString() + " FPS)");
                }
            }
            else
            {
                selecteur_resolution.Items.Clear();
                selecteur_resolution.Items.Add("Selectionner une caméra !");
            }
            selecteur_resolution.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getWebCamListe();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btn_afficher_flux_cam_Click(object sender, EventArgs e)
        {

            if (DeviceExist && videoSource == null)
            {
                videoSource = new VideoCaptureDevice(videoDevices[selecteur_camera.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                //CloseVideoSource();
                //videoSource.DesiredFrameRate = 10;
                videoSource.DesiredFrameRate = videoSource.VideoCapabilities[selecteur_resolution.SelectedIndex].FrameRate;
                videoSource.DesiredFrameSize = videoSource.VideoCapabilities[selecteur_resolution.SelectedIndex].FrameSize;
                videoSource.Start();
                


            }
            else if (videoSource != null)
            {
                label_etat_programe.Text = "Erreur flux déjà en route !";
            }
            else
            {
                label_etat_programe.Text = "Erreur, aucune caméra sélectionnée !";
            }

            btn_afficher_flux_cam.Enabled = false;
            btn_stopper_flux_video.Enabled = true;
        }

        private void btn_stopper_flux_video_Click(object sender, EventArgs e)
        {
            if (videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
                btn_afficher_flux_cam.Enabled = true;
                btn_stopper_flux_video.Enabled = false;
            }
        }


        int counterImg = 0;
        int posX = 0, posY = 0;
        //eventhandler if new frame is ready
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            if (counterImg == 10)
            {

                double delaisImage = DateTime.Now.TimeOfDay.TotalMilliseconds - _mill_last_pic;
                _mill_last_pic = DateTime.Now.TimeOfDay.TotalMilliseconds;

                double FPS = 1 / delaisImage * 1000 * counterImg + 1;
                // txt_nb_fps.Text = FPS.ToString() ;
               

                //txt_resolution.Text = "" + videoSource.DesiredFrameSize.Height + " * " + videoSource.DesiredFrameSize.Width;
                string resolutionTxt = "" + img.Width + " * " + img.Height;
                if (this != null && (!this.IsDisposed))
                {
                    try
                    {
                        this.Invoke((ProcessNewFPS)UpdateNewFPS, FPS);
                        this.Invoke((ProcessNewResolution)UpdateNewResolution, resolutionTxt);
                    }
                    catch (ObjectDisposedException) // La fenetre était en train de se fermée 
                    {
                    }
                }
                counterImg = 0;
            }
            counterImg++;

            //Rectangle rect = new Rectangle(0,0,eventArgs.Frame.Width,eventArgs.Frame.Height);



            // 1 - grayscaling
           /* UnmanagedImage image = UnmanagedImage.FromManagedImage(img);
            UnmanagedImage imageRouge = image.Clone();
            UnmanagedImage imageBleu = image.Clone();
            UnmanagedImage imageVert = image.Clone();
            UnmanagedImage grayImage = null;
            
            Color colorPoint = image.GetPixel(posX, posY);

            this.Invoke((ProcessLalbelText)ChangeLabelText, new object[] { colorPoint.GetHue().ToString(), lbl_hue });
            this.Invoke((ProcessLalbelText)ChangeLabelText, new object[] { colorPoint.GetBrightness().ToString(), lbl_lum });
            this.Invoke((ProcessLalbelText)ChangeLabelText, new object[] { colorPoint.GetSaturation().ToString(), lbl_sat });

            if (image.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                grayImage = image;
            }
            else
            {
                grayImage = UnmanagedImage.Create(image.Width, image.Height,
                    PixelFormat.Format8bppIndexed);
                Grayscale.CommonAlgorithms.BT709.Apply(image, grayImage);
            }

            // 2 - Edge detection
            DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector();
            UnmanagedImage edgesImage = edgeDetector.Apply(grayImage);

            // 3 - Threshold edges
            Threshold thresholdFilterGlyph = new Threshold((short)numericUpDown3.Value);
            Threshold thresholdFilterCouleur = new Threshold((short)numericUpDown2.Value);

            thresholdFilterGlyph.ApplyInPlace(edgesImage);

            /*
             * 
             * Bitmap image = (Bitmap)eventArgs.Frame.Clone();

            //Reference : http://www.aforgenet.com/framework/docs/html/743311a9-6c27-972d-39d2-ddc383dd1dd4.htm
            
             *  private HSLFiltering filter = new HSLFiltering();
            // set color ranges to keep red-orange
            filter.Hue = new IntRange(0, 20);
            filter.Saturation = new DoubleRange(0.5, 1);
            
            // apply the filter
            filter.ApplyInPlace(image);
             * */
            /*RGB colorRed = new RGB(215, 30, 30);
            RGB colorBlue = new RGB(10, 10, 215);
            RGB colorVert = new RGB(30, 215, 30);
            RGB colorBlanc = new RGB(225, 219, 160);*/

            //HSLFiltering filter = new HSLFiltering();
            // create filter
           // EuclideanColorFiltering filter = new EuclideanColorFiltering();
            //filter.Radius = (short)numericUpDown1.Value;
            //filter.Hue = new IntRange(40, 140);
            //filter.Saturation = new Range(0.5f, 1.0f);
            //filter.Luminance = new Range(0.2f, 1.0f);
           
            //filter.CenterColor = colorRed;
            //filter.ApplyInPlace(imageRouge);

            //filter.Hue = new IntRange(100, 180);
            //filter.CenterColor = colorBlanc;
            //filter.ApplyInPlace(imageVert);

            //filter.Hue = new IntRange(0, 40);
           //  //filter.CenterColor = colorBlue;
           // filter.ApplyInPlace(imageBleu);



           /* Grayscale filterRouge = new Grayscale(0.800, 0.200, 0.200);
            Grayscale filterVert = new Grayscale(0.200, 0.800, 0.200);
            Grayscale filterBleu = new Grayscale(0.200, 0.200, 0.800);

            UnmanagedImage grayRougeImage = filterRouge.Apply(imageRouge);
            UnmanagedImage grayBleuImage = filterBleu.Apply(imageBleu);


            UnmanagedImage edgesRougeImage = edgeDetector.Apply(grayRougeImage);
            UnmanagedImage edgesBleuImage = edgeDetector.Apply(grayBleuImage);

            thresholdFilterCouleur.ApplyInPlace(edgesRougeImage);
            thresholdFilterCouleur.ApplyInPlace(edgesBleuImage);
            // All the image processing is done here...
            */
            // pictureBox1.Image = image.ToManagedImage();
            if (this != null && (!this.IsDisposed)) // Si on est pas en train de suppirmer la fenetre 
            {
                try
                {
                    UnmanagedImage image = UnmanagedImage.FromManagedImage(img);
                    this.Invoke((ProcessNewImage)DisplayNewImage, new object[] { image, pic_ImageNormal });
                    image.Dispose();
/*this.Invoke((ProcessNewImage)DisplayNewImage, new object[] { edgesImage,    pic_ImageEdge });

                    this.Invoke((ProcessNewImage)DisplayNewImage, new object[] { imageRouge, pic_ImageRouge });

                    this.Invoke((ProcessNewImage)DisplayNewImage, new object[] { imageBleu, pic_ImageBleu });
                    this.Invoke((ProcessNewImage)DisplayNewImage, new object[] { imageVert, pic_ImageVert });*/
                }
                catch (ObjectDisposedException) // La fenetre était en train de se fermée 
                {
                }
            }
            /*pictureBox2.Image = grayImage.ToManagedImage();
            pictureBox3.Image = edgesImage.ToManagedImage();
            pictureBox4.Image = imageRouge.ToManagedImage();*/

        }

        private void pic_ImageNormal_Click(object sender, MouseEventArgs e)
        {
           // System.Drawing.Point mouseDownLocation = ((PictureBox)sender).PointToClient(new System.Drawing.Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y));
            posX = e.X;
            posY = e.Y;
        }



    }
}
