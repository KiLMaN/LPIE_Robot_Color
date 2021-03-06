#include "includes.h"
#include <Servo.h>
#define   BAUDRATE_SERIAL    9600 // Débit du port série

//********** GLOBALS CONTROLE ROUES ***********
int STBY = 2;
int PWMA = 5;
int AIN1 = 3;
int AIN2 = 4;
int PWMB = 6;
int BIN1 = 8;
int BIN2 = 7;
unsigned long timeCmdWeelsCheck;
unsigned long timeCmdWeelsCompare;
unsigned long timeCmdTurnWeelsCheck;
unsigned long timeCmdTurnWeelsCompare;
//***********************************************

int USCaptPin = 12;
//pin 12 pour l'ultrason  ref : 


//************ GLOBALS COMM BYTES ***************
byte src = 0x03; //Id ROBOT (ref num on XBEE)
byte dst = 0xFE; //Id IA
byte ir_id = 0x01;
byte us_id = 0x02;
byte dataObstDetec[] = {0x70};

word lastRecvTramNum; 
byte highWordByte;
byte lowWordByte;
TrameProtocole *trameAckCmd;
TrameProtocole *trameAckPing;
byte dataAckPing[] = {0x22}; //réponse de ping
byte dataCommAsk[] = {0x12}; //demande connexion
word num = 0x0001; 
//***********************************************

//*********** GLOBALS COMMAND WORDS *************
boolean CLAWCLOSE = false;
boolean CLAWOPEN = false;
boolean GOFORWARD = false;
boolean GOBACKWARD = false;
boolean TURNTOLEFT = false;
boolean TURNTORIGHT = false;
boolean AUTOSTATE = false;

byte angleRate;
byte speedRate;
byte lenghtRate;
unsigned long timeRate;
//***********************************************

//*********** GLOBALS SERVO CONTROL *************
Servo clawServo;  // creation objet type servo pour ouvrir/fermer pince (a maximum of eight servo objects can be created) 
Servo armServo;   // creation objet type servo pour lever/baisser pince 
Servo headServo;  // creation objet type servo pour la tête du robot 
//***********************************************

int headPosition=0;
int rotationAngle=45;
unsigned long timeCheck;
unsigned long timeCompare;

unsigned long timeRollCheck;
unsigned long timeRollCompare;

boolean bolien = false;


void setup()
{
  //********** setup command weels *********
  pinMode(STBY, OUTPUT);
  pinMode(PWMA, OUTPUT);
  pinMode(AIN1, OUTPUT);
  pinMode(AIN2, OUTPUT);
  pinMode(PWMB, OUTPUT);
  pinMode(BIN1, OUTPUT);
  pinMode(BIN2, OUTPUT);
  //****************************************

  //********** setup command servo *********
  clawServo.attach(11);                   //
  armServo.attach(10);                    // assignation des pins PWM pour les servo-moteurs
  headServo.attach(9);                    //
  //****************************************
  pinMode(13,OUTPUT);
  Serial.begin(BAUDRATE_SERIAL);     // Active le port série au bon débit

  //timeCheck = millis();//take time

  //********* setup connexion init **********
  TrameProtocole *trameAskConnection = MakeTrame(src,dst, num, dataCommAsk, sizeof(dataCommAsk));
  SendTrame(*trameAskConnection);
  //*****************************************  
}



