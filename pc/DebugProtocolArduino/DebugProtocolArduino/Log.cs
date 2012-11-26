using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DebugProtocolArduino
{
    class Log
    {

        // TODO : Ajouter une liste d'objets atachées 
        // TODO : Ajouter des fichiers

        static RichTextBox m_RTB = null;

        public static void attachTo(RichTextBox RTB)
        {
            m_RTB = RTB;
        }


        public static void log(string str)
        {
            if (m_RTB != null)
                m_RTB.AppendText(str + '\n');
        }
    }
}
