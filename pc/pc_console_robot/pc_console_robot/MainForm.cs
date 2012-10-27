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
            this.FormClosing += MainForm_close;
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

        void MainForm_close(object e, FormClosingEventArgs arg )
        {
            if (videoSource != null)
            {
                if (videoSource.IsRunning)
                {
                    videoSource.Stop(); // Arrète le flux video avant la fermeture du programme 
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

            if (DeviceExist)
            {
                videoSource = new VideoCaptureDevice(videoDevices[selecteur_camera.SelectedIndex].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                //CloseVideoSource();
                //videoSource.DesiredFrameRate = 10;
                videoSource.DesiredFrameRate = videoSource.VideoCapabilities[selecteur_resolution.SelectedIndex].FrameRate;
                videoSource.DesiredFrameSize = videoSource.VideoCapabilities[selecteur_resolution.SelectedIndex].FrameSize;
                videoSource.Start();
               
            }
            else
            {
                label_etat_programe.Text = "Erreur, aucune caméra sélectionnée !";
            }

            btn_afficher_flux_cam.Enabled = false;
            btn_stopper_flux_video.Enabled = true;
        }


        int counterImg = 0;
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
                Invoke((ProcessNewFPS)UpdateNewFPS, FPS);

                //txt_resolution.Text = "" + videoSource.DesiredFrameSize.Height + " * " + videoSource.DesiredFrameSize.Width;
                string resolutionTxt = "" + img.Width + " * "  +  img.Height ;
                Invoke((ProcessNewResolution)UpdateNewResolution, resolutionTxt);
                counterImg = 0;
            }
            counterImg++;
            
            

            //Rectangle rect = new Rectangle(0,0,eventArgs.Frame.Width,eventArgs.Frame.Height);
         


            // 1 - grayscaling
            UnmanagedImage image = UnmanagedImage.FromManagedImage(img);
            UnmanagedImage imageRouge = image.Clone();
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
            Threshold thresholdFilter = new Threshold((int)numericUpDown1.Value);
            thresholdFilter.ApplyInPlace(edgesImage);

                 // 3 - Threshold edges
            thresholdFilter.ApplyInPlace(edgesImage);

           

            // create filter
            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            // set center colol and radius
            RGB color = new RGB(215, 30, 30);
            filter.CenterColor = color;
            filter.Radius = 100;
            // apply the filter
            filter.ApplyInPlace(imageRouge);

            // All the image processing is done here...
            pictureBox1.Image = image.ToManagedImage();
            pictureBox2.Image = grayImage.ToManagedImage();
            pictureBox3.Image = edgesImage.ToManagedImage();
            pictureBox4.Image = imageRouge.ToManagedImage();

        }

        private void btn_stopper_flux_video_Click(object sender, EventArgs e)
        {
           


            if (videoSource.IsRunning)
            {
                videoSource.Stop();
                btn_afficher_flux_cam.Enabled = true;
                btn_stopper_flux_video.Enabled = false;
            }
        }

    }
}
