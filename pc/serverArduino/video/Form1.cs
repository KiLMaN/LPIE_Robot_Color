using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using AForge.Math;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace video
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private ulong nbImageCapture = 0;
        private ulong imageShow = 0;

        private const int nbThread = 4;
        private int lastThread = 0;
        private Thread[] ListeThread = new Thread[nbThread];
        private ImgWebCam[] ListeImage = new ImgWebCam[nbThread];

        double millLastPic = 0;

        public Form1()
        {
            int i;
            InitializeComponent();
            // Initialisation des webcams
            if (ListerWebCam() == false)
            {
                button_Ok.IsAccessible = false;
                button_Ok.Enabled = false;
                MessageBox.Show("Aucune caméra détectée.");
            }

        }
        /* -------------------------- Fonction WebCam -------------------------- */

        protected Boolean ListerWebCam()
        {
            /* Retourne la liste des webcams connecté en usb */
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                ListeWebCam.Items.Add(VideoCaptureDevice.Name);
            }
            
            if(ListeWebCam.Items.Count > 0)
            {
                ListeWebCam.SelectedIndex = 0;
                chargementListeResolution(0);
                return true;
            }
            return false;
        }
        protected void chargementListeResolution(int NomCamera)
        {
            /* Charge la liste des résolutions disponibles en fonction de la WebCam selectionnée */
            VideoCaptureDevice tmpVideo = new VideoCaptureDevice(VideoCaptureDevices[NomCamera].MonikerString);
            Resolution.Items.Clear();
            foreach (VideoCapabilities Cap in tmpVideo.VideoCapabilities)
            {
                Resolution.Items.Add("" + Cap.FrameSize.Width + " * " + Cap.FrameSize.Height + " (" + Cap.FrameRate.ToString() + " FPS)");
            }
            if(Resolution.Items.Count > 0 )
                Resolution.SelectedIndex = 0;

        }
        protected Boolean openWebCam(int NomCamera)
        {
            /* Ouvre le flux vidéo et initialise le EventHandler */
            
            // Creation de la source vidéo
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[NomCamera].MonikerString);
            FinalVideo.DesiredFrameRate = FinalVideo.VideoCapabilities[Resolution.SelectedIndex].FrameRate;
            FinalVideo.DesiredFrameSize = FinalVideo.VideoCapabilities[Resolution.SelectedIndex].FrameSize;

            // Création du Eventhandler
            FinalVideo.NewFrame += new NewFrameEventHandler(afficheImage);
            FinalVideo.Start();

            if (FinalVideo.IsRunning == false)
            {
                MessageBox.Show("Erreur Ouverture camera");
                return false;
            }
            return true;
        }


        /* -------------------------- Gestions des images  -------------------------- */
        public void TraitementThread(object id)
        {
            while (true)
            {
                // Traitement et Affichage des images  
                try
                {
                    if (ListeImage[(int)id] != null)
                    {
                        this.Invoke((TraitementImg)imgTraitment, ListeImage[(int)id]);
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
            ListeImage[lastThread] = new ImgWebCam((Bitmap)eventArgs.Frame.Clone(), nbImageCapture);
            lastThread++;
            lastThread %=  nbThread;

            // Instantiation d'un objet Image
            //ImgWebCam img = new ImgWebCam((Bitmap) eventArgs.Frame.Clone(), nbImageCapture);

            // Traitement et Affichage des images   
            try
            {
                if ((nbImageCapture % 10) == 0)
                {
                    this.Invoke((UpdateFPS)affichageFPS);
                }
               // this.Invoke((TraitementImg)imgTraitment, img);
            }
            catch { }
            
            
            nbImageCapture++;
        }

        public delegate void TraitementImg(ImgWebCam img);
        public void imgTraitment(ImgWebCam img)
        {
           img.ColeurVersNB();
           img.DetectionContour((short)numericUpDown1.Value);

           try
           {
               if (imageShow < img.getNumeroImg())
               {
                   imageShow = img.getNumeroImg();
                   // this.Invoke((afficageImg)imgAffiche, img.getImageReel(), ImageReel);
                   // this.Invoke((afficageImg)imgAffiche, img.getImageNB().ToManagedImage(),ImgNb);
                   this.Invoke((afficageImg)imgAffiche, img.getImageContour().ToManagedImage(), ImgContour);
               }
             
            }
            catch { }
        }

        public delegate void afficageImg(Bitmap img, PictureBox box);
        public void imgAffiche(Bitmap img, PictureBox box)
        {
            /* Affichage de l'image dans la PictureBox*/
            box.Image = img;
        }

        /* -------------------------- Gestions des FPS  -------------------------- */
        public delegate void UpdateFPS();
        public void affichageFPS()
        {
             double delaisImage = DateTime.Now.TimeOfDay.TotalMilliseconds - millLastPic;
             millLastPic = DateTime.Now.TimeOfDay.TotalMilliseconds;

             double FPS = 1 / delaisImage * 1000 * 10 + 1;

             LblFPS.Text = FPS + " FPS";
        }


        /* -------------------------- Gestions des actions de la fenêtre  -------------------------- */
        private void ValideCamera_Click(object sender, EventArgs e)
        {
            int i;

            /* Demande le démarage du flux video de la WebCam */
            if( FinalVideo != null && FinalVideo.IsRunning)
                FinalVideo.SignalToStop();
   
            if (ListeWebCam.Items.Count != 0 && ListeWebCam.SelectedIndex >= 0)
            {
                openWebCam(ListeWebCam.SelectedIndex);    
            }

            // Initialisation des threads
            for (i = 0; i < nbThread; i++)
            {
                ListeThread[i] = new Thread(TraitementThread);
                ListeThread[i].Start(i);
            }
        }
        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (FinalVideo != null && FinalVideo.IsRunning)
            {
                FinalVideo.SignalToStop();
                for (int i = 0; i < nbThread; i++)
                {
                    ListeThread[i].Abort();
                    ListeImage[i] = null;
                }
                nbImageCapture = 0;
                imageShow = 0;

            }
        }

        private void ListeWebCam_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* Chargement des nouvelles résolutions de la caméra sélectionnée */
            chargementListeResolution(ListeWebCam.SelectedIndex);
        }

     
    }
}
