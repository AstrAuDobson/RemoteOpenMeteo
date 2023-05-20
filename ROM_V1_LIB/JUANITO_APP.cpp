#include "JUANITO_APP.h"

/// <summary>
/// Constructeur
/// </summary>
JUANITO_APP::JUANITO_APP()
{
  // Désactivation par défaut du flag interne
  _modeDebug = false;
}

/// <summary>
/// Positionne le mode Debug
/// </summary>
void JUANITO_APP::SetModeDebug(bool modeDebug)
{
    _modeDebug = modeDebug;
}

/// <summary>
/// Trace en console les performances de l'application
/// </summary>
void JUANITO_APP::TraceAppPerformances()
{
    if (_modeDebug)
    {
        // TimeSpan actuel
        long currentDif = millis() - _lastLoopTime;

        // Temps min
        if (_minLoopTime == 0
            || (currentDif != 0 && currentDif < _minLoopTime))
        {
          _minLoopTime = currentDif;
          if (_minLoopTime != 0)
          {
            if (_modeDebug)
            {
              Serial.print(F("[PERF] LoopTime : min = "));
              Serial.print(String(_minLoopTime));
              Serial.println(F(" ms"));
            }
          }
        }

        // Temps MAX
        if (_maxLoopTime == 0 || currentDif > _maxLoopTime)
        {
          _maxLoopTime = currentDif;
          if (_maxLoopTime != 0)
          {
            if (_modeDebug)
            {
              Serial.print(F("[PERF] LoopTime : MAX = "));
              Serial.print(String(_maxLoopTime));
              Serial.println(F(" ms"));
            }
          }
        }

        // Actualisation moyenne
        _avgLoopTime = _avgLoopTime + ((currentDif - _avgLoopTime) / (_nbTotalLoops + 1));
        // On trace la moyenne tous les XX Loop
        if (_nbTotalLoops % 100000 == 0)
        {
          if (_modeDebug)
          {
            Serial.print(F("[PERF] SRAM restant: "));
            Serial.print(GetFreeRam());
            Serial.println(F(" octets"));

            Serial.print(F("[PERF] LoopTime : AVG = "));
            Serial.print(String(_avgLoopTime, 5));
            Serial.print(F(" ms / Nb. LOOPS = "));
            Serial.println(String(_nbTotalLoops));
          }
        }
    }

    // Actualisation du chrono et flag
    _lastLoopTime = millis();
    _nbTotalLoops++;
}

/// <summary>
/// Renvoi la quantité de mémoire disponbile restante
/// </summary>
/// <returns></returns>
int JUANITO_APP::GetFreeRam() {
#ifdef __AVR__
    extern int __heap_start, * __brkval;
    int v;
    return (int)&v - (__brkval == 0
        ? (int)&__heap_start : (int)__brkval);
#else
    return ESP.getFreeHeap();
#endif
}

/// <summary>
/// Réinitialisation des compteurs et chronos de performances de l'application
/// </summary>
void JUANITO_APP::InitPerformances()
{
    // Actualisation du chrono et flag
    _lastLoopTime = millis();
    _nbTotalLoops = 0;
}
