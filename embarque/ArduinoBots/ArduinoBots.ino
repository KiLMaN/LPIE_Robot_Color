#include "includes.h"
#include <Servo.h>
#define   BAUDRATE_SERIAL    9600 // Débit du port série

<<<<<<< HEAD
=======
<<<<<<< HEAD
#include <Servo.h> 

byte src = 0x01;
byte dst = 0xFE;

int headPosition=0;
int rotationAngle=45;
unsigned long timeCheck;
unsigned long timeCompare;
boolean bolien = false;
Servo clawServo;  // creation objet type servo pour ouvrir/fermer pince (a maximum of eight servo objects can be created) 
Servo armServo;   // creation objet type servo pour lever/baisser pince 
Servo headServo;  // creation objet type servo pour la tête du robot 
    
word lastRecvTramNum; 
byte highWordByte;
byte lowWordByte;
TrameProtocole *trameAckCmd;
TrameProtocole *trameAckPing;
byte dataAckPing[1] = {0x22}; //demande connexion
    
void setup()
{
  pinMode(13,OUTPUT);
  Serial.begin(BAUDRATE_SERIAL);     // Active le port série au bon débit
   clawServo.attach(11);              //
   armServo.attach(10);               // assignation des pins PWM pour les servo-moteurs
   headServo.attach(9);               //
   timeCheck = millis();//take time
   
   
   word num = 0x0001;
   byte data[] = { 0x12}; //demande connexion
  TrameProtocole *trameAskConnection = MakeTrame(src,dst, num, data,sizeof(data));
  // TrameProtocole *trame= MakeTrame(0x01, 0xFE, 0x01, 0x12);
   SendTrame(*trameAskConnection);
=======
>>>>>>> 5803f8ed12263c16ba03bcc6dacfb7c4cb5f5158
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

boolean uniquePass;
//***********************************************

//************ GLOBALS COMM BYTES ***************
byte src = 0x03; //Id ROBOT (ref num on XBEE)
byte dst = 0xFE; //Id IA
word lastRecvTramNum; 
byte highWordByte;
byte lowWordByte;
TrameProtocole *trameAckCmd;
TrameProtocole *trameAckPing;
byte dataAckPing[1] = {
  0x22}; //réponse de ping
byte dataCommAsk[] = {
  0x12}; //demande connexion
word num = 0x0001; 
//byte data[4];
//***********************************************

//*********** GLOBALS COMMAND WORDS *************
boolean CLAWCLOSE = false;
boolean CLAWOPEN = false;
boolean GOTOWARD = false;
boolean GOFORWARD = false;
boolean TURNTOLEFT = false;
boolean TURNTORIGHT = false;
byte angleRate;
byte speedRate;
byte lenghtRate;
byte timeRate;
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
<<<<<<< HEAD
=======
>>>>>>> upload robots
>>>>>>> 5803f8ed12263c16ba03bcc6dacfb7c4cb5f5158
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
      digitalWrite(13,HIGH);   ///si erreur CRC, la led de l'arduino s'allume
      delay(15);
    }
    if(checkCrc(*trame))
    {
<<<<<<< HEAD
=======
<<<<<<< HEAD
          
      
=======
>>>>>>> 5803f8ed12263c16ba03bcc6dacfb7c4cb5f5158
      //*********** Mise en forme de l'acquitement de commande************
      lastRecvTramNum = trame->num; 
      highWordByte =(lastRecvTramNum >> 8)& 0xFF;
      lowWordByte = lastRecvTramNum & 0xFF;
      byte dataAckCmd[] = {
        0x41,highWordByte,lowWordByte,0x01      };
      //******************************************************************
      timeRollCheck=millis();
>>>>>>> upload robots
      //SendTrame(*trame);
      //Serial.println("Trame Ok");
            lastRecvTramNum = trame->num; 
            highWordByte =(lastRecvTramNum >> 8)& 0xFF;
            lowWordByte = lastRecvTramNum & 0xFF;
            byte dataAckCmd[] = { 0x41,highWordByte,lowWordByte,0x01}; //demande connexion
      byte data[] = {0x72};
      switch(trame->data[0])
      {
<<<<<<< HEAD
=======
<<<<<<< HEAD
            
       //**********************************************//
       //               Commandes pince                //
       //----------------------------------------------//
       //        0x61 = fermer     0x62 = ouvrir       //
       //**********************************************//  
        case 0x61: 
            //ack commande
            clawServo.write(180);
            
            

              trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckCmd);
            //armServo.write(180); //lever pince
           break;
        case 0x62:
            //ack commande  
            clawServo.write(0);
            

              trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckCmd);
            //armServo.write(180); //baisser pince
           break;
          
       //**********************************************//
       //             Commandes deplacement            //
       //----------------------------------------------//
       // 0x51 = tourner                               //
       //    --> 0x00/0x01 = tourner à gauche/droite   //
       // 0x52 = ligne droite                          //
       //    --> 0x00/0x01 = avancer / reculer         //
       //**********************************************//
        case 0x51: 
           if(trame->data[1]==0x00)
           {
             //ack commande
              trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckCmd);
           }
           else if(trame->data[1]==0x01)
           {
             trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckCmd);
             //ack commande
           }
          break;
        case 0x52:
           if(trame->data[1]==0x00)
           {
              trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckCmd);
             //ack commande
           }
           else if(trame->data[1]==0x01)
           {
              trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckCmd);
             //ack commande
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
          
          case 0x71:
            //envoyer 0x22
              lastRecvTramNum = trame->num;  
             
         
             
              trameAckCmd = MakeTrame(src,dst, lastRecvTramNum, dataAckCmd,sizeof(dataAckCmd));
              SendTrame(*trameAckPing);
              
              delay(1000);
              
               trameAckPing = MakeTrame(src,dst, lastRecvTramNum++, data,sizeof(data));
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
             
             byte dataAckCmd[4] = { 0x41,highWordByte,lowWordByte,0x01}; //demande connexion

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
}
=======
>>>>>>> 5803f8ed12263c16ba03bcc6dacfb7c4cb5f5158

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
          GOTOWARD = false;
          GOFORWARD = false;
          TURNTOLEFT = false;
          TURNTORIGHT = true;
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
          GOTOWARD = true;
          GOFORWARD = false;
          TURNTOLEFT = false;
          TURNTORIGHT = false;
          speedRate = trame->data[2];
          lenghtRate = trame->data[3];
