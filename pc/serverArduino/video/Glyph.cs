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
        private int glyphSize;
        private int Idenfitifant;
        private int nbRotation = 0;
        private int positionBibliotheque = -1;
        public int[] Position = new int[2];
        public double rotation = 0;
        UnmanagedImage imgGlyph;
        
        public Glyph(UnmanagedImage glyphImage, int Size )
        {
            imgGlyph = glyphImage;
            glyphSize = Size;
            Idenfitifant = 0;
        }
        public int getPosition()
        {
            return positionBibliotheque;
        }
        public int getNbRotation()
        {
            return nbRotation%4;
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
            lectureGlyph(Recognize(corners[0], img));
        }

        public Boolean[,] Recognize(IntPoint corners, UnmanagedImage img)
        {
            /* Retourne une matrice représentant le glyph*/

            int moyenne;
            IntPoint ip = new IntPoint();
            int marge = (imgGlyph.Width * 5) / 100;

            // Calucul de la taille des cellules
            int cellWidth = (imgGlyph.Width - marge) / glyphSize;
            int cellHeight = (imgGlyph.Height - marge) / glyphSize;
            // Définition d'une matrice contenant les valeurs de l'image
            Boolean[,] cellIntensity = new Boolean[glyphSize, glyphSize];


            // Découpage du glyph en zone
            for (int i = 0; i < glyphSize; i++)
            {
                for (int j = 0; j < glyphSize; j++)
                {
                    moyenne = 0;
                    int count = 0;

                    for (int x = (marge + cellWidth * i + 1); x < (marge + cellWidth * (i + 1) - 2); x += 2)
                    {
                        for (int y = (marge + cellHeight * j + 1); y < (marge + cellHeight * (1 + j) - 2); y += 2)
                        {
                            ip.X = x;
                            ip.Y = y;
                            count++;
                    
                            moyenne += imgGlyph.GetPixel(ip).R;
                        }
                    }
                    cellIntensity[i, j] = ( moyenne/count > 127 ) ? false : true;
                }
            }

            // Debug
            /*{
                string chaine;
                Logger.GlobalLogger.debug("");
                Logger.GlobalLogger.debug("Nouvelle analyse");
                Logger.GlobalLogger.debug("");
                for (int gi = 0; gi < glyphSize; gi++)
                {
                    chaine = "";
                    for (int gj = 0; gj < glyphSize; gj++)
                    {
                        chaine += cellIntensity[gi, gj] + "\t";
                    }
                    Logger.GlobalLogger.debug(chaine);
                }

            }
            */
            return cellIntensity;
        }

        public Boolean[,] rotationMatrice(Boolean[,] matrice)
        {
            Boolean[,] matriceTmp = new Boolean[glyphSize, glyphSize];
            this.nbRotation++;
            for (int i = 0; i < glyphSize; i++)
            {
                for (int j = 0; j < glyphSize; j++)
                {
                    matriceTmp[glyphSize - j - 1, i] = matrice[i, j];
                }
            }
            return matriceTmp;
        }
        public int lectureGlyph(Boolean[,] matrice)
        {
            /* Vérifie la validitée du glyph et lecture */
            
            int i;
            // Verification du contour exterieure
            for (i = 0; i < glyphSize; i++)
            {
                if (matrice[i, 0] == false || matrice[i, (glyphSize - 1)] == false || matrice[0, i] == false || matrice[(glyphSize - 1), i] == false)
                    return 0;
            }


            Boolean Erreur = false;
            // Vérification si le glyph est présent dans la bibliothéque
            for (i = 0; i < 4; i++)
            {
                for (int j = 0; j < BibliotequeGlyph.Biblioteque.Count; j++)
                {
                    Erreur = false;
                    Boolean[,] me = BibliotequeGlyph.Biblioteque[j].matrice;
                    for (int x = 1; x < glyphSize - 1 && !Erreur; x++)
                    {
                        for (int y = 1; y < glyphSize - 1 && !Erreur; y++)
                        {
                            if (BibliotequeGlyph.Biblioteque[j].matrice[x - 1, y - 1] != matrice[x, y])
                            {
                                Erreur = true;
                            }
                        }
                    }
                    if (Erreur == false) // Glyph trouvé
                    {
                        Idenfitifant = BibliotequeGlyph.Biblioteque[j].Identifiant;
                        positionBibliotheque = j;
                        return Idenfitifant;
                    }
                    matrice = rotationMatrice(matrice);
                }
            }
            return 0;
        }
    }
}
