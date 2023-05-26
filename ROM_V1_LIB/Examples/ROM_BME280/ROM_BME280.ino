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
///
/// ---------------------------------------------------------------------
// Librairies
#include <ROM_APP_BME280.h>

// Déclaration des constantes correspondant aux PIN et aux adresses sur le bus I2C
#define PIN_PUSH_ACTION                   14        // GPIO14 = D5
#define OLED_SCREEN_I2C_ADDRESS           0x3C      // Ecran OLED
#define BME280_I2C_ADDRESS                0x76      // Sensor T/P/H BME280

// Déclaration des constantes correspondant à l'interruption sur bouton d'action
#define PUSH_ACTION_DEBOUNCE_INTERVAL     1000      // Durée du Debounce logiciel sur interruption : 
                                                    // Mon bouton PullUp est de qualité "MEDIOCRE" ... pour pas dire pourrie, et envoi des interruptions multiples !!
                                                    // => Ajout d'un Debounce logiciel de 1s => Et hop, un bouton tout neuf qui fonctionne Nickel-Chrome :)

// Instanciation des objets internes
ROM_APP_BME280 app;

// Déclaration des variables servant à la gestion de l'interruption
volatile bool actionStateChanged = false;           // Flag de changement d'état sur interruption
volatile long chronoDebounceInterrupt;              // Chrono servant au Debounce sur interruption

// Déclaration des variables servant à la lecture sur Port Série
String inputString = "";                            // Commande complète
bool stringComplete = false;                        // Flag sur fin de commande
char serialInChar;                                  // Variable pour lecture du port série caractère par caractère

/// -------------------------------------------
/// Setup
///
void setup()
{ 
  // Initialisation SerialPort
  Serial.begin(57600);
  while(!Serial);
  Serial.println("Program started");

  // Positionnement des PIN et de l'interruption sur le bouton d'action
  pinMode(PIN_PUSH_ACTION, INPUT_PULLUP);
  attachInterrupt(digitalPinToInterrupt(PIN_PUSH_ACTION), CheckActionState, FALLING);

  // Initialisation de l'application
  app.Init(OLED_SCREEN_I2C_ADDRESS, BME280_I2C_ADDRESS);

  // Mode DEBUG. Commentez cette ligne à la fin du développement
  app.SetModeDebug(true);
}

/// --------------------------------------------
/// Loop
///
void loop()
{
  // Trace des perf de l'appli
  app.TraceAppPerformances();

  // Sur interruption sur bouton d'action, on change l'état d'affichage en cours (avec réinit du chrono de 30s)
  if (actionStateChanged)
  {
    app.ChangeDisplayState();
    actionStateChanged = false;
  }

  // Lecture des conditions d'environnement
  app.ReadEnvironment();

  // Affichage des conditions d'environnement
  app.DisplayMesure();

  // Commande complète arrivée sur Port Série (USB). On traite la commande
  if (stringComplete)
  {
    app.DoCommand(inputString);
    // Réinit des flags
    inputString = "";
    stringComplete = false;
  }
}

/// -------------------------------------------------
/// Lecture caractère par caractère sur port série
///
void serialEvent()
{
  // Vérif si données disponible
  if (Serial.available())
  {
    // Lecture d'1 Byte (8 bits :p)
    Serial.readBytes(&serialInChar, 1);
    // Vérif si caractère de fin de commande
    if (serialInChar == '\n')
      stringComplete = true;
    else
      inputString += serialInChar;
  }
}

/// -------------------------------------------------
/// Traitement sur interruption sur bouton d'action
///
ICACHE_RAM_ATTR void CheckActionState()
{
  // Vérif Debounce logiciel
  if (millis() > chronoDebounceInterrupt + PUSH_ACTION_DEBOUNCE_INTERVAL)
  {
    // Actualisation Flag et Chrono
    actionStateChanged = true;
    chronoDebounceInterrupt = millis();
  }
}