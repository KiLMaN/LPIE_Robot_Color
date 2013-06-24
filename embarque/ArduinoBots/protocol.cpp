#include "includes.h"

/* State :
 0 -> Recherche du premier octet de start 
 1 -> Premier trouvé -> Recherche du second octet se start 
 2 -> Second trouvé  -> 1 octet de source 
 3 -> Source passée  -> 1 octet de destination
 4 -> Dest passée    -> 1 octet de numéro
 5 -> Numéro Ok      -> 1 octet de longeur
 6 -> Longeur OK     -> n octets de données
 7 -> Données OK     -> X octets de CRC
 8 -> CRC recu       -> Recherche de l'octet de stop
 9 -> Stop recu     -> Tramme recu :)
 
 Si > 100 alors le prochain byte est échapé (si c'est un flag, il deviens une data)
 */
int ProtocolState = 0;

/* Stockage de la trame temporairement */
TrameProtocole m_TrameReceive;

int tmp;

int CurrentLengthDatas = 0;

/* Parse la trame en fonction des données reçues et du state */
boolean parseTrame(int data)
{
  switch(ProtocolState)
  {
  case 2: // SOURCE (Copie directe)
    m_TrameReceive.src = data;
    return true;
    break;
  case 3: // DEST (Copie directe)
    m_TrameReceive.dst = data;
    return true;
    break;
  case 4: // NUMERO DE PACKET (Besoin de deux bytes)
    CurrentLengthDatas++;
    if(CurrentLengthDatas == 2) // On a besoin de deux bytes pour la longeur et le numéro 
    {
      m_TrameReceive.num = word(tmp,data);
      return true;
    }
    else 
    {
      tmp = data;
      return false;
    }
    break;
  case 5: // LONGEUR (Copie directe)
    m_TrameReceive.length = data;
    if( m_TrameReceive.length > BUFFER_DATA_IN ) // On envoi un trame trop longue 
    {

      ProtocolState = -1;
      return false;
    }
    //byte datas[m_TrameReceive.length];
    //m_TrameReceive.data = datas;
    return true;

    break;
  case 6: // DATAS (Besoin de m_TrameReceive.length bytes)
    m_TrameReceive.data[CurrentLengthDatas] = data; // remplis le tableau des données
    CurrentLengthDatas++;

    if(CurrentLengthDatas == m_TrameReceive.length)
    {
      return true;
    }
    else
      return false;
    break;
  case 7: //  CRC RECU (Besoin de deux bytes)
    CurrentLengthDatas++;
    if(CurrentLengthDatas == 2) // On a besoin de deux bytes pour le crc 
    {
      m_TrameReceive.crc = word(tmp,data);
      return true;
    }
    else 
    {
      tmp = data;
      return false;
    }
    break;
  }
  return false;
}


/* Recupere la trame découpée */
/* Aucune vérification de CRC ou de destinataire n'est fait */
/* Pour le CRC : crc16_protocole(Trame); */
TrameProtocole * getTrame()
{

  if(Serial.available() == 0) // Si auccune donnée n'est disponible 
    return NULL;

  int DataSerial;
  boolean TrameOk = false;

  while (Serial.available() > 0 && !TrameOk)  // Lit les données entrantes du port com
  {
    // Lit un octet du port série
    DataSerial = Serial.read();

    if(ProtocolState < 100) // Pas d'échapement
    {
      switch(DataSerial)
      {
        /* Premier Flag de start */
      case PROTOCOL_START_1: // Debut du message

        if(ProtocolState == 0) // Si on avait pas commencer un nouveau OK
        {
          ProtocolState = 1;
        }
        else                   // Sinon Erreur dans le protocol mais on prend quand même en compte la nouvelle trame
        {
          ProtocolState = 1;
        }
        break;

        /* Second Flag de start */
      case PROTOCOL_START_2:
        if(ProtocolState == 1) // Suit bien le premier flag, tout vas bien 
        {
          ProtocolState = 2;
        }
        else 
        { 
          ProtocolState = -1;
        }
        break;

        /* Flag de fin */
      case PROTOCOL_STOP:
        if(ProtocolState == 8)
        {
          ProtocolState = 0;
          TrameOk = true;
        }
        else 
        { 
          ProtocolState = -1;
        }
        break;

        /* Si c'est un escape Alors on ne prends pas en compte le prochain byte comme un flag */
      case PROTOCOL_ESCAPE: 
        ProtocolState += 100;// On ajoute l'échapement
        break;


        /* Ce n'est pas un flag */
      default:
        if(parseTrame(DataSerial)) // Si le parseTrame indique de passer a l'état suivant
        {
          CurrentLengthDatas = 0;
          ProtocolState++;
        }
        break;
      }
    }
    else // Echapé !
    {
      ProtocolState -= 100; // On enlève l'échapement
      if(parseTrame(DataSerial)) // Si le parseTrame indique de passer a l'état suivant
      {
        CurrentLengthDatas = 0;
        ProtocolState++;
      }
    }

    if(ProtocolState == -1) // Erreur dans l'ordre des Données
    {
      Serial.println("Protocol ERROR");
    }

    if(TrameOk)
    {
      return &m_TrameReceive;
    }
    else
      return NULL;
  } 
  return NULL;
}

