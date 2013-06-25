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
        public List<video.VideoProg.Cub> getImageColor(List<HSLFiltering> lst)
        {
            UnmanagedImage tmpCol = null;
            List<video.VideoProg.Cub> tmp = new List<video.VideoProg.Cub>();
            for(int i = 0; i < lst.Count;i++)
            {
                HSLFiltering Filter = lst[i];
                tmpCol = ImgColor.Clone();
                Filter.ApplyInPlace(tmpCol);

                // create blob counter and configure it
                BlobCounter blobCounter1 = new BlobCounter();
                blobCounter1.MinWidth = 25;                    // set minimum size of
                blobCounter1.MinHeight = 25;                   // objects we look for
                blobCounter1.FilterBlobs = true;               // filter blobs by size
                blobCounter1.ObjectsOrder = ObjectsOrder.Size; // order found object by size

               
                blobCounter1.ProcessImage(tmpCol);
                Rectangle[] rects = DeleteRectInterne( blobCounter1.GetObjectsRectangles());
                
                // draw rectangle around the biggest blob
                for (int j = 0; j < rects.Length; j++)
                {
                    if (rects[j] == null)
                        break;
                    tmp.Add(new video.VideoProg.Cub(rects[j],i));
                }
            }
            return tmp;
        }
        public List<PositionRobot> getLstRobot()
        {
            return LstRbt;
        }
        public ulong getNumeroImg()
        {
            return this.numeroImage;
        }
        public int max(int a, int b, int c, int d)
        {
            int max = (a > b) ? a : b;
            max = (max > c) ? max : c;
            max = (max > d) ? max : d;
            return max;
        }
        public int min(int a, int b, int c, int d)
        {
            int min = (a < b) ? a : b;
            min = (min < c) ? min : c;
            min = (min < d) ? min : d;
            return min;
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
            foreach (PolyligneDessin p in Polyline)
            {
                for (int i = 0; i < p.ListePoint.Count;i++ )
                {
                    Drawing.Line(UnImgReel,new IntPoint(p.ListePoint[i].X,p.ListePoint[i].Y),new IntPoint(p.ListePoint[(i+1)%p.ListePoint.Count].X,p.ListePoint[(i+1)%p.ListePoint.Count].Y),Color.Green);
                }
            }
        }
        public void dessineRectangle(List<Rectangle> lstRec,Color col)
        {
            for (int i = 0; i < lstRec.Count; i++)
            {
                Drawing.Rectangle(UnImgReel, lstRec[i], col);
            }
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
                                
                                
                                // Pythagore pour detection taille
                                Rectangle a = blobs[i].Rectangle;
                                double angle = - Gl.rotation + 180;
                                List<IntPoint> coins = new List<IntPoint>();
                                coins.Add(new IntPoint(100,100));
                                coins.Add(new IntPoint(100, 100 + tailleglyph));
                                coins.Add(new IntPoint(100 + tailleglyph , 100 + tailleglyph));
                                coins.Add(new IntPoint(100 + tailleglyph, 100));
                                IntPoint Centre = new IntPoint((coins[2].X + coins[0].X)/2, (coins[2].Y + coins[0].Y) / 2);
                                int radius = (int)(0.5 * Math.Sqrt(coins[0].DistanceTo(coins[1]) * coins[0].DistanceTo(coins[1]) + coins[1].DistanceTo(coins[2]) * coins[1].DistanceTo(coins[2])));
                                double alpha = Math.Atan2(coins[0].DistanceTo(coins[1]), coins[1].DistanceTo(coins[2])) * (180 / Math.PI);

                                double ang = 0;
                                for(i = 0; i < 4; i++)
                                {
                                    IntPoint tmp = coins[i];
                                    switch (i)
                                    {
                                        case 0:
                                            ang = alpha - 180 + angle;
                                            break;
                                        case 1:
                                            ang = + angle - alpha;
                                            break;
                                        case 2:
                                            ang = + angle + alpha;
                                            break;
                                        case 3:
                                            ang = - alpha + 180 + angle;
                                            break;
                                    }
                                    ang *= (Math.PI / 180);
                                    tmp.X = (int)(Centre.X + radius * Math.Cos(ang));
                                    tmp.Y = (int)(Centre.Y + radius * Math.Sin(ang));
                                    
                                    coins[i] = tmp;
                                }
                                dessinePoint(coins[0], UnImgReel, 10, Color.OrangeRed);
                                dessinePoint(coins[1], UnImgReel, 10, Color.Yellow);
                                dessinePoint(coins[2], UnImgReel, 10, Color.Green);
                                dessinePoint(coins[3], UnImgReel, 10, Color.Blue);
                                
                                Rectangle r = new Rectangle(min(coins[0].X, coins[1].X, coins[2].X, coins[3].X), min(coins[0].Y, coins[1].Y, coins[2].Y, coins[3].Y),
                                                            max(coins[0].X, coins[1].X, coins[2].X, coins[3].X) - min(coins[0].X, coins[1].X, coins[2].X, coins[3].X),
                                                            max(coins[0].Y, coins[1].Y, coins[2].Y, coins[3].Y) - min(coins[0].Y, coins[1].Y, coins[2].Y, coins[3].Y));
                                Drawing.Rectangle(UnImgReel, r, Color.Yellow);
                                ratio[0] = ((double)r.Width / (double)a.Width) * 1.48;
                                ratio[1] = ((double)r.Height / (double)a.Height) * 1.48;

                               
                            }
                        }
                    }
                }
            }
            if (Trouve == false || ratio[0] == 0 || ratio[0] == 1 || ratio[1] == 0 || ratio[1] == 1)
            {
                return null;
            }
            ratio[0] *= 0.7;
            ratio[1] *= 0.7;
            return ratio;
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
            return new int[2] { (int)(UnImgReel.Width * ratioX), (int)(UnImgReel.Height * ratioY)};
        }
        #endregion

        private Rectangle[] DeleteRectInterne(Rectangle[] ls)
        {
            Rectangle[] lstRect = new Rectangle[ls.Length];
            int count = 0;
            for (int i = 0; i < lstRect.Length; i++)
            {
                Boolean trouve = false;
                for (int j = 0; j < i; j++)
                {

                    if (ls[i].IntersectsWith(ls[j]) && (ls[i].X + ls[i].Width / 2) > ls[j].X && (ls[i].X + ls[i].Width / 2) < ls[j].Right && (ls[i].Y + ls[i].Height / 2) > ls[j].Y && (ls[i].Y + ls[i].Height / 2) < ls[j].Bottom)
                    {
                       
                        trouve = true;
                        break;
                    }
                }
                if (!trouve)
                {
                    lstRect[count++] = ls[i];
                }
            }
            return lstRect;
        }
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
