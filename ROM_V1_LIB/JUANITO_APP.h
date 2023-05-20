/// ---------------------------------------------------------------------
///
/// Auteur          : Juanito del Pepito
/// Version         : 1.0.1.2
/// Date            : 15/04/2023
/// Description     : Classe mère des applications utilisant la JUANITO_LIB
/// Prérequis       : Si #define DEBUG => Hardware Serial doit être activé
/// Objet           : JUANITO_APP : Classe mère des Objets applicatif
///
/// ---------------------------------------------------------------------
// Librairies
#include "Arduino.h"

/// ---------------------
/// JUANITO_APP : Classe mère des Objets applicatif
///
class JUANITO_APP
{
public:
  JUANITO_APP();
  void TraceAppPerformances();
  void SetModeDebug(bool modeDebug);

protected:
  void InitPerformances();

  // Champs internes
  bool  _modeDebug;                                         // Gestion interne du mode Debug (Possibilité d'envoyer DEBUG ON/OFF via commande sur Port Série)

private:
  int GetFreeRam();

  // Déclaration des variables nécessaire aux mesures des temps de Loop
  long  _lastLoopTime = 0;                                  // DEBUG : Flag de lecture de l'interval mini
  long  _minLoopTime = 0;                                   // DEBUG : Interval mini de la fonction loop
  long  _maxLoopTime = 0;                                   // DEBUG : Interval mini de la fonction loop
  float _avgLoopTime = 0;                                   // DEBUG : Moyenne du temps d'un Interval
  long  _nbTotalLoops = 0;                                  // DEBUG : Nombre total d'intervalles
};