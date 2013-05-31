using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge;
using AForge.Math.Geometry;
using utils.Events;
using utils;

namespace video
{
    public class ImgWebCam
    {
        protected Bitmap imgReel;
        protected UnmanagedImage UnImgReel;
        protected UnmanagedImage ImgColor;
        protected UnmanagedImage imgNB;
        protected UnmanagedImage imgContour;

        protected ulong numeroImage;
        private List<PositionRobot> LstRbt = new List<PositionRobot>();
        protected bool[] AutAffichage = new bool[] { true, true, true }; // ContourGlyph, CentreGlyph, PositionPince
        const int stepSize = 3;
        private int GlyphSize;

        public ImgWebCam(Bitmap image, ulong noImage, int Size)
        {
            this.imgReel = image;
            this.GlyphSize = Size;
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
        public UnmanagedImage getUnImgReel()
        {
            return UnImgReel;
        }
        public UnmanagedImage getImageNB()
        {
            return this.imgNB;
        }
        public UnmanagedImage getImageContour()
        {
            return this.imgContour;
        }
        public UnmanagedImage getImageColor(int minHue, int maxHue, int minSat, int maxSat, int minLum, int maxLim)
        {
            //Create color filter
            HSLFiltering HslFilter = new HSLFiltering();
            //configre the filter
            HslFilter.Hue = new IntRange(340, 20);
            HslFilter.Saturation = new Range(0.5f,1.0f);
           // HslFilter.Luminance = new Range(minLum, maxLim);
            HslFilter.ApplyInPlace(ImgColor);

            return ImgColor;
            //apply color filter to the image
            ColorFiltering colorFilter = new ColorFiltering();
            colorFilter.Red = new IntRange(80, 255);
            colorFilter.Green = new IntRange(0, 60);
            colorFilter.Blue = new IntRange(0, 60);
            colorFilter.ApplyInPlace(ImgColor);

            
            // create blob counter and configure it
            BlobCounter blobCounter1 = new BlobCounter();
            blobCounter1.MinWidth = 25;                    // set minimum size of
            blobCounter1.MinHeight = 25;                   // objects we look for
            blobCounter1.FilterBlobs = true;               // filter blobs by size
            blobCounter1.ObjectsOrder = ObjectsOrder.Size; // order found object by size
            // grayscaling
            GrayscaleBT709 grayscaleFilter = new GrayscaleBT709();
            // apply it to color filtered image
            ImgColor = grayscaleFilter.Apply(ImgColor);
            // locate blobs 
            blobCounter1.ProcessImage(ImgColor);
            Rectangle[] rects = blobCounter1.GetObjectsRectangles();     
                // draw rectangle around the biggest blob
            foreach( Rectangle objectRect  in rects)
            {
                Drawing.Rectangle(UnImgReel, objectRect, Color.Turquoise);
            }

            return ImgColor;
        }
        public List<PositionRobot> getLstRobot()
        {
            return LstRbt;
        }
        public ulong getNumeroImg()
        {
            return this.numeroImage;
        }

        #region ##### Dessin #####
        public void dessinePoint(IntPoint point, UnmanagedImage img,int nbPixel,Color col)
        {
            for (int i = point.X - nbPixel / 2; i < point.X + nbPixel / 2 + 1; i++)
            {
                for (int j = point.Y - nbPixel / 2; j < point.Y + nbPixel / 2 + 1; j++)
                {
                    img.SetPixel(i, j, col);
                }
            }
        }
        public void dessinePolyline(List<PolyligneDessin> Polyline)
        {
            
        }
        #endregion

        #region ##### Traitement image #####
        public void homographie(List<IntPoint> LimiteTerain)
        {
            UnImgReel = UnmanagedImage.FromManagedImage(imgReel);
            
            /* Remplacement de l'image par le terain détecte dedans */
            if (LimiteTerain.Count == 4)
            {
                QuadrilateralTransformation quadrilateralTransformation = new QuadrilateralTransformation(LimiteTerain, UnImgReel.Width, UnImgReel.Height);
                UnImgReel = quadrilateralTransformation.Apply(UnImgReel);
            }
            ImgColor = UnImgReel.Clone();
        }
        public void ColeurVersNB()
        {
            /* Convertie l'image en noir et blanc */

            UnmanagedImage image = UnmanagedImage.Create(UnImgReel.Width, UnImgReel.Height,
                    PixelFormat.Format8bppIndexed);
            Grayscale.CommonAlgorithms.BT709.Apply(UnImgReel, image);

            imgNB = image;
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
        #endregion

        #region ##### Traitement Glyph #####
        public double[] detectionGlyph(bool CalculTailleTerrain)
        {
            bool Trouve = false;
            double[] ratio = new double[2] { 0, 0 };
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.MinHeight = 32;
            blobCounter.MinWidth = 32;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            // 4 - find all stand alone blobs
            blobCounter.ProcessImage(imgContour);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // 5 - check each blob
            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);
                List<IntPoint> corners = null;

                // Test de la forme selectionnée
                if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                {
                    // Détection des points de coutour
                    List<IntPoint> leftEdgePoints, rightEdgePoints, topEdgePoints, bottomEdgePoints;
                    
                    Line Horizontale = Line.FromPoints(new IntPoint(0,0),new IntPoint(10,0));
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
                        QuadrilateralTransformation quadrilateralTransformation = new QuadrilateralTransformation(corners, 50, 50);
                        UnmanagedImage glyphImage = quadrilateralTransformation.Apply(imgNB);
                        // Filtre de contraste
                        OtsuThreshold otsuThresholdFilter = new OtsuThreshold();
                        otsuThresholdFilter.ApplyInPlace(glyphImage);

                        // Reconnaissance du Glyph
                        Glyph Gl = new Glyph(glyphImage, GlyphSize);

                        Gl.ReconnaissanceGlyph(corners, imgNB);

                        // Si le Glyph est valide
                        if (Gl.getIdentifiant() > 0)
                        { 
                            if (AutAffichage[0])
                            {
                                // Coloration des contours des zones détectées
                                UnImgReel.SetPixels(leftEdgePoints, Color.Red);
                                UnImgReel.SetPixels(rightEdgePoints, Color.Red);
                                UnImgReel.SetPixels(topEdgePoints, Color.Red);
                                UnImgReel.SetPixels(bottomEdgePoints, Color.Red);
                            }

                            // Détection du milieu
                            Line line = Line.FromPoints(corners[0], corners[2]);
                            Line line2 = Line.FromPoints(corners[1], corners[3]);
                            IntPoint intersection = (IntPoint)line.GetIntersectionWith(line2);
                            if (AutAffichage[1])
                            {
                                dessinePoint(intersection, UnImgReel, 4, Color.Yellow);
                            }

                            // Calcul de la rotation
                            Line ComparasionAngle = Line.FromPoints(corners[0], corners[1]);
                            Double rotation = (int) ComparasionAngle.GetAngleBetweenLines(Horizontale);
                            rotation += 90 * Gl.getNbRotation();
                            Gl.rotation = rotation - 180;
                            rotation *= (Math.PI / 180.0);


                            // Calcul d'un point en bout de pince
                            float[] Taille = new float[4];
                            // TODO: Ratio proportionnel en fonction de l'inclinaison
                            Taille[0] = corners[0].DistanceTo(corners[1]);
                            Taille[1] = corners[1].DistanceTo(corners[2]);
                            Taille[2] = corners[2].DistanceTo(corners[3]);
                            Taille[3] = corners[3].DistanceTo(corners[0]);

                            float taille = (Taille[0] / BibliotequeGlyph.Biblioteque[Gl.getPosition()].taille) * BibliotequeGlyph.Biblioteque[Gl.getPosition()].DistancePince;
                            int x = -(int)(System.Math.Sin(rotation) * taille);
                            int y = -(int)(System.Math.Cos(rotation) * taille);
                            x += (int)intersection.X;
                            y += (int)intersection.Y;
                            Gl.Position = new int[2]{x,y};
                            if (AutAffichage[2])
                            {
                                dessinePoint(new IntPoint(x, y), UnImgReel, 4, Color.Cyan);
                            }
                            imgContour = Gl.getImage();
                            addGlyph(Gl);

                            if (CalculTailleTerrain == true && Trouve == false)
                            {
                                Trouve = true;
                                int tailleglyph = BibliotequeGlyph.Biblioteque[Gl.getPosition()].taille;

                                // Detection ratio axe verticale
                                IntPoint top = (corners[0].Y < corners[1].Y ) ? corners[0] : corners[1];
                                Line l = Line.FromPoints(top, new IntPoint(top.X,UnImgReel.Height));
                                LineSegment t = new LineSegment(corners[2],corners[3]);

                                AForge.Point? pointTmp = l.GetIntersectionWith(t);
                                if (pointTmp == null)
                                {
                                    t = new LineSegment(corners[3], corners[0]);
                                    pointTmp = l.GetIntersectionWith(t);
                                    if (pointTmp == null)
                                        Trouve = false;
                                    else
                                    {
                                        ratio[0] = corners[0].DistanceTo(new IntPoint((int)(pointTmp.Value.X), (int)(pointTmp.Value.Y)));
                                        ratio[0] = (ratio[0] / Taille[3]) * tailleglyph + (tailleglyph * tailleglyph);
                                        ratio[0] = top.DistanceTo((IntPoint)pointTmp) / ratio[0];
                                    }
                                }
                                else
                                {
                                    ratio[0] = corners[2].DistanceTo(new IntPoint((int)(pointTmp.Value.X), (int)(pointTmp.Value.Y)));
                                    ratio[0] = (ratio[0] / Taille[2]) * tailleglyph + (tailleglyph * tailleglyph);
                                    ratio[0] = top.DistanceTo((IntPoint)pointTmp) / ratio[0];
                                }
                                /*if (pointTmp != null)
                                {
                                    dessinePoint((IntPoint)pointTmp, UnImgReel, 4, Color.LightSalmon);
                                    Drawing.Line(UnImgReel, top, new IntPoint(top.X, UnImgReel.Height), Color.Lavender);
                                    dessinePoint(top, UnImgReel, 5, Color.GreenYellow);
                                }
                                 */

                                // Detection ration axe horizontal
                                if (Trouve == true)
                                {
                                    l = Line.FromPoints(corners[0], new IntPoint(UnImgReel.Width, corners[0].Y));
                                    t = new LineSegment(corners[1], corners[2]);
                                    pointTmp = (IntPoint?)l.GetIntersectionWith(t);
                                    if (pointTmp == null)
                                    {
                                        t = new LineSegment(corners[2], corners[3]);
                                        pointTmp = (IntPoint?)l.GetIntersectionWith(t);
                                        if (pointTmp == null)
                                            Trouve = false;
                                        else
                                        {
                                            ratio[1] = corners[3].DistanceTo(new IntPoint((int)(pointTmp.Value.X), (int)(pointTmp.Value.Y)));
                                            ratio[1] = (ratio[1] / Taille[2]) * tailleglyph + (tailleglyph * tailleglyph);
                                            ratio[1] = top.DistanceTo((IntPoint)pointTmp) / ratio[1];
                                        }
                                    }
                                    else
                                    {
                                        ratio[1] = corners[1].DistanceTo(new IntPoint((int)(pointTmp.Value.X), (int)(pointTmp.Value.Y)));
                                        ratio[1] = (ratio[1] / Taille[1]) * tailleglyph + (tailleglyph * tailleglyph);
                                        ratio[1] = top.DistanceTo((IntPoint)pointTmp) / ratio[1];
                                    }
                                   /* if (Trouve == true)
                                    {
                                        if (pointTmp != null)
                                            dessinePoint((IntPoint)pointTmp, UnImgReel, 4, Color.LightSalmon);
                                        Drawing.Line(UnImgReel, corners[0], new IntPoint(UnImgReel.Width, corners[0].Y), Color.Lavender);
                                        dessinePoint(corners[0], UnImgReel, 5, Color.GreenYellow);
                                        
                                    }  
                                   */  
                                }
                            }
                        }
                    }
                }
            }
            return (Trouve == false) ? null : ratio;
        }
        #endregion

        #region #### Gestion info #####
        private void addGlyph(Glyph g)
        {
            PositionRobot p = new PositionRobot();
            p.Angle = (float) g.rotation;
            p.Position = new PositionElement();
            p.Position.X = g.Position[0];
            p.Position.Y = g.Position[1];
            p.Identifiant = g.getIdentifiant();

            LstRbt.Add(p);
        }
        public int[] getTailleTerrain(double ratioX, double ratioY)
        {
            return new int[2] {(int) (UnImgReel.Width * ratioX) , (int) (UnImgReel.Height* ratioY) };
        }
        #endregion

        private float CalculateAverageEdgesBrightnessDifference(List<IntPoint> leftEdgePoints, List<IntPoint> rightEdgePoints, UnmanagedImage image)
        {
            // Calculate average brightness difference between pixels outside and
            // inside of the object bounded by specified left and right edge
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