void loop()
{

  /*
  //à chaque entrée zde boucle on jete un oeil aux alentours (droit devant) pour verifier si il y a présence d'obstable
  //si oui, envoi 0x70  à l'IA
  int distanceObstacle = distanceMesuree();
  if(distanceObstacle < distanceminimale)
  {
    TrameProtocole *trameObstacleDetected = MakeTrame(src,dst, num, dataObstDetec, sizeof(dataObstDetec));
    SendTrame(*trameObstacleDetected);
  }
  
  
  
  */
  
  
  /*timeCompare= millis();//take time
   if((timeCompare-timeCheck)>=1000)
   {
   headServo.write(headPosition);
   headPosition+=rotationAngle;
   timeCheck = millis();
   if(headPosition==180)  
   rotationAngle=(-45);
   
   if(headPosition==0)
   rotationAngle=45;  
   }*/
  //verify time
  //if time elapsed , action
  //else nothing's appen



  
  digitalWrite(13,LOW);
  TrameProtocole * trame = getTrame();
  if(trame != NULL) // Nouvelle trame reçue
  {

    if(!checkCrc(*trame))
    {
      digitalWrite(13,HIGH);   ///si erreur CRC, la led de l'arduino s'allume
      delay(15);
    }
    if(checkCrc(*trame))
    {
      //*********** Mise en forme de l'acquitement de commande************
      lastRecvTramNum = trame->num; 
      highWordByte =(lastRecvTramNum >> 8)& 0xFF;
      lowWordByte = lastRecvTramNum & 0xFF;
      byte dataAckCmd[] = {0x41,highWordByte,lowWordByte,0x01};
      //******************************************************************
      timeRollCheck=millis();
      //SendTrame(*trame);
      //Serial.println("Trame Ok");

      //demande connexion

      switch(trame->data[0])
      {

        //**********************************************//
        //               Commandes pince                //
        //----------------------------------------------//
        //        0x61 = fermer     0x62 = ouvrir       //
        //**********************************************//  
      case 0x61: /*pas utilisé car géré dans le mode autonome*//*fermeture pince et levée du bras*/

        CLAWCLOSE = true;
        CLAWOPEN = false;
        GOFORWARD = false;
        GOBACKWARD = false;
        TURNTOLEFT = false;
        TURNTORIGHT = false;
        AUTOSTATE = false;
        /* acquitement de la commande*/
        trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
        SendTrame(*trameAckCmd);
        break;

      case 0x62: //abaissement du bras et ouverture de la pince 
        CLAWCLOSE = false;
        CLAWOPEN = true;
        GOFORWARD = false;
        GOBACKWARD = false;
        TURNTOLEFT = false;
        TURNTORIGHT = false;
        AUTOSTATE = false;
        /* acquitement de la commande*/
        trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
        SendTrame(*trameAckCmd);
        break;

        //**********************************************//
        //             Commandes deplacement            //
        //----------------------------------------------//
        // 0x51 = tourner                               //
        //    --> 0x00/0x01 = tourner à gauche/droite   //
        // 0x52 = ligne droite                          //
        //    --> 0x00/0x01 = avancer / reculer         //
        //**********************************************//


      case 0x51: //commande tourner
        if(trame->data[1]==0x00) //gauche
        {
          CLAWCLOSE = false;
          CLAWOPEN = false;
          GOFORWARD = false;
          GOBACKWARD = false;
          TURNTOLEFT = true;
          TURNTORIGHT = false;
          AUTOSTATE = false;
          angleRate = trame->data[2];
          timeCmdTurnWeelsCheck = millis();


          /* acquitement de la commande*/
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        else if(trame->data[1]==0x01) //droite
        {
          CLAWCLOSE = false;
          CLAWOPEN = false;
          GOFORWARD = false;
          GOBACKWARD = false;
          TURNTOLEFT = false;
          TURNTORIGHT = true;
          AUTOSTATE = false;
          angleRate = trame->data[2];
          timeCmdTurnWeelsCheck = millis();
          
          /* acquitement de la commande*/
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        break;
      case 0x52:  //commande ligne droite
        if(trame->data[1]==0x00)  //avant
        {
          CLAWCLOSE = false;
          CLAWOPEN = false;
          GOFORWARD = true;
          GOBACKWARD = false;
          TURNTOLEFT = false;
          TURNTORIGHT = false;
          AUTOSTATE = false;
          speedRate = trame->data[2];
          lenghtRate = trame->data[3];
          timeCmdWeelsCheck = millis();
    
          /* acquitement de la commande*/
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        else if(trame->data[1]==0x01) //arriere
        {
          CLAWCLOSE = false;
          CLAWOPEN = false;
          GOFORWARD = false;
          GOBACKWARD = true;
          TURNTOLEFT = false;
          TURNTORIGHT = false;
          AUTOSTATE = false;
          speedRate = trame->data[2];
          lenghtRate = trame->data[3];
          timeCmdWeelsCheck = millis();
          
          /* acquitement de la commande*/
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        } 
        break;

        //**********************************************//
        //                Réponse de ping               //
        //----------------------------------------------//
        // 0x21 = demande de ping (serveur->robot)      //
        // 0x22 = réponse de ping (robot->serveur)      //
        //**********************************************//   
      case 0x21:
        lastRecvTramNum = trame->num;  
        trameAckPing = MakeTrame(src,dst, lastRecvTramNum, dataAckPing,sizeof(dataAckPing));
        SendTrame(*trameAckPing);
        break;

        //**********************************************//
        //                Réponse capteurs              //
        //----------------------------------------------//
        // 0x31 = demande de ping (serveur->robot)      //
        //   --> 0x01 = capteur IR                      //
        //   --> 0x02 = capteur US                      //
        //**********************************************//
      case 0x31:
        if(trame->data[1]==0x01)
        {
          //réponse capteur IR
        }
        else if(trame->data[1]==0x02)
        {
          //réponse capteur US
        } 
        break;

        //**********************************************//
        //            Réponse accès réseau              //
        //----------------------------------------------//
        // 0x11 = Réponse de connexion réseau           //
        //   -->0x01 = connexion acceptée               //
        //   -->0x00 = connexion refusée                //
        //**********************************************//
      case 0x11:
        if(trame->data[1]==0x01) ///accepté
        {
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        else if(trame->data[1]==0x00) ///refusé ,dans ce cas on réenvoie une demande de connexion
        {
          TrameProtocole *trameAskConnection = MakeTrame(src,dst, num, dataCommAsk,sizeof(dataCommAsk));
          SendTrame(*trameAskConnection);
        }
        break;
        
        //**********************************************//
        //            Passage mode autonome             //
        //----------------------------------------------//
        // 0x71 = Passage mode autonome                 //
        //                                              //
        //                                              //
        //**********************************************//
      case 0x71:
        CLAWCLOSE = false;
        CLAWOPEN = false;
        GOFORWARD = false;
        GOBACKWARD = false;
        TURNTOLEFT = false;
        TURNTORIGHT = false;
        AUTOSTATE = true;
        break;   
      }
    }
  }

  if(AUTOSTATE)
  {
    //implémenter braitenberg
    
    
  }

  if(CLAWOPEN)
  {
    clawServo.write(0);
    delay(100);
    //armServo.write(-x);
  }
  if(CLAWCLOSE)
  {
    clawServo.write(180);
    delay(100);
  }

  if(GOFORWARD)
  {
    timeRate=((long)lenghtRate*1000)/25;
    timeCmdWeelsCompare = millis();
    if((timeCmdWeelsCompare-timeCmdWeelsCheck)<timeRate)
    {
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,LOW);
      digitalWrite(AIN2,HIGH);
      analogWrite(PWMA,speedRate);
      digitalWrite(BIN1,LOW);
      digitalWrite(BIN2,HIGH);
      analogWrite(PWMB,speedRate+10);
    }
    else 
    {
      digitalWrite(STBY,LOW);
    }
  }

  if(GOBACKWARD)
  {
    timeRate=((long)lenghtRate*1000)/25;
    timeCmdWeelsCompare = millis();
    if((timeCmdWeelsCompare-timeCmdWeelsCheck)<timeRate) 
    {
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,HIGH);
      digitalWrite(AIN2,LOW);
      analogWrite(PWMA,speedRate);
      digitalWrite(BIN1,HIGH);
      digitalWrite(BIN2,LOW);
      analogWrite(PWMB,speedRate);
    }
    else 
    {
      digitalWrite(STBY,LOW);
    }
  }

  if(TURNTOLEFT)
  {
    timeRate=(long)((long)angleRate*4.3+110);
    timeCmdTurnWeelsCompare = millis();
    if((timeCmdTurnWeelsCompare-timeCmdTurnWeelsCheck)<timeRate)
    {
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,LOW);
      digitalWrite(AIN2,HIGH);
      analogWrite(PWMA,70);
      digitalWrite(BIN1,HIGH);
      digitalWrite(BIN2,LOW);
      analogWrite(PWMB,70);
    }
    else 
    {
      digitalWrite(STBY,LOW);
    }
  }

  if(TURNTORIGHT)
  {
    timeRate=(long)((long)angleRate*4.3+110);
    timeCmdTurnWeelsCompare = millis();
    if((timeCmdTurnWeelsCompare-timeCmdTurnWeelsCheck)<timeRate)
    {
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,HIGH);
      digitalWrite(AIN2,LOW);
      analogWrite(PWMA,70);
      digitalWrite(BIN1,LOW);
      digitalWrite(BIN2,HIGH);
      analogWrite(PWMB,70);
    }
    else 
    {
      digitalWrite(STBY,LOW);
    }
  }
}







