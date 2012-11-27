using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebugProtocolArduino
{
    class crc16
    {
        /* Implémentation du calcul du crc fait sur l'arduino */
        public static ushort calc_crc16(byte[] data, int length) 
        {
          ushort CRC = 0x0000;
          ushort A = 0, B = 0;
          int index = 0, Counter = 0; 

          for( index = 0; index < length; index++ )
          {
            byte _byte = data[index];

            A = (ushort)(CRC / 256);
            A = (ushort)(A ^ _byte);
            A = (ushort)(A * 256);

            B = (ushort)(CRC & 0xFF);
            CRC = (ushort)(A | B);

            for( Counter = 0; Counter < 8; Counter++ )
            {
              if ((ushort)(CRC & 0x8000) > (ushort)(0x0))
              {
                CRC = (ushort)(CRC * 2);
                CRC = (ushort)(CRC ^ 0x8005);
              }
              else
              {
                  CRC = (ushort)(CRC * 2);
              }
            }
          }
          return CRC;
        }
    }        
}
