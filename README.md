LPIE ROBOT COLOR
================

Introduction
------------

Le projet créer pour l'examen de la Licence Pro. Informatique Embarquée de L'ITIN en partenariat avec L'UCP,  
A pour but de creer des unités mobiles communicantes chagées de déplacer des objets de couleur dans des zones prédéfinies


Fonctionnement
--------------

### Infos Globales
Un protocole spécifique est mis en place pour envoyer et recevoir depuis / vers les modules embarqués 

### Robots

Les robots basés sur des arduino communiquent avec le pc central via une liaison Xbee.  
Les robots sont équipés d'une pince controlée par des servomoteurs qui lui permet de saisir les objets  
Ils sont aussi équipés de capteurs infrarouge et ultrasonic pour etudier l'environement du robot lors de ses déplacements  

### PC

Le pc est équipé d'une caméra de type 'webcam' de haute qualitée qui filme les déplacements et positions des robots et de leurs cibles  
Cette information est décodée puis traitée via des algorithmes pour envoyer des commandes au robots 