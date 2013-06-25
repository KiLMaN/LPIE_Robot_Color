
#include "includes.h"

#define   BAUDRATE_SERIAL    9600 // Débit du port série

#include <Servo.h> 

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
//***********************************************



byte src = 0x03;
byte dst = 0xFE;

int headPosition=0;
int rotationAngle=45;
unsigned long timeCheck;
unsigned long timeCompare;

unsigned long timeRollCheck;
unsigned long timeRollCompare;

boolean CLAWCLOSE = false;
boolean CLAWOPEN = false;
boolean GOTOWARD = false;
boolean GOFORWARD = false;
boolean TURNTOLEFT = false;
boolean TURNTORIGHT = false;
byte angleRate;
byte speedRate;
byte lenghtRate;


boolean bolien = false;
Servo clawServo;  // creation objet type servo pour ouvrir/fermer pince (a maximum of eight servo objects can be created) 
Servo armServo;   // creation objet type servo pour lever/baisser pince 
Servo headServo;  // creation objet type servo pour la tête du robot 

word lastRecvTramNum; 
byte highWordByte;
byte lowWordByte;
TrameProtocole *trameAckCmd;
TrameProtocole *trameAckPing;
byte dataAckPing[1] = {
  0x22}; //demande connexion

void setup()
{
  //********** setup command weels *********
  timeCmdWeelsCheck = millis();
  //****************************************
  pinMode(13,OUTPUT);
  Serial.begin(BAUDRATE_SERIAL);     // Active le port série au bon débit
  clawServo.attach(11);              //
  armServo.attach(10);               // assignation des pins PWM pour les servo-moteurs
  headServo.attach(9);               //
  timeCheck = millis();//take time


  word num = 0x0001;
  byte data[] = { 
    0x12  }; //demande connexion
  TrameProtocole *trameAskConnection = MakeTrame(src,dst, num, data,sizeof(data));
  // TrameProtocole *trame= MakeTrame(0x01, 0xFE, 0x01, 0x12);
  SendTrame(*trameAskConnection);
}

void loop()
{

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



  //TrameProtocole * trame = getTrame();
  digitalWrite(13,LOW);
  TrameProtocole * trame = getTrame();
  if(trame != NULL) // Nouvelle trame reçue
  {

    if(!checkCrc(*trame))
    {
      digitalWrite(13,HIGH);
      delay(15);
    }
    if(checkCrc(*trame))
    {
      lastRecvTramNum = trame->num; 
      highWordByte =(lastRecvTramNum >> 8)& 0xFF;
      lowWordByte = lastRecvTramNum & 0xFF;
      byte dataAckCmd[] = {0x41,highWordByte,lowWordByte,0x01};

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
      case 0x61: /*pas utilisé car géré dans le mode autonome*/

        CLAWCLOSE = true;
        CLAWOPEN = false;
        GOTOWARD = false;
        GOFORWARD = false;
        TURNTOLEFT = false;
        TURNTORIGHT = false;

        /* acquitement de la commande*/
        trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
        SendTrame(*trameAckCmd);
        ///clawServo.write(180);
        ///armServo.write(x);
        break;

      case 0x62: //abaissement du bras et ouverture de la pince 
        CLAWCLOSE = false;
        CLAWOPEN = true;
        GOTOWARD = false;
        GOFORWARD = false;
        TURNTOLEFT = false;
        TURNTORIGHT = false;

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
          GOTOWARD = false;
          GOFORWARD = false;
          TURNTOLEFT = true;
          TURNTORIGHT = false;
          angleRate = data[2];

          /* acquitement de la commande*/
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        else if(trame->data[1]==0x01) //droite
        {
          CLAWCLOSE = false;
          CLAWOPEN = false;
          GOTOWARD = false;
          GOFORWARD = false;
          TURNTOLEFT = false;
          TURNTORIGHT = true;
          angleRate = data[2];

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
          GOTOWARD = true;
          GOFORWARD = false;
          TURNTOLEFT = false;
          TURNTORIGHT = false;
          speedRate = data[2];
          lenghtRate = data[3];

          /* acquitement de la commande*/
          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        else if(trame->data[1]==0x01) //arriere
        {
          CLAWCLOSE = false;
          CLAWOPEN = false;
          GOTOWARD = false;
          GOFORWARD = true;
          TURNTOLEFT = false;
          TURNTORIGHT = false;
          speedRate = data[2];
          lenghtRate = data[3];

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
        //envoyer 0x22
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
        // 0x11 =                                       //
        //                                              //
        //**********************************************//
      case 0x11:
        if(trame->data[1]==0x01) ///accepté
        {
          lastRecvTramNum = trame->num;  

          highWordByte =(lastRecvTramNum >> 8)& 0xFF;
          lowWordByte = lastRecvTramNum & 0xFF;

          byte dataAckCmd[4] = { 
            0x41,highWordByte,lowWordByte,0x01          }; //demande connexion

          trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
          SendTrame(*trameAckCmd);
        }
        else if(trame->data[1]==0x00) ///refusé
        {
          lastRecvTramNum = trame->num;  
        }


        clawServo.write(0);
        break;

      }
      //************************************************

      //***********************************************
    }
    //Serial.println("Trame Ok");

    //Serial.println("Trame Ok");
  }

  if(CLAWOPEN)
  {
    clawServo.write(0);
    //armServo.write(-x);
  }
  if(CLAWCLOSE)
  {
    clawServo.write(180);
  }
  
  if(GOTOWARD)
  {
    timeCmdWeelsCompare = millis();
    if(timeCmdWeelsCompare-timeCmdWeelsCheck)
    {
      
    }
  }
  
  if(GOFORWARD)
  {
  }
  
  if(TURNTOLEFT)
  {
  }
  
  if(TURNTORIGHT)
  {
  }

  
}


