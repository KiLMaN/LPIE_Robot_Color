LPIE ROBOT COLOR
================

Introduction
------------

Le projet cr�er pour l'examen de la Licence Pro. Informatique Embarqu�e de L'ITIN en partenariat avec L'UCP,  
A pour but de creer des unit�s mobiles communicantes chag�es de d�placer des objets de couleur dans des zones pr�d�finies


Fonctionnement
--------------

### Infos Globales
Un protocole sp�cifique est mis en place pour envoyer et recevoir depuis / vers les modules embarqu�s 

### Robots

Les robots bas�s sur des arduino communiquent avec le pc central via une liaison Xbee.  
Les robots sont �quip�s d'une pince control�e par des servomoteurs qui lui permet de saisir les objets  
Ils sont aussi �quip�s de capteurs infrarouge et ultrasonic pour etudier l'environement du robot lors de ses d�placements  

### PC

Le pc est �quip� d'une cam�ra de type 'webcam' de haute qualit�e qui filme les d�placements et positions des robots et de leurs cibles  
Cette information est d�cod�e puis trait�e via des algorithmes pour envoyer des commandes au robots 