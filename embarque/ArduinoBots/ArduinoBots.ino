
#include "includes.h"

#define   BAUDRATE_SERIAL    115200 // Débit du port série


#define   DEBUG             1


void setup()
{
  Serial.begin(BAUDRATE_SERIAL);     // Active le port série au bon débit
}

void loop()
{
  TrameProtocole * trame = getTrame();
  if(trame != NULL) // Nouvelle trame reçue
  {
    if(checkCrc(*trame))
    {
      SendTrame(*trame);
      //Serial.println("Trame Ok");
    }
    //Serial.println("Trame Ok");
    
    //Serial.println("Trame Ok");
  }
}
