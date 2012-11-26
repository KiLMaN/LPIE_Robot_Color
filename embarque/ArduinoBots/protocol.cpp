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
/* Parse la trame en fonction des données reçues et du state */
int CurrentLengthDatas = 0;

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



unsigned int crc16_protocole(TrameProtocole trame)
{
  return calc_crc16((byte *)&trame,trame.length + 5);
}
