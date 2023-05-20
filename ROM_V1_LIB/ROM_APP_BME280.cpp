#include "ROM_APP_BME280.h"

/// <summary>
/// Constructeur
/// </summary>
ROM_APP_BME280::ROM_APP_BME280()
{
}

/// <summary>
/// Initialisation de l'application
/// </summary>
void ROM_APP_BME280::Init(int oledI2cAddress, int bme280I2cAddress)
{
  // Valorisation des champs internes
  _oledI2cAddress = oledI2cAddress;
  _bme280I2cAddress = bme280I2cAddress;

  // Initialisation BME
  _bme280.begin(_bme280I2cAddress, &Wire);

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
void ROM_APP_BME280::ReadEnvironment()
{
  if (millis() > _chronoMesureEnv + ENV_MEASURE_INTERVAL)
  {
    // Lecture BME280
    _temperature = _bme280.readTemperature();
    _pression = _bme280.readPressure() / 100;
    _humidity = _bme280.readHumidity();

    if (_modeDebug)
    {
      Serial.print(F("[ENV] T:"));
      Serial.print(String(_temperature, 2));
      Serial.print(F(" °C / P:"));
      Serial.print(String(_pression, 2));
      Serial.print(F(" hPa / H:"));
      Serial.print(String(_humidity, 2));
      Serial.println(F(" %"));
    }

    // Actualisation du chrono
    _chronoMesureEnv = millis();
  }
}
