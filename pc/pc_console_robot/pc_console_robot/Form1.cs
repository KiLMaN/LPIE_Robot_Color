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
    public partial class Form1 : Form
    {
        bool DeviceExist = false;
        FilterInfoCollection videoDevices;
        VideoCaptureDevice videoSource;

        public Form1()
        {
            InitializeComponent();
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
            }
            catch (ApplicationException)
            {
                selecteur_camera.Items.Add("No capture device on your system");
                selecteur_camera.SelectedIndex = 0; //make default to first cam
                DeviceExist = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
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
                videoSource.Start();
                
            }
            else
            {
                label_etat_programe.Text = "Error: No Device selected.";
            }

            btn_afficher_flux_cam.Enabled = false;
            btn_stopper_flux_video.Enabled = true;
        }

        //eventhandler if new frame is ready
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //Rectangle rect = new Rectangle(0,0,eventArgs.Frame.Width,eventArgs.Frame.Height);
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();


            // 1 - grayscaling
            UnmanagedImage image = UnmanagedImage.FromManagedImage(img);
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
            thresholdFilter.ApplyInPlace(edgesImage);

            // All the image processing is done here...
            pictureBox1.Image = image.ToManagedImage();
                pictureBox2.Image = grayImage.ToManagedImage();
                pictureBox3.Image = edgesImage.ToManagedImage();
        }

        private void btn_stopper_flux_video_Click(object sender, EventArgs e)
        {
            btn_afficher_flux_cam.Enabled = true;
            btn_stopper_flux_video.Enabled = false;
        }
    }
}
