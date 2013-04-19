using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;

using AForge.Math;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace video
{
    public class ImgWebCam
    {
        protected Bitmap imgReel;
        protected UnmanagedImage imgNB;
        protected UnmanagedImage imgContour;
        protected ulong numeroImage;

        public ImgWebCam(Bitmap image, ulong noImage)
        {
            this.imgReel = image;
           
            this.numeroImage = noImage;
        }

        private void setter(Bitmap image, ulong noImage)
        {
            this.imgReel = image;
            this.numeroImage = noImage;
        }
        public Bitmap getImageReel()
        {
            return this.imgReel;
        }
        public UnmanagedImage getImageNB()
        {
            return this.imgNB;
        }
        public UnmanagedImage getImageContour()
        {
            return this.imgContour;
        }
        public ulong getNumeroImg()
        {
            return this.numeroImage;
        }

        /* -------------------------- Traitement image -------------------------- */
        public void ColeurVersNB()
        {
            /* Convertie l'image en noir et blanc */
            UnmanagedImage image = UnmanagedImage.FromManagedImage(imgReel);

            imgNB = UnmanagedImage.Create(imgReel.Width, imgReel.Height,
                    PixelFormat.Format8bppIndexed);
            Grayscale.CommonAlgorithms.BT709.Apply(image, imgNB);
        }
        public void DetectionContour(int sueil)
        {
            /* Detecte les contours de l'image depuis l'image noir et blanc */

            // 2 - Edge detection
            DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector();
            imgContour = edgeDetector.Apply(imgNB);

            // 3 - Threshold edges
            Threshold thresholdFilterGlyph = new Threshold(sueil);

            thresholdFilterGlyph.ApplyInPlace(imgContour);
        }
        public void detectionGlyph()
        {

        }

    }
}
