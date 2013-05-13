using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Imaging;
using System.Drawing;
using AForge;
using System.IO;
using utils;
namespace video
{
    class Glyph
    {
        int glyphSize = 5;
        int Idenfitifant;
        UnmanagedImage imgGlyph;

        public Glyph(UnmanagedImage glyphImage)
        {
            imgGlyph = glyphImage;
            Idenfitifant = 0;
        }
        public void setImage(UnmanagedImage img)
        {
            imgGlyph = img;
        }
        public int getIdentifiant()
        {
            return Idenfitifant;
        }
        public void ReconnaissanceGlyph(List<IntPoint> corners, UnmanagedImage img)
        {
            Recognize(img);
        }

        public void Recognize(UnmanagedImage img)
        {
            Rectangle rect = new Rectangle(0, 0, imgGlyph.Width, imgGlyph.Height);
            // Détection des de la taille du glyph
            int glyphStartX = rect.Left;
            int glyphStartY = rect.Top;
            int glyphWidth = rect.Width;
            int glyphHeight = rect.Height;

            // Calucul de la taille des cellules
            int cellWidth = glyphWidth / glyphSize;
            int cellHeight = glyphHeight / glyphSize;

            // allow some gap for each cell, which is not scanned => Marge de taille
            int cellOffsetX = (int)(cellWidth * 0.2);
            int cellOffsetY = (int)(cellHeight * 0.2);

            // Définition d'une taille de zone de scan
            int cellScanX = (int)(cellWidth * 0.6);
            int cellScanY = (int)(cellHeight * 0.6);
            int cellScanArea = cellScanX * cellScanY;

            // Définition d'une matrice contenant les valeurs de l'image
            int[,] cellIntensity = new int[glyphSize, glyphSize];

            unsafe
            {
                int stride = img.Stride;

                byte* srcBase = (byte*)img.ImageData.ToPointer() +
                    (glyphStartY + cellOffsetY) * stride +
                    glyphStartX + cellOffsetX;
                byte* srcLine;
                byte* src;

                // for all glyph's rows
                for (int gi = 0; gi < glyphSize; gi++)
                {
                    srcLine = srcBase + cellHeight * gi * stride;

                    // for all lines in the row
                    for (int y = 0; y < cellScanY; y++)
                    {
                        // for all glyph columns
                        for (int gj = 0; gj < glyphSize; gj++)
                        {
                            src = srcLine + cellWidth * gj;

                            // for all pixels in the column
                            for (int x = 0; x < cellScanX; x++, src++)
                            {
                                cellIntensity[gi, gj] += *src;
                            }
                        }

                        srcLine += stride;
                    }
                }
            }

            byte[,] glyphValues = new byte[glyphSize, glyphSize];
            float confidence = 1f;

            for (int gi = 0; gi < glyphSize; gi++)
            {
                for (int gj = 0; gj < glyphSize; gj++)
                {
                    float fullness = (float)
                        (cellIntensity[gi, gj] / 255) / cellScanArea;
                    float conf = (float)System.Math.Abs(fullness - 0.5) + 0.5f;

                    glyphValues[gi, gj] = (byte)((fullness > 0.5f) ? 1 : 0);

                    if (conf < confidence)
                        confidence = conf;
                }
            }

            Logger.GlobalLogger.debug("" + ((cellWidth - (2 * cellOffsetX)) * 90));
            // Binarisation des résultats
            for (int i = 0; i < glyphSize; i++)
            {
                String tmp = "";
                for (int j = 0; j < glyphSize; j++)
                {
                    tmp += glyphValues[i, j] + "\t";
                    //cellIntensity[i, j] = (cellIntensity[i, j] > ((cellWidth - (2 * cellOffsetX)) * 90)) ? 1 : 0;
                   // tmp += cellIntensity[i, j]  + "\t";
                }
                Logger.GlobalLogger.debug(tmp);
            }
            
        }
    }
}
