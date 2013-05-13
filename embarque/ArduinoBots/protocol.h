/* Protocole de communication */
//#define   DEBUG_PROTOCOL      1 
#ifdef DEBUG_PROTOCOL

/** Definition du protocole **/
#define   PROTOCOL_ESCAPE      'e'//0xEC
/* Octets de start */
#define   PROTOCOL_START_1      'a'//0xAF
#define   PROTOCOL_START_2      'b'//0xC9
/* Octets de stop */
#define   PROTOCOL_STOP         'c'//0x8C

#else

/** Definition du protocole **/
#define   PROTOCOL_ESCAPE       0xEC //'e'//0xEC
/* Octets de start */
#define   PROTOCOL_START_1      0xAF // 'a'//0xAF
#define   PROTOCOL_START_2      0xC9 //'b'//0xC9
/* Octets de stop */
#define   PROTOCOL_STOP         0x8C //'c'//0x8C

#endif
#define   BUFFER_DATA_IN        50   // Nombre d'octets de data que le protocole peut envoyé en meme temps

/* Structure de la trame */
typedef struct t_TrameProtocole 
{
  byte src;
  byte dst;
  word num;
  byte length;
  byte data[BUFFER_DATA_IN];
  word crc;
} 
TrameProtocole;

/* Recupere la trame */
TrameProtocole * getTrame();

/* Calcul du CRC - 16 */
unsigned int crc16_protocole(TrameProtocole trame);

TrameProtocole * MakeTrame(byte src, byte dst, word num, byte data[]);
void SendTrame(TrameProtocole trame);

boolean checkCrc(TrameProtocole trame);
