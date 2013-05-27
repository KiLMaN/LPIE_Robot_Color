using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace utils
{
    public struct PointDessin
    {
        public int X;
        public int Y;
    }

    public class PolyligneDessin
    {
        private List<PointDessin> _listePoint;
        private System.Drawing.Color _couleur;

        #region #### Constructeurs ####

        #region #### Normal ####
        public PolyligneDessin()
        {
            _listePoint = new List<PointDessin>(); 
            _couleur = System.Drawing.Color.Black;
        }
        public PolyligneDessin(PointDessin pointDepart)
            :this()
        {
            _listePoint.Add(pointDepart);
        }
        public PolyligneDessin(List<PointDessin> points)
            : this()
        {
            _listePoint.AddRange(points);
        }
        #endregion

        #region #### Couleurs ####
        public PolyligneDessin(System.Drawing.Color couleur)
             : this()
        {
            _couleur = couleur;
        }
       public PolyligneDessin(PointDessin pointDepart,System.Drawing.Color couleur)
            :this(pointDepart)
        {
            _couleur = couleur;
        }
        public PolyligneDessin(List<PointDessin> points,System.Drawing.Color couleur)
            : this(points)
        {
            _couleur = couleur;
        }
        #endregion

        #endregion

        #region #### Ajouts ####
        public void addPoint(PointDessin pt)
        {
            _listePoint.Add(pt);
        }
        public void addPoint(PointDessin pt, int index)
        {
            _listePoint.Insert(index, pt);
        }
        #endregion

        #region #### Supression ####
        public void removePoint(PointDessin pt)
        {
            _listePoint.Remove(pt);
        }
        public void removePoint(int index)
        {
            _listePoint.RemoveAt(index);
        }
        #endregion

        #region #### Recuperation ####
        public List<PointDessin> ListePoint
        { get { return _listePoint; } }
        public System.Drawing.Color Couleur
        { get { return _couleur; } }
        #endregion
    }
}