/* Fabrique une trame a partir des informations données */
TrameProtocole * MakeTrame(byte src, byte dst, word num, byte data[],byte length)
{
  if ((sizeof(data))  > BUFFER_DATA_IN) // Taille trop grande
    return NULL;

	// Creation d'une nouvelle trame, a liberer apres
	//TrameProtocole * trame = (TrameProtocole *)malloc(sizeof(TrameProtocole));
	
  //TrameProtocole trame = new TrameProtocole();
  /*trame->src = src;
  trame->dst = dst;
  trame->num = num;
  trame->length = length;
  memcpy(trame->data, data , trame->length);
  trame->crc = crc16_protocole(*trame);*/
  
  m_TrameReceive.src = src;
  m_TrameReceive.dst = dst;
  m_TrameReceive.num = num;
  m_TrameReceive.length = length;
  memcpy(m_TrameReceive.data, data , m_TrameReceive.length);
  m_TrameReceive.crc = crc16_protocole(m_TrameReceive);
  return &m_TrameReceive;
}

/* Echape au besoin les valeurs pour les insérées dans la liste de bytes a transmètre */
void addToTrameBinary(byte* trameSortie, byte Donnee,int * index)
{
  switch(Donnee)
  {
  case PROTOCOL_START_1  : 
  case PROTOCOL_START_2  : 
  case PROTOCOL_STOP     : 
  case PROTOCOL_ESCAPE   : 
    trameSortie[*index] = PROTOCOL_ESCAPE; // on l'échape 
    //Serial.println("ADD PROTOCOL_ESCAPE");
    ++*index;
    break;
  }
  trameSortie[*index] = Donnee; // on l'échape 
  ++*index;
  //trameSortie.Add(Donnee); // et on l'ajoute
}

/* Creer une trame binaire pour l'envoi des informations sur le réseau */
/* Retourne le nombre de bytes a transmètre */
int MakeTrameBinary(TrameProtocole trame,byte * trameBinary)
{
  int index = 0;
  trameBinary[index] = PROTOCOL_START_1; // on l'échape 
  index++;
  trameBinary[index] = PROTOCOL_START_2; // on l'échape 
  index++;

  addToTrameBinary(trameBinary, trame.src,&index);
  addToTrameBinary(trameBinary, trame.dst,&index);

  addToTrameBinary(trameBinary, (byte)(trame.num >> 8),&index);
  addToTrameBinary(trameBinary, (byte)(trame.num & 0xFF),&index);

  addToTrameBinary(trameBinary, trame.length,&index);

  int i;
  for (i =0; i < trame.length ; i++)
    addToTrameBinary(trameBinary, trame.data[i],&index);

  addToTrameBinary(trameBinary, (byte)(trame.crc >> 8),&index);
  addToTrameBinary(trameBinary, (byte)(trame.crc & 0xFF),&index);

  trameBinary[index] = PROTOCOL_STOP; // on l'échape 

  return index+1;
}


/* Envoi la trame sur le réseau avec les champs calculés */
void SendTrame(TrameProtocole trame)
{
  // TODO optimiser : reduire taille buffer
  byte trameBinary[BUFFER_DATA_IN * 2]; // Maximum de deux fois la taille max

  int len = MakeTrameBinary(trame,trameBinary);
  //Log.log(Bin.ToString());

  int i = 0;
  for(i = 0; i < len; i++)
    Serial.write(trameBinary[i]);
    
    free(&trame);
}



/***************************************/ 
/***************************************/ 
/* Section calcul CRC et verification  */ 
/***************************************/ 
/***************************************/ 

/* Extrait les bytes pour le calcul CRC */
void getBytesCrc(TrameProtocole trame,byte * buffer)
{
  //byte[] retVal = buffer;
  int i ;
  int index = 0;
  buffer[index++] = trame.src;
  buffer[index++] = trame.dst;
  buffer[index++] = (byte)(trame.num >> 8);
  buffer[index++] = (byte)(trame.num & 0xFF);
  buffer[index++] = trame.length;

  for (i = 0; i <  trame.length ; i++ )
    buffer[index++] = trame.data[i];
}
/* Recupere les bytes pour composer le crc et le retourne */
unsigned int crc16_protocole(TrameProtocole trame)
{
  byte buff[trame.length + 5];
  getBytesCrc(trame,buff);
  return calc_crc16(buff,trame.length + 5);
}

/* Compare le crc reçu et celui calculé en local */
boolean checkCrc(TrameProtocole trame)
{
  return (crc16_protocole(trame) == trame.crc);
}



