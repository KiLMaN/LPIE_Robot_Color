
#include "includes.h"

#define   BAUDRATE_SERIAL    9600 // Débit du port série

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
          
      
      //SendTrame(*trame);
      //Serial.println("Trame Ok");
            lastRecvTramNum = trame->num; 
            highWordByte =(lastRecvTramNum >> 8)& 0xFF;
            lowWordByte = lastRecvTramNum & 0xFF;
            byte dataAckCmd[] = { 0x41,highWordByte,lowWordByte,0x01}; //demande connexion
      byte data[] = {0x72};
      switch(trame->data[0])
      {
            
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
