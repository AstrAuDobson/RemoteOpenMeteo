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
///                   *** Mode SENSOR_BME280 :
///                     - "Adafruit_BME280_Library"
///                   *** Mode SENSOR_BMP280_DHT :
///                     - "Adafruit_BMP280_Library"
///                     - "DHT_Sensor_Library"
/// Objet           : ROM_APP : Objet applicatif de l'application ROM
///
/// ---------------------------------------------------------------------
// Librairies
#include "JUANITO_APP.h"
#include <Adafruit_SSD1306.h>
#include "DSEG7_ClassicMiniBold_24.h"
#include "Roboto.h"

// Déclaration des constantes propres à l'application
#define ENV_MEASURE_INTERVAL              2000      // Intervalle de temps entre 2 mesures des conditions d'environnement
#define DISPLAY_TIME_BEFORE_STANDBY       30000     // Timeout avant extinction automatique de l'écran OLED

// Déclaration des constantes correspondant à l'affichage (OLED)
#define OLED_SCREEN_WIDTH                 128       // Largeur de l'écran
#define OLED_SCREEN_HEIGHT                64        // Hauteur de l'écran
#define OLED_RESET                        -1        // Resset de l'écran (-1: default value)
#define DISPLAY_REFRESH_SCREEN            50        // Taux de rafraichissement de l'écran

/// ---------------------
/// ROM_APP : Objet applicatif de l'application ROM
///
class ROM_APP : public JUANITO_APP
{
public:
  ROM_APP();
  void ChangeDisplayState();
  virtual void ReadEnvironment();
  void DisplayMesure();
  void DoCommand(String command);

protected:
  void DisplaySplashApp();
  void UpdateDewPoint();

  // Initialisation des objets internes
  Adafruit_SSD1306 _display = Adafruit_SSD1306(OLED_SCREEN_WIDTH, OLED_SCREEN_HEIGHT, &Wire, OLED_RESET);
	
  // Champs internes
  float _pression = NAN;                    // Pression atmosphérique mesurée sur le BME
  float _temperature = NAN;                 // Température mesurée sur le BME
  float _humidity = NAN;                    // Humidité mesurée sur le BME
  float	_dewPoint = NAN;					// Point de rosée calculée à partir de la T° et du taux d'Humidité
  bool  _displayOn = true;                  // Affichage en mode Standby : On / Off
  int	_oledI2cAddress;					// Adresse de l'écran OLED sur le bus I2C

  // Variables pour chronos internes
  long  _chronoMesureEnv;                   // Chrono sur mesure des variables d'environnement
  long  _chronoDisplayRefresh;              // Chrono sur le taux de rafraichissement de l'affichage
  long  _chronoDisplayGlobal;               // Chrono sur le timeout sur l'affichage pour mesure des 30s
};
