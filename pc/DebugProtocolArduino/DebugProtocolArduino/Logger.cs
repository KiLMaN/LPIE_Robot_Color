using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DebugProtocolArduino
{
    public class Logger
    {

        public static Logger GlobalLogger;
        private List<RichTextBox> m_RTB_list = new List<RichTextBox>();
        private StreamWriter _Fichier;
        private string _LogFile = "log.log";
       
       
        public string LogFile
        {
            get
            {
                return _LogFile;
            }
            set
            {
                _LogFile = value;
            }
        }





        public void attachToRTB(RichTextBox RTB)
        {
            m_RTB_list.Add(RTB);
            RTB.AppendText("Logger Initialisé sur ce controle \n");

        }
        public void logToScreen(string str)
        {
            foreach (RichTextBox item in m_RTB_list)
                if (item != null)
                    item.AppendText(str + '\n');
        }
        public void logToFile(string Message)
        {
            try
            {
                _Fichier = new StreamWriter("log/" + LogFile, true);
                string format = "G";
                Message = DateTime.Now.ToString(format) + " : " + Message;
                _Fichier.Write(Message + "\r\n");
                _Fichier.Close();
                
            }
            catch (Exception) { }
        }

        public void log(string str)
        {
            logToScreen(str);
            logToFile(str);
        }
    }
}
