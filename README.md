# AmazonAsinTracker
Track amazon article review by asin code

## Enoncé de l'exercice

https://www.notion.so/Backend-Home-Project-5501b0773ec048b289703f7666875141

## Pour tester la solution
Lancer simultanément les projets AmazonAsinTrackerCron et AmazonAsinTrackerApi
Utiliser les endpoints fournis par l'API afin de mettre en place le tracking et voir les review des produits tracked.

## Liste des choses à faire pour basculer le POC en prod
Beaucoup de compromis ont été fait afin d'essayer de respecter une contrainte de temps pour réaliser le POC.


  * Il faudrait donc découper la partie infrastructure afin de spécialiser entre les besoins du Cron et de l'API et d'adapter la DI en fonction.
  * Rajouter une notion d'utilisateur qui fait la demande de tracking
  * Avoir une notion d'utilisateur Authentifié
  * D'un point de vue DDD, c'est un User qui crée la demande de tracking
  * Le storage actuel est du fileSystem avec écrasement à chaque nouvel appel, il faudrait passer sur un véritable storage (probablement mongo, raven...)
  * Dans le POC je ne connais pas l'utilisateur qui initie la demande, je ne peux donc pas le notifier, je ne sais  d'ailleurs pas comment le notifier (à spécifier, mail, signalR, ... ?)
  * Au niveau de la persistence, il faudrait prévoir un système capable d'allez lire toutes les review de manière incrémentale, et de stocker uniquement les nouvelles review par rapport à celles connues.
  * La lecture des review renvoit actuellement les 10 dernières review, mettre en place au niveau de l'API (via QueryOptions) un système de querying
  * Vu que c'est un POC, il n'y a aucune notion de log ni de gestion d'erreurs, il faut donc les mettre en place
  * Les données persistées des reviews sont pour l'instant le minima, il faudrait prévoir de persister plus d'info de chaque review (autheur, contenu, acheteur vérifié, nombre de vote utiles ...)
