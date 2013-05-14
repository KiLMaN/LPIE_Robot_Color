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
        public UnmanagedImage getImage()
        {
            return imgGlyph;
        }
        public int getIdentifiant()
        {
            return Idenfitifant;
        }
        public void ReconnaissanceGlyph(List<IntPoint> corners, UnmanagedImage img)
        {
            Recognize(corners[0],img);
        }

        public void Recognize(IntPoint corners, UnmanagedImage img)
        {
            Logger.GlobalLogger.debug("Nouvelle analyse");
            Logger.GlobalLogger.debug("");
            Logger.GlobalLogger.debug("");
            List<IntPoint> LstPoint = new List<IntPoint>();
            string chaine,chainebis;
            int moyenne;
            IntPoint ip1 = new IntPoint();
            IntPoint ip2 = new IntPoint();
            IntPoint ip3 = new IntPoint();
            int marge = (imgGlyph.Width * 5) / 100;

            // Calucul de la taille des cellules
            int cellWidth = (imgGlyph.Width - marge) / glyphSize;
            int cellHeight = (imgGlyph.Height - marge) / glyphSize;
            // Définition d'une matrice contenant les valeurs de l'image
            int[,] cellIntensity = new int[glyphSize, glyphSize];


            // Découpage du glyph en zone
            for (int i = 0; i < glyphSize; i++)
            {
                ip1.X = cellWidth * i + marge;
                ip2.X = cellWidth * (i + 1) + marge;
                for (int j = 0; j < glyphSize; j++)
                {
                    ip1.Y = cellHeight * j + marge;
                    ip2.Y = cellHeight * (j + 1) + marge;

                    LstPoint.Add(ip1);
                    LstPoint.Add(ip2);

                    moyenne = 0;
                    int count = 0;

                    for (int x = (marge + cellWidth * i + 1); x < (marge + cellWidth * (i + 1) - 2); x += 2)
                    {
                        for (int y = (marge + cellHeight * j + 1); y < (marge + cellHeight * (1 + j) - 2); y += 2)
                        {
                            ip3.X = x;
                            ip3.Y = y;
                            count++;
                    
                            moyenne += imgGlyph.GetPixel(ip3).R;
                            if (i == 2 && j == 3)
                                imgGlyph.SetPixel(ip3, Color.Red);
                        }
                    }
                    cellIntensity[i, j] = moyenne;
                }
            }

            // Debug
            {
                for (int gi = 0; gi < glyphSize; gi++)
                {
                    chaine = "";
                    chainebis = "";
                    for (int gj = 0; gj < glyphSize; gj++)
                    {
                        chainebis += cellIntensity[gi, gj] + "\t";
                        cellIntensity[gi, gj] = (cellIntensity[gi, gj] > 127 ) ? 0 : 1;
                        chaine += cellIntensity[gi, gj] + "\t";
                    }
                    //Logger.GlobalLogger.debug(chainebis);
                    Logger.GlobalLogger.debug(chaine);
                }

            }
        }
    }
}
