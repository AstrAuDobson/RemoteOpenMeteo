#include "ROM_APP.h"

/// <summary>
/// Constructeur
/// </summary>
ROM_APP::ROM_APP()
{
}

/// <summary>
/// Affichage du démarrage de l'application
/// </summary>
void ROM_APP::DisplaySplashApp()
{
  _display.clearDisplay();
  _display.setTextColor(SSD1306_WHITE);
  _display.setCursor(30, 5);
  _display.setTextSize(4);
  _display.print(F("ROM"));
  _display.setTextSize(1);
  _display.setCursor(55, 45);
  _display.print(F("BY"));
  _display.setCursor(10, 55);
  _display.print(F("LE ZAV' & JUANITO"));
  _display.display();
  delay(2000);
  _display.clearDisplay();
  _display.display();
}

/// <summary>
/// Mise à jour de l'état de l'affichage
/// </summary>
void ROM_APP::ChangeDisplayState()
{
  // Inversion de l'affichage et update chrono
  _displayOn = !_displayOn;
  _chronoDisplayGlobal = millis();
}

/// <summary>
/// Lecture des conditions d'environnement
/// </summary>
void ROM_APP::ReadEnvironment()
{
  if (millis() > _chronoMesureEnv + ENV_MEASURE_INTERVAL)
  {
    _temperature = NAN;
    _pression = NAN;
    _humidity = NAN;

    // Actualisation du chrono
    _chronoMesureEnv = millis();
  }
}

/// <summary>
/// Mise à jour du point de rosée à partir de T° et de H
/// </summary>
void ROM_APP::UpdateDewPoint()
{
  double a = 17.271;
  double b = 237.7;
  if (!isnan(_temperature) && !isnan(_humidity))
  {
    double temp = (a * _temperature) / (b + _temperature) + log(_humidity * 0.01);
    _dewPoint = (b * temp) / (a - temp);
  }
  else
    _dewPoint = NAN;
}

/// <summary>
/// Affichage des conditions d'environnement
/// </summary>
void ROM_APP::DisplayMesure()
{
  if (millis() > _chronoDisplayRefresh + DISPLAY_REFRESH_SCREEN)
  {
    // Update display sur timeout de l'affichage (30s)
    if (millis() > _chronoDisplayGlobal + DISPLAY_TIME_BEFORE_STANDBY)
    {
      _displayOn = false;
    }

    _display.clearDisplay();
    // Gestion de l'affichage
    if (_displayOn)
    {
      // Cadre blanc
      _display.drawRect(0, 0, OLED_SCREEN_WIDTH, OLED_SCREEN_HEIGHT, SSD1306_WHITE);

      // Température
      if (!isnan(_temperature))
      {
        // Texte centré
        // On calcul la position X du texte (21px par carac, 2 décimales + 1px pour le '.' + 4px pour le ' ' + 9px pour le 'C')
        String temperatureText = String(_temperature, 2);
        int sizeText = (21 * (temperatureText.length() - 1)) + 1 + 4 + 9;
        int posX = (OLED_SCREEN_WIDTH - sizeText) / 2;
        // Affichage
        _display.setFont(&DSEG7_Classic_Mini_Bold_24);
        _display.setCursor(posX, 30);
        _display.print(temperatureText);
        _display.setFont(&Roboto_12);
        _display.print(F(" C"));
      }

      // Humidité
      if (!isnan(_humidity))
      {
        // Texte aligné droite + marge
        // On calcul la position X du texte (8px par carac, 2 décimales + 1px pour le '.' + 4px pour le ' ' + 10px pour le '%' + 23px de marge)
        String humidityText = String(_humidity, 2);
        int sizeText = (8 * (humidityText.length() - 1)) + 1 + 4 + 10 + 23;
        int posX = OLED_SCREEN_WIDTH - sizeText;
        // Affichage
        _display.setFont(&Roboto_12);
        _display.setCursor(posX, 48);
        _display.print(String(_humidity, 2));
        _display.setFont(&Roboto_12);
        _display.print(F(" %"));
      }

      // Pression
      if (!isnan(_pression))
      {
        // Texte aligné droite + marge
        // On calcul la position X du texte (8px par carac, 2 décimales + 1px pour le '.' + 4px pour le ' ' + 25px pour le 'hPa' + 8px de marge)
        String pressionText = String(_pression, 2);
        int sizeText = (8 * (pressionText.length() - 1)) + 1 + 4 + 25 + 8;
        int posX = OLED_SCREEN_WIDTH - sizeText;
        // Affichage
        _display.setFont(&Roboto_12);
        _display.setCursor(posX, 60);
        _display.print(String(_pression, 2));
        _display.setFont(&Roboto_12);
        _display.print(F(" hPa"));
      }
    }
    _display.display();

    // Actualisation du chrono
    _chronoDisplayRefresh = millis();
  }
}

/// <summary>
/// Traitement d'une commande arrivée sur Port série
/// </summary>
void ROM_APP::DoCommand(String command)
{
  String reponse;
  // Commande IA
  if (command.indexOf("[IA]") != -1)
  {
      reponse = "[IA]ROM V1";
      Serial.println(reponse);
  }
  // Commande SET DEBUG ON
  else if (command.indexOf("[SET]DEBUG:ON") != -1)
  {
    _modeDebug = true;
    reponse = "OK";
  }
  // Commande SET DEBUG OFF
  else if (command.indexOf("[SET]DEBUG:OFF") != -1)
  {
    _modeDebug = false;
    reponse = "OK";
  }
  // Demande de T°
  else if (command.indexOf("[GET]TA") != -1)
  {
    Serial.print(F("[GET]TA:"));
    Serial.println(String(_temperature, 2));
    reponse = "[GET]TA:" + String(_temperature, 2);
  }
  // Demande de Pression atmosphérique
  else if (command.indexOf("[GET]PA") != -1)
  {
    Serial.print(F("[GET]PA:"));
    Serial.println(String(_pression, 2));
    reponse = "[GET]PA:" + String(_pression, 2);
  }
  // Demande d'humidité
  else if (command.indexOf("[GET]TH") != -1)
  {
    Serial.print(F("[GET]TH:"));
    Serial.println(String(_humidity, 2));
    reponse = "[GET]TH:" + String(_humidity, 2);
  }
  // Demande du Point de rosée
  else if (command.indexOf("[GET]DP") != -1)
  {
      Serial.print(F("[GET]DP:"));
      Serial.println(String(_dewPoint, 2));
      reponse = "[GET]DP:" + String(_dewPoint, 2);
  }
  if (_modeDebug)
  {
    Serial.print(F("Commande reçue : "));
    Serial.println(command);
    Serial.print(F("Réponse : "));
    Serial.println(reponse);
  }
}
