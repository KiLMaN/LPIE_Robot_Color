using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;



namespace DebugProtocolArduino
{
    public partial class Form1 : Form
    {
        int BAUD_RATE = 115200;


        public Form1()
        {
            InitializeComponent();
            getListePortSerie();
            Log.attachTo(txtbox_debug_log);
        }

        /*delegate void LogLigne(string str);
        void writeLogDebugWin(string str)
        {
            txtbox_debug_log.AppendText(str + '\n');
        }
        void log(string str)
        {
            Invoke((LogLigne)writeLogDebugWin, str);
        }*/

        private void liste_portSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                
        }

        /* Appui sur le bouton Connection */
        private void btn_connection_Click(object sender, EventArgs e)
        {
            /* Déjà ouvert */
            if (gPortSerie.IsOpen)
            {
                Log.log("Fermeture du port : " + gPortSerie.PortName);
                gPortSerie.Close();
                btn_connection.Text = "Connection";
            }
            else
            {
                try
                {
                    Log.log("Ouverture du port : " + (string)liste_portSerie.SelectedItem);
                    gPortSerie.PortName = (string)liste_portSerie.SelectedItem;
                    gPortSerie.BaudRate = BAUD_RATE;
                    gPortSerie.Open();
                    btn_connection.Text = "Fermeture";
                }
                catch (Exception E)
                {
                    Log.log(E.Message.ToString());
                }
            }
        }
        /* Appui sur le bouton Actualiser */
        private void btn_ActualiserListePortSerie_Click(object sender, EventArgs e)
        {
            getListePortSerie();
        }

        /** Recupere la liste des ports séries disponibles sur la machine et l'affiche dans la liste **/
        public void getListePortSerie()
        {
            /* Protège contre l'actualisation apres ouverture du port */
            if (gPortSerie.IsOpen)
                return;

            /* Trouve les ports */
            string[] listePort = SerialPort.GetPortNames();
            

            /* Vide la liste */
            liste_portSerie.Items.Clear();
            /* Remplie la liste */
            foreach (string port in listePort)
                liste_portSerie.Items.Add(port);
            /* Selectionne le premier de la liste */
            liste_portSerie.SelectedIndex = 0 ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Protocol ptTest = new Protocol();
            ptTest.PortSerie = gPortSerie;
            Protocol.TrameProtocole trame = ptTest.MakeTrame(0x01,0x02,0x0001,new byte[]{0x01,0x25,0x32});
            Log.log(DateTime.Now.ToString("HH:mm:ss.ffffff") + '\n');
            ptTest.SendTrame(trame);
            Protocol.TrameProtocole newtrame ;

            System.Threading.Thread.Sleep(1000);
            while (gPortSerie.BytesToRead > 0 )  // Lit les données entrantes du port com
            {
                //int data = gPortSerie.ReadByte();
//Log.log(data.ToString());

                //Log.log(String.Format("0x{0:X}", gPortSerie.ReadByte()));
                Log.log(gPortSerie.ReadExisting());
            }

            /*while ((newtrame = ptTest.getTrame()).Equals(default(Protocol.TrameProtocole)));
            Log.log(DateTime.Now.ToString("HH:mm:ss.ffffff") + '\n');
            Log.log(newtrame.ToString());*/

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = { (byte)('s'),(byte)('d'),(byte)('n'),(byte)('n'),(byte)('1'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p'),(byte)('p') };


            ushort crc = crc16.calc_crc16(data, data.Length);
        }

        

       
    }
}
