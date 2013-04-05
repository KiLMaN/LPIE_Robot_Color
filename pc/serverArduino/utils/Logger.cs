using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace utils
{
    public class Logger
    {
        public static bool enableDebug = true;
        public static bool enableInfo = true;
        public static bool enableError = true;

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
        private void logToScreen(string str)
        {
            // Faire le log to screen qui fonctionne interthread (invoke)
            foreach (RichTextBox item in m_RTB_list)
            {
                try
                {
                    if (item != null)
                        item.AppendText(str + '\n');
                }
                catch (Exception )
                {

                }
            }


        }
        private void logToFile(string Message)
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

        private void log(string str)
        {
            logToScreen(str);
            logToFile(str);
        }


        public void error(string message)
        {
            if (enableError)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;

                log("[ERROR] [" + type.Name + "] [" + name + "] " + message);
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

                log("[INFO] [" + type.Name + "] [" + name + "] " + message);
            }
        }
        public void debug(string message)
        {
            if (enableDebug)
            {
                StackFrame frame = new StackFrame(1);
                var method = frame.GetMethod();
                var type = method.DeclaringType;
                var name = method.Name;

                log("[DEBUG] [" + type.Name + "] [" + name + "] " + message);
            }
        }
    }
}
