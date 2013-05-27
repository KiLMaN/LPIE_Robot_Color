using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using utils;
using System.Xml;

namespace video
{
    public class BibliotequeGlyph
    {
        public class Tag
        {
            public Boolean[,] matrice;
            public int Type; // 0 pour Robot ; 1 => Glyph
            public int Identifiant;
            public Tag(int size)
            {
                matrice = new Boolean[size-2, size-2];
            }
        }
        public static List<Tag> Biblioteque = new List<Tag>();
       

        public int Size;

        public BibliotequeGlyph(int GlyphSize)
        {
            Size = GlyphSize;
        }
        public void chargementListeGlyph()
        {

            Biblioteque.Clear();
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load("glyph.xml");
                XmlNode node = document.DocumentElement;
               
                XmlNodeList nodeList = node.SelectNodes("glyph");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    Tag TagLec = new Tag(Size);
                    XmlNode nodeGlyph = nodeList[i];
                    TagLec.Type = Convert.ToInt16(nodeGlyph.SelectNodes("type").Item(0).InnerText);
                    TagLec.Identifiant = Convert.ToInt16 ( nodeGlyph.SelectNodes("id").Item(0).InnerText);

                    XmlNodeList LigneList = nodeGlyph.SelectNodes("lignes").Item(0).SelectNodes("ligne");
                    for (int j = 0; j < LigneList.Count; j++)
                    {
                        XmlNode Ligne = LigneList[j];
                        char[] lign = Ligne.InnerText.ToCharArray();
                        for (int k = 0; k < lign.Length; k++)
                        {
                            TagLec.matrice[j, k] = ( lign[k] == '1') ? false : true;
                        }
                    }
                    Biblioteque.Add(TagLec);
                    
                }

            }
            catch (Exception ex)
            {
                Logger.GlobalLogger.error("Erreur traitement fichier  Msg : " + ex.Message);
            }
            Logger.GlobalLogger.info("Chargement de " + Biblioteque.Count + " Glyphs");

        }
    }
}
