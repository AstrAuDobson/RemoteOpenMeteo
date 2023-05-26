/// ---------------------------------------------------------------------
///
/// Projet          : ROM - Remote Open Météo
/// Auteur          : Le Zav' & Juanito del Pepito
/// Version         : BETA 0.6.0.2
/// Date            : 14/05/2023
/// Description     : Station météo Open source pour téléscope
///                   *** Mode SENSOR_BME280 :
///                   Capteur BME280 pour :
///                     - la température
///                     - le taux d'humidité
///                     - la pression atmosphérique
/// Gitub           : https://github.com/AstrAuDobson/RemoteOpenMeteo
/// Prérequis       : Cette application nécessite l'ajout des bibliothèques suivantes
///                     - "Adafruit_SSD1306"
///                     - "Adafruit_GFX_Library"
///                     - "Adafruit_BME280_Library"
/// Objet           : ROM_APP_BME280 : Objet applicatif de l'application ROM en mode SENSOR_BME280
///
/// ---------------------------------------------------------------------
// Librairies
#include <ROM_APP.h>
#include <Adafruit_BME280.h>

/// ---------------------
/// ROM_APP_BME280 : Objet applicatif de l'application ROM
/// Mode SENSOR_BME280
///
class ROM_APP_BME280 : public ROM_APP
{
public:
  ROM_APP_BME280();
  void Init(int oledI2cAddress, int bme280I2cAddress);
  void ReadEnvironment();

private:
  // Déclaration des objets internes
  Adafruit_BME280 _bme280;

  // Champs internes
  int _bme280I2cAddress;
};