timeCmdWeelsCheck = millis();
uniquePass=true;
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
          speedRate = trame->data[2];
          lenghtRate = trame->data[3];
timeCmdWeelsCheck = millis();
uniquePass=true;
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

      }
    }


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

  if(GOTOWARD)
  {
    
    timeRate=(lenghtRate*1000)/14;
    timeCmdWeelsCompare = millis();
    if((timeCmdWeelsCompare-timeCmdWeelsCheck)<timeRate)//remplacer 10000 par timerate
<<<<<<< HEAD
    {
      //uniquePass=false;
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,LOW);
      digitalWrite(AIN2,HIGH);
      analogWrite(PWMA,speedRate);
      digitalWrite(BIN1,LOW);
      digitalWrite(BIN2,HIGH);
      analogWrite(PWMB,speedRate+15);
      
    }
    else //if(timeCmdWeelsCompare-timeCmdWeelsCheck>=10000)
=======
>>>>>>> 5803f8ed12263c16ba03bcc6dacfb7c4cb5f5158
    {
      //uniquePass=false;
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,LOW);
      digitalWrite(AIN2,HIGH);
      analogWrite(PWMA,speedRate);
      digitalWrite(BIN1,LOW);
      digitalWrite(BIN2,HIGH);
      analogWrite(PWMB,speedRate+15);
      
      digitalWrite(STBY,LOW);
    }
    else //if(timeCmdWeelsCompare-timeCmdWeelsCheck>=10000)
    {
      
      digitalWrite(STBY,LOW);
    }
  }

  if(GOFORWARD)
  {
     timeRate=(lenghtRate*1000)/14;
    timeCmdWeelsCompare = millis();
    if((timeCmdWeelsCompare-timeCmdWeelsCheck)<timeRate) //remplacer 10000 par timerate
    {
      //uniquePass=false;
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,HIGH);
      digitalWrite(AIN2,LOW);
      analogWrite(PWMA,speedRate);
      digitalWrite(BIN1,HIGH);
      digitalWrite(BIN2,LOW);
      analogWrite(PWMB,speedRate+15);
   }
    else //if((timeCmdWeelsCompare-timeCmdWeelsCheck)>=10000)
    {
      
      digitalWrite(STBY,LOW);
    }
  }

  if(TURNTOLEFT)
  {
     timeRate=(angleRate*1000)/190;

    timeCmdTurnWeelsCompare = millis();
    if((timeCmdTurnWeelsCompare-timeCmdTurnWeelsCheck)<timeRate)//remplacer 10000 par timerate
    {
      //uniquePass=false;
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,LOW);
      digitalWrite(AIN2,HIGH);
      analogWrite(PWMA,150);
      digitalWrite(BIN1,HIGH);
      digitalWrite(BIN2,LOW);
      analogWrite(PWMB,150);
   }
    else //if((timeCmdWeelsCompare-timeCmdWeelsCheck)>=10000)
    {
      
      digitalWrite(STBY,LOW);
    }
  }

  if(TURNTORIGHT)
  {
    timeRate=(angleRate*1000)/190;
    timeCmdTurnWeelsCompare = millis();
    if((timeCmdTurnWeelsCompare-timeCmdTurnWeelsCheck)<timeRate)//remplacer 10000 par timerate
    {
      //uniquePass=false;
      digitalWrite(STBY,HIGH);
      digitalWrite(AIN1,HIGH);
      digitalWrite(AIN2,LOW);
      analogWrite(PWMA,150);
      digitalWrite(BIN1,LOW);
      digitalWrite(BIN2,HIGH);
      analogWrite(PWMB,150);
   }
    else //if((timeCmdWeelsCompare-timeCmdWeelsCheck)>=10000)
    {
      
      digitalWrite(STBY,LOW);
    }
  }



}




<<<<<<< HEAD
=======
>>>>>>> upload robots
>>>>>>> 5803f8ed12263c16ba03bcc6dacfb7c4cb5f5158
