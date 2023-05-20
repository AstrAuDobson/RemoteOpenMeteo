/// ---------------------------------------------------------------------
///
/// Projet          : ROM - Remote Open Météo
/// Auteur          : Le Zav' & Juanito del Pepito
/// Version         : BETA 0.4.2.1
/// Date            : 14/05/2023
/// Description     : Station météo Open source pour téléscope
///                   *** Mode SENSOR_BMP280_DHT :
///                   Capteur BMP280 pour :
///                     - la température
///                     - la pression atmosphérique
///                   Capteur DHT11 pour :
///                     - le taux d'humidité
/// Gitub           : https://github.com/AstrAuDobson/RemoteOpenMeteo
/// Prérequis       : Cette application nécessite l'ajout des bibliothèques suivantes
///                     - "Adafruit_SSD1306"
///                     - "Adafruit_GFX_Library"
///                     - "Adafruit_BMP280_Library"
///                     - "DHT_Sensor_Library"
/// Objet           : ROM_APP : Objet applicatif de l'application ROM
///
/// ---------------------------------------------------------------------
// Librairies
#include <ROM_APP.h>
#include <Adafruit_BMP280.h>
#include <DHT.h>

/// ---------------------
/// ROM_APP_BMP280_DHT : Objet applicatif de l'application ROM
/// Mode ROM_APP_BMP280_DHT
///
class ROM_APP_BMP280_DHT : public ROM_APP
{
public:
  ROM_APP_BMP280_DHT();
  void Init(int oledI2cAddress, int dhtPin, int dhtSensorType);
  void ReadEnvironment();

private:
  // Déclaration des objets internes
  Adafruit_BMP280 _bmp280;
  DHT * _dht;

  // Champs internes
  int _oledI2cAddress;
  int _dhtPin;
  int _dhtSensorType;
};
