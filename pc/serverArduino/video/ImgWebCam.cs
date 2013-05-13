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
using AForge;
using AForge.Math.Geometry;

namespace video
{
    public class ImgWebCam
    {
        protected Bitmap imgReel;
        protected UnmanagedImage imgNB;
        protected UnmanagedImage imgContour;
        protected ulong numeroImage;
        const int stepSize = 3;

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
        public int detectionGlyph()
        {
            int nbElement = 0;
            UnmanagedImage tmp = UnmanagedImage.FromManagedImage(imgReel);
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker( );
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.MinHeight = 32;
            blobCounter.MinWidth = 32;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            // 4 - find all stand alone blobs
            blobCounter.ProcessImage(imgContour);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // 5 - check each blob
            for ( int i = 0, n = blobs.Length; i < n; i++ )
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners = null;

                // Test de la forme selectionnée
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    // Détection des points de coutour
                    List<IntPoint> leftEdgePoints, rightEdgePoints, topEdgePoints, bottomEdgePoints;
                    blobCounter.GetBlobsLeftAndRightEdges(blobs[i], out leftEdgePoints, out rightEdgePoints);
                    blobCounter.GetBlobsTopAndBottomEdges(blobs[i], out topEdgePoints, out bottomEdgePoints);

                    // calculate average difference between pixel values from outside of the
                    // shape and from inside
                    float diff = CalculateAverageEdgesBrightnessDifference(leftEdgePoints, rightEdgePoints, imgNB);

                    // check average difference, which tells how much outside is lighter than
                    // inside on the average
                    if (diff > 20)
                    {
                        // Transformation de l'image reçu en un carré pour la reconnaissance
                        QuadrilateralTransformation quadrilateralTransformation = new QuadrilateralTransformation(corners, 100, 100);
                        UnmanagedImage glyphImage = quadrilateralTransformation.Apply(imgNB);
                        // Filtre de contraste
                        OtsuThreshold otsuThresholdFilter = new OtsuThreshold();
                        otsuThresholdFilter.ApplyInPlace(glyphImage);

                        // Reconnaissance du Glyph
                        Glyph Gl = new Glyph(glyphImage);
                        imgContour = glyphImage;
                        Gl.ReconnaissanceGlyph(corners,imgNB);
 

                        // Si le Glyph est valide
                        if (Gl.getIdentifiant() > 0)
                        {
                            // Coloration des contours des zones détectées
                            tmp.SetPixels(leftEdgePoints, Color.Red);
                            tmp.SetPixels(rightEdgePoints, Color.Red);
                            tmp.SetPixels(topEdgePoints, Color.Red);
                            tmp.SetPixels(bottomEdgePoints, Color.Red);
                            nbElement++;
                        }
                    }
                }
            }
            imgReel = tmp.ToManagedImage();
            return nbElement;
        }
        
        
        // Calculate average brightness difference between pixels outside and
        // inside of the object bounded by specified left and right edge
        private float CalculateAverageEdgesBrightnessDifference(List<IntPoint> leftEdgePoints, List<IntPoint> rightEdgePoints, UnmanagedImage image)
        {
            // create list of points, which are a bit on the left/right from edges
            List<IntPoint> leftEdgePoints1 = new List<IntPoint>();
            List<IntPoint> leftEdgePoints2 = new List<IntPoint>();
            List<IntPoint> rightEdgePoints1 = new List<IntPoint>();
            List<IntPoint> rightEdgePoints2 = new List<IntPoint>();

            int tx1, tx2, ty;
            int widthM1 = image.Width - 1;

            for (int k = 0; k < leftEdgePoints.Count; k++)
            {
                tx1 = leftEdgePoints[k].X - stepSize;
                tx2 = leftEdgePoints[k].X + stepSize;
                ty = leftEdgePoints[k].Y;

                leftEdgePoints1.Add(new IntPoint(
                    (tx1 < 0) ? 0 : tx1, ty));
                leftEdgePoints2.Add(new IntPoint(
                    (tx2 > widthM1) ? widthM1 : tx2, ty));

                tx1 = rightEdgePoints[k].X - stepSize;
                tx2 = rightEdgePoints[k].X + stepSize;
                ty = rightEdgePoints[k].Y;

                rightEdgePoints1.Add(new IntPoint(
                    (tx1 < 0) ? 0 : tx1, ty));
                rightEdgePoints2.Add(new IntPoint(
                    (tx2 > widthM1) ? widthM1 : tx2, ty));
            }

            // collect pixel values from specified points
            byte[] leftValues1 = image.Collect8bppPixelValues(leftEdgePoints1);
            byte[] leftValues2 = image.Collect8bppPixelValues(leftEdgePoints2);
            byte[] rightValues1 = image.Collect8bppPixelValues(rightEdgePoints1);
            byte[] rightValues2 = image.Collect8bppPixelValues(rightEdgePoints2);

            // calculate average difference between pixel values from outside of
            // the shape and from inside
            float diff = 0;
            int pixelCount = 0;

            for (int k = 0; k < leftEdgePoints.Count; k++)
            {
                if (rightEdgePoints[k].X - leftEdgePoints[k].X > stepSize * 2)
                {
                    diff += (leftValues1[k] - leftValues2[k]);
                    diff += (rightValues2[k] - rightValues1[k]);
                    pixelCount += 2;
                }
            }

            return diff / pixelCount;
        }

    }
}
