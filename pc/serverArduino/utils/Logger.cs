using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace utils
{
    public class Logger
    {
        public static Logger GlobalLogger;

        public bool enableDebug = true;
        public bool enableInfo = true;
        public bool enableError = true;
        public int levelDebug = 0;
        
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

        #region #### RichTextBox ####
        public void attachToRTB(RichTextBox RTB)
        {
            m_RTB_list.Add(RTB);
            logToScreen("Logger Initialisé sur ce controle");
            //RTB.AppendText("Logger Initialisé sur ce controle \n");
        }
        #endregion

        #region #### InterThread ####
        delegate void d_writeToScreen(RichTextBox RTB, string str, Color couleur);
        void writeToScreen(RichTextBox RTB, string str, Color couleur)
        {
            RTB.SelectionStart = RTB.TextLength;
            RTB.SelectionLength = 0;

            RTB.SelectionColor = couleur;
            RTB.AppendText(str + '\n');
            RTB.SelectionColor = RTB.ForeColor;     
        }
        #endregion

        #region #### Affichage dans un fichier ####
        private void logToFile(string Message)
        {
            try
            {
                if(_Fichier == null)
                    _Fichier = new StreamWriter("log/" + LogFile, true);
                string format = "G";
                Message = DateTime.Now.ToString(format) + " : " + Message;
                _Fichier.Write(Message + "\r\n");
                

            }
            catch (Exception) { }
        }
        #endregion

        #region #### Log Global ####
        private void log(string str,Color couleur)
        {
            logToScreen(str,couleur);
            logToFile(str);
        }
        private void logToScreen(string str, Color couleur)
        {
            //riteToScreen(str, couleur);
            foreach (RichTextBox item in m_RTB_list)
            {
                try
                {
                    if (item != null)
                    {
                        if (item.InvokeRequired)
                            item.Invoke(new d_writeToScreen(writeToScreen), new object[] { item, str, couleur });
                        else
                            writeToScreen(item, str, couleur);
                    }
                }
                catch (Exception)
                {

                }
            }
            //System.Windows.Forms.invo();
        }
        private void logToScreen(string str)
        {
            logToScreen(str, Color.Black);
        }
        #endregion

        #region #### Fonctions de logs automatique ####
        public void error(string message)
        {
            if (enableError)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;

                log("[ERROR] [" + type.Name + "] [" + name + "] " + message, Color.Red);
            }
        }
        public void info(string message)
        {
            if (enableInfo)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;

                log("[INFO] [" + type.Name + "] [" + name + "] " + message, Color.DarkBlue);
            }
        }
        public void debug(string message,int level = 0)
        {
            if (enableDebug)
            {
                if (level >= levelDebug)
                {

                    StackFrame frame = new StackFrame(1);
                    var method = frame.GetMethod();
                    var type = method.DeclaringType;
                    var name = method.Name;

                    log("[DEBUG] [" + type.Name + "] [" + name + "] " + message, Color.Gray);
                }
            }
        }
        #endregion
    }
}
