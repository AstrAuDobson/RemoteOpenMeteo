#include "ROM_APP_BMP280_DHT.h"

/// <summary>
/// Constructeur
/// </summary>
ROM_APP_BMP280_DHT::ROM_APP_BMP280_DHT()
{
}

/// <summary>
/// Initialisation de l'application
/// </summary>
void ROM_APP_BMP280_DHT::Init(int oledI2cAddress, int dhtPin, int dhtSensorType)
{
  // Valorisation des champs internes
  _oledI2cAddress = oledI2cAddress;
  _dhtPin = dhtPin;
  _dhtSensorType = dhtSensorType;

  // Initialisation BMP et DHT
  _bmp280.begin();
  _dht = new DHT(_dhtPin, _dhtSensorType);
  _dht->begin();

  // Initialisation Display
  if (!_display.begin(SSD1306_SWITCHCAPVCC, _oledI2cAddress))
  {
    if (_modeDebug)
      Serial.print(F("Une erreur est survenue lors de l'initialisation de l'écran ... Fin de l'application :(\n"));
    for (;;);
  }

  // Splash App
  DisplaySplashApp();

  // Réinititialisation des compteurs de performances
  InitPerformances();

  // Actualiation des chronos et flags internes
  _displayOn = true;
  _chronoDisplayGlobal = millis();
}

/// <summary>
/// Lecture des conditions d'environnement
/// </summary>
void ROM_APP_BMP280_DHT::ReadEnvironment()
{
  if (millis() > _chronoMesureEnv + ENV_MEASURE_INTERVAL)
  {
    // Lecture BME280
    _temperature = _bmp280.readTemperature();
    _pression = _bmp280.readPressure() / 100;
    _humidity = _dht->readHumidity();
    UpdateDewPoint();

    if (_modeDebug)
    {
      Serial.print(F("[ENV] T:"));
      Serial.print(String(_temperature, 2));
      Serial.print(F(" °C / P:"));
      Serial.print(String(_pression, 2));
      Serial.print(F(" hPa / H:"));
      Serial.print(String(_humidity, 2));
      Serial.print(F(" % / DP:"));
      Serial.println(String(_dewPoint, 2));
    }

    // Actualisation du chrono
    _chronoMesureEnv = millis();
  }
}
