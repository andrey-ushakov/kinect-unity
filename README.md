# Projet Kinect
ENSIIE / semestre 5 / Interactions Et Capteurs

Le but de ce projet est de programmer des interactions gestuelles de différents types dans un jeu sous Unity, en utilisant la Kinect

#### Menu de démarrage
Au lancement du jeu, le joueur doit sélectionner une activité parmi 2 proposées, ou bien quitter.

#### Activité 1 : Head-tracking2
Le programme affiche une scène 3d fixe avec des objets à différentes profondeurs.

Le point de vue sur la scène est modifié selon la position de l’utilisateur pour donner la perception
de ces profondeurs (i.e. " parallaxe de mouvement ").

L’utilisateur peut revenir au menu à tout moment.

#### Activité 2 : Jeu
Le jeu fait apparaitre des objets de manière aléatoire en haut, à gauche et à droite de l’écran. Ils se déplacent en direction du joueur, qui doit faire un geste au bon moment pour les détruire :
- Geste de la main (laquelle...) vers la droite (resp. gauche) pour détruire l’objet à droite
(resp. gauche)
- Geste des 2 mains vers le haut pour détruire l’objet en haut

A chaque réussite, un score augmente, sinon il diminue.

Le jeu dure X secondes maximum, et s’arrête lorsque le score atteint une limite minimum.

Affichage durant le jeu :
- Indice de réussite ou non de la destruction
- Score en cours

A la fin, affichage du score final et possibilité de revenir au menu.

