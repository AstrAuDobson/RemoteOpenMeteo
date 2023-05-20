// --------------------------------------------------------------------------------
// ASCOM ObservingConditions driver for RemoteOpenMeteo
//
// Description:	Remote Open Meteo
//              Station météo DIY
//
// Implements:	ASCOM ObservingConditions interface version: 6.6
// Author:		Le Zav' et Juanito del Pepito
//
// Date			Who	        Vers	    Description
// -----------	---	        -----	    -------------------------------------------------------
// 16/05/2023	JUANITO	    6.6.0.1	    Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------------------
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.RemoteOpenMeteo.Properties;
using ASCOM.Utilities;

namespace ASCOM.RemoteOpenMeteo
{
    /// <summary>
    /// ASCOM ObservingConditions Driver for RemoteOpenMeteo.
    /// </summary>
    [Guid("3ce92a41-5991-4624-8554-a2dcb7cfe94a")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : IObservingConditions
    {
        #region Champs statiques

        /// <summary>
        /// ASCOM DeviceID (COM ProgID) du driver.
        /// The DeviceID est utilisé par l'application ASCOM pour accéder au driver.
        /// </summary>
        internal static string driverID = "ASCOM.RemoteOpenMeteo.ObservingConditions";

        /// <summary>
        /// Driver description affiché dans le ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "Remote Open Meteo";

        /// <summary>
        /// Constante pour utilisation interne
        /// </summary>
        internal static string comPortProfileName = "COM Port";

        /// <summary>
        /// COM Port par défaut
        /// </summary>
        internal static string comPortDefault = string.Empty;

        /// <summary>
        /// Nom de profil pour les traces
        /// </summary>
        internal static string traceStateProfileName = "Trace Level";

        /// <summary>
        /// Trace ON/OFF par défaut
        /// </summary>
        internal static string traceStateDefault = "false";

        /// <summary>
        /// Port série défini dans les Settings
        /// </summary>
        internal static string comPort;

        #endregion

        #region Champs

        /// <summary>
        /// Etat de la connexion au module ROM
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Port série de connection du module ROM
        /// </summary>
        private Serial serialPort;

        /// <summary>
        /// Objet permettant d'accéder au modèle objet ASCOM.Utilities.Util
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Objet permettant d'accéder au modèle objet ASCOM.Astrometry.AstroUtils
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Objet permettant d'accéder au modèle objet ASCOM.Utilities.TraceLogger
        /// </summary>
        internal TraceLogger tl;

        /// <summary>
        /// Dictionnaire permettant le stockage des informations de vitesse de vent par intervalle de temps
        /// </summary>
        private Dictionary<DateTime, double> winds = new Dictionary<DateTime, double>();

        /// <summary>
        /// Vitesse des rafales de vent
        /// </summary>
        private double gustStrength;

        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur
        /// <para>Initialise une nouvelle instance de la classe <see cref="ObservingConditions"/></para>
        /// </summary>
        public ObservingConditions()
        {
            // Initialisation des objets internes
            tl = new TraceLogger("", "RemoteOpenMeteo");
            utilities = new Util();
            astroUtilities = new AstroUtils();
            serialPort = null;

            // Initialisation des Flags
            connectedState = false;

            // Lecture de la configuration du périphérique ROM depuis la base de registre (ASCOM)
            ReadProfile();

            // Trace
            tl.LogMessage("ObservingConditions", "Initialisation effectuée avec succès");
        }

        #endregion

        #region Méthodes publiques

        /// <summary>
        /// Ouvre la boîte de dialogue des paramètres du ROM
        /// <para>Sur OK, les paramètres sont sauvegardés en base de registre (ASCOM)</para>
        /// </summary>
        public void SetupDialog()
        {
            // Pas d'ouverture de la boîte de dialogue si la connexion est déjà établie
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show(Resources.ModuleROMDejaConnecte
                                                        + Environment.NewLine
                                                        + Resources.VeuillezDeconnecterLeModuleROMPourAccederAuxParametres,
                                                    "Remote Open Meteo",
                                                    System.Windows.Forms.MessageBoxButtons.OK,
                                                    System.Windows.Forms.MessageBoxIcon.Information);
            else
            {
                // Déclaration et ouverture boîte de dialogue
                using (SetupDialogForm F = new SetupDialogForm(tl))
                {
                    if (F.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        // Ecriture dans la registry
                        WriteProfile();
                    }
                }
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the device and return immediately without waiting for a response

            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            // string retString = CommandString(command, raw); // Send the command and wait for the response
            // bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
            // return retBool; // Return the boolean value to the client

            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        /// <summary>
        /// Dispose the late-bound interface, if needed. Will release it via COM
        /// if it is a COM object, else if native .NET will just dereference it
        /// for GC.
        /// </summary>
        public void Dispose()
        {
            // Fermeture de la connexion
            CloseConnection();
            // Clean up des objets internes
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        #endregion

        #region Propriétés : Informations Driver

        /// <summary>
        /// Description du Driver
        /// </summary>
        public string Description
        {
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        /// <summary>
        /// Informations sur le Driver
        /// </summary>
        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverInfo = Resources.DriverASCOMPourModuleROM;
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// Version du Driver
        /// </summary>
        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = $"Version: {version.Major}.{version.Minor}.{version.MajorRevision}.{version.MinorRevision}";
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// Numéro de version de l'interface
        /// </summary>
        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "1");
                return Convert.ToInt16("1");
            }
        }

        /// <summary>
        /// Nom court
        /// </summary>
        public string Name
        {
            get
            {
                string name = "Remote Open Meteo";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        /// <summary>
        /// Renvoi la liste des Custom Actions supportées par le Driver
        /// </summary>
        /// <value><see cref="ArrayList"/> de string</value>
        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Pas de custom actions supportées. Renvoi d'une liste vide");
                return new ArrayList();
            }
        }

        #endregion

        #region Propriétés et Champs liés à la gestion de la connexion

        /// <summary>
        /// Etat de la connexion au port série
        /// </summary>
        public bool Connected
        {
            get
            {
                // Trace
                LogMessage("Connected", "Get {0}", IsConnected);

                // Renvoi la valeur du champ interne
                return IsConnected;
            }
            set
            {
                // Trace
                tl.LogMessage("Connected", "Set {0}", value);

                // On ne fait rien si l'état souhaité est l'état actuel
                if (value == IsConnected)
                    return;

                // Connection au port série
                if (value)
                {
                    StartConnection();
                    LogMessage("Connected Set", "Connexion au port {0}", comPort);
                }
                // Déconnexion du port série
                else
                {
                    CloseConnection();
                    LogMessage("Connected Set", "Déconnexion du port {0}", comPort);
                }
            }
        }

        /// <summary>
        /// Renvoi la valeur du flag interne <see cref="connectedState"/>
        /// </summary>
        private bool IsConnected
        {
            get
            {
                return connectedState;
            }
        }

        /// <summary>
        /// Vérifie l'état de la connexion au module ROM
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            Stopwatch sw = Stopwatch.StartNew();

            // Vérification sur valeur du flag interne
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }

            // Envoi d'une commande IA pour validation de la présence d'un module ROM
            try
            {
                serialPort.Transmit($"[IA]\n");
                string retourIA = serialPort.ReceiveTerminated("\n");
#if DEBUG
                Debug.WriteLine($"CheckConnected [IA] : {retourIA} en {sw.ElapsedMilliseconds} ms");
#endif
                if (string.IsNullOrEmpty(retourIA) || !retourIA.ToUpper().StartsWith("[IA]ROM V1"))
                    throw new NotConnectedException(Resources.PasDeModuleROMDetecteSurLePortSerie);
            }
            catch(Exception ex)
            {
                LogMessage("CheckConnected", "Erreur : {0}", ex.Message);

                // Mise à jour du flag interne et throw de l'exception
                CloseConnection();
                throw new NotConnectedException(ex);
            }
        }

        /// <summary>
        /// Lance la connexion au port série défini dans les Settings du périphérique ROM
        /// <para>Après la connexion au port série, envoi des commandes [IA] et [SET]DEBUG:OFF</para>
        /// </summary>
        /// <exception cref="DriverException"></exception>
        private void StartConnection()
        {
            try
            {
                // Vérif validité du settings
                if (string.IsNullOrEmpty(comPort))
                    throw new NotConnectedException(Resources.PasDePortSerieDefiniDansLesParametres);

                // Initialisation port COM
                serialPort = new Serial();
                serialPort.PortName = comPort;
                serialPort.ReceiveTimeout = 1;
                //serialPort.WriteTimeout = 1000;
                serialPort.Speed = SerialSpeed.ps57600;
                serialPort.DTREnable = false;
                serialPort.Handshake = SerialHandshake.None;
                serialPort.RTSEnable = false;
                serialPort.Parity = SerialParity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = SerialStopBits.One;
                serialPort.Connected = true;

                // On attend 2s d'intialisation du module ROM + 1s de marge
                Thread.Sleep(3000);

                // Vérif présence d'un module ROM
                // DEBUG OFF
                serialPort.Transmit($"[SET]DEBUG:OFF\n");
                serialPort.ClearBuffers();
                Thread.Sleep(100);

                // IA
                serialPort.Transmit($"[IA]\n");
                string retourIA = serialPort.ReceiveTerminated("\n");
                if (string.IsNullOrEmpty(retourIA) || !retourIA.ToUpper().StartsWith("[IA]ROM V1"))
                    throw new NotConnectedException(Resources.PasDeModuleROMDetecteSurLePortSerie);

                // Mise à jour du flag interne
                connectedState = true;
            }
            catch (Exception ex)
            {
                LogMessage("StartConnection", "Erreur : {0}", ex.Message);

                // Mise à jour du flag interne et throw de l'exception
                CloseConnection();
                throw new DriverException(ex.Message);
            }
        }

        /// <summary>
        /// Fermeture de la connexion au port série
        /// </summary>
        /// <exception cref="DriverException"></exception>
        private void CloseConnection()
        {
            try
            {
                if (serialPort != null)
                {
                    // Dispose et Close du port
                    serialPort.Connected = false;
                    serialPort.Dispose();
                    serialPort = null;
                }

                // Mise à jour du flag interne
                connectedState = false;
            }
            catch (Exception ex)
            {
                LogMessage("CloseConnection", "Erreur : {0}", ex.Message);

                // Mise à jour du flag interne et throw de l'exception
                connectedState = false;
                if (serialPort != null)
                    serialPort.Dispose();
                serialPort = null;
            }
        }

        /// <summary>
        /// Envoi une commande et lis la réponse avec TryParse du résultat
        /// <para>Renvoi une valeur de type double nullable</para>
        /// <para>Pour utiliser la valeur de retour, passez par monDouble.HasValue et mon.Double.Value</para>
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>double ou null</returns>
        private double? EnvoyerCommande(string commande)
        {
            try
            {
                // Vérif validité port série
                if (serialPort == null || !IsConnected)
                {
                    throw new NotConnectedException(Resources.PasDeModuleROMDetecteSurLePortSerie);
                }

                // Flush IN/OUT Buffer
                serialPort.ClearBuffers();

                // Envoi de la commande
                LogMessage($"{comPort} : SEND [{commande}]", GetType().Name);
                serialPort.Transmit($"{commande}\n");

                // Exemple de retour : [GET]T:21.26 / [GET]P:982.54
                string retour = serialPort.ReceiveTerminated("\n");
                // Validation présence d'un retour et Sécurité sur la validité de la réponse
                if (!string.IsNullOrEmpty(retour) && retour.ToUpper().StartsWith("[GET]") && retour.Length > 7)
                {
                    // On tryParse la valeur de retour
                    if (double.TryParse(retour.Substring(7),
                        NumberStyles.Number | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign,
                        CultureInfo.InvariantCulture,
                        out double result))
                        return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                LogMessage("EnvoyerCommande", "Erreur : {0}", ex.Message);

                // Mise à jour du flag interne et throw de l'exception
                CloseConnection();
                throw new DriverException($"{Resources.ErreurLorsDeLEnvoiDeLaCommande} : {ex.Message}", ex);
            }
        }

        #endregion

        #region IObservingConditions Implementation

        /// <summary>
        /// Gets and sets the time period over which observations will be averaged
        /// </summary>
        public double AveragePeriod
        {
            get
            {
                LogMessage("AveragePeriod", "get - 0");
                return 0;
            }
            set
            {
                LogMessage("AveragePeriod", "set - {0}", value);
                if (value != 0)
                    throw new InvalidValueException("AveragePeriod", value.ToString(), "0 only");
            }
        }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        public double CloudCover
        {
            get
            {
                LogMessage("CloudCover", "get - not implemented");
                throw new PropertyNotImplementedException("CloudCover", false);
            }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory in deg C
        /// </summary>
        public double DewPoint
        {
            get
            {
                LogMessage("DewPoint", "get - not implemented");
                throw new PropertyNotImplementedException("DewPoint", false);
            }
        }

        /// <summary>
        /// Atmospheric relative humidity at the observatory in percent
        /// </summary>
        public double Humidity
        {
            get
            {
                try
                {
                    Stopwatch sw = Stopwatch.StartNew();

                    // Vérif de connexion et de présence d'un module ROM
                    CheckConnected("Humidité");

                    // Envoi de la commande
                    double? result = EnvoyerCommande("[GET]H");
#if DEBUG
                    string resultLine = result.HasValue ? result.Value.ToString() : "NaN";
                    Debug.WriteLine($"Humidité : {resultLine} en {sw.ElapsedMilliseconds} ms");
#endif
                    if (result.HasValue)
                        return result.Value;

                    // Pas de réponse valide
                    throw new InvalidValueException();
                }
                catch (InvalidValueException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogMessage("Humidité", "Erreur : {0}", ex.Message);

                    // Mise à jour du flag interne et throw de l'exception
                    CloseConnection();
                    throw new DriverException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory in hectoPascals (hPa)
        /// </summary>
        public double Pressure
        {
            get
            {
                try
                {
                    Stopwatch sw = Stopwatch.StartNew();

                    // Vérif de connexion et de présence d'un module ROM
                    CheckConnected("Pression atmosphérique");

                    // Envoi de la commande
                    double? result = EnvoyerCommande("[GET]P");
#if DEBUG
                    string resultLine = result.HasValue ? result.Value.ToString() : "NaN";
                    Debug.WriteLine($"Pression atmosphérique : {resultLine} en {sw.ElapsedMilliseconds} ms");
#endif
                    if (result.HasValue)
                        return result.Value;

                    // Pas de réponse valide
                    throw new InvalidValueException();
                }
                catch (InvalidValueException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogMessage("Pression atmosphérique", "Erreur : {0}", ex.Message);

                    // Mise à jour du flag interne et throw de l'exception
                    CloseConnection();
                    throw new DriverException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Rain rate at the observatory, in millimeters per hour
        /// </summary>
        public double RainRate
        {
            get
            {
                LogMessage("RainRate", "get - not implemented");
                throw new PropertyNotImplementedException("RainRate", false);
            }
        }

        /// <summary>
        /// Forces the driver to immediately query its attached hardware to refresh sensor
        /// values
        /// </summary>
        public void Refresh()
        {
            throw new MethodNotImplementedException();
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="propertyName">Name of the property whose sensor description is required</param>
        /// <returns>The sensor description string</returns>
        public string SensorDescription(string propertyName)
        {
            switch (propertyName.Trim().ToLowerInvariant())
            {
                case "averageperiod":
                    return "Average period in hours, immediate values are only available";
                case "humidity":
                    return "Taux d'humidité";
                case "pressure":
                    return "Pression atmosphérique";
                case "temperature":
                    return "Température ambiante";
                case "cloudcover":
                case "dewpoint":
                case "rainrate":
                case "skybrightness":
                case "skyquality":
                case "skytemperature":
                case "starfwhm":
                case "winddirection":
                case "windgust":
                case "windspeed":
                    // Throw an exception on the properties that are not implemented
                    LogMessage("SensorDescription", $"Property {propertyName} is not implemented");
                    throw new MethodNotImplementedException($"SensorDescription - Property {propertyName} is not implemented");
                default:
                    LogMessage("SensorDescription", $"Invalid sensor name: {propertyName}");
                    throw new InvalidValueException($"SensorDescription - Invalid property name: {propertyName}");
            }
        }

        /// <summary>
        /// Sky brightness at the observatory, in Lux (lumens per square meter)
        /// </summary>
        public double SkyBrightness
        {
            get
            {
                LogMessage("SkyBrightness", "get - not implemented");
                throw new PropertyNotImplementedException("SkyBrightness", false);
            }
        }

        /// <summary>
        /// Sky quality at the observatory, in magnitudes per square arc-second
        /// </summary>
        public double SkyQuality
        {
            get
            {
                LogMessage("SkyQuality", "get - not implemented");
                throw new PropertyNotImplementedException("SkyQuality", false);
            }
        }

        /// <summary>
        /// Seeing at the observatory, measured as the average star full width half maximum (FWHM in arc secs) 
        /// within a star field
        /// </summary>
        public double StarFWHM
        {
            get
            {
                LogMessage("StarFWHM", "get - not implemented");
                throw new PropertyNotImplementedException("StarFWHM", false);
            }
        }

        /// <summary>
        /// Sky temperature at the observatory in deg C
        /// </summary>
        public double SkyTemperature
        {
            get
            {
                LogMessage("SkyTemperature", "get - not implemented");
                throw new PropertyNotImplementedException("SkyTemperature", false);
            }
        }

        /// <summary>
        /// Temperature at the observatory in deg C
        /// </summary>
        public double Temperature
        {
            get
            {
                try
                {
                    Stopwatch sw = Stopwatch.StartNew();

                    // Vérif de connexion et de présence d'un module ROM
                    CheckConnected("Temperature");

                    // Envoi de la commande
                    double? result = EnvoyerCommande("[GET]T");
#if DEBUG
                    string resultLine = result.HasValue ? result.Value.ToString() : "NaN";
                    Debug.WriteLine($"Temperature : {resultLine} en {sw.ElapsedMilliseconds} ms");
#endif
                    if (result.HasValue)
                        return result.Value;

                    // Pas de réponse valide
                    throw new InvalidValueException();
                }
                catch(InvalidValueException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    LogMessage("Temperature", "Erreur : {0}", ex.Message);

                    // Mise à jour du flag interne et throw de l'exception
                    CloseConnection();
                    throw new DriverException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="propertyName">Name of the property whose time since last update Is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        public double TimeSinceLastUpdate(string propertyName)
        {
            // Test for an empty property name, if found, return the time since the most recent update to any sensor
            if (!string.IsNullOrEmpty(propertyName))
            {
                switch (propertyName.Trim().ToLowerInvariant())
                {
                    // Return the time for properties that are implemented, otherwise fall through to the MethodNotImplementedException
                    case "averageperiod":
                    case "cloudcover":
                    case "dewpoint":
                    case "humidity":
                    case "pressure":
                    case "rainrate":
                    case "skybrightness":
                    case "skyquality":
                    case "skytemperature":
                    case "starfwhm":
                    case "temperature":
                    case "winddirection":
                    case "windgust":
                    case "windspeed":
                        // Throw an exception on the properties that are not implemented
                        LogMessage("TimeSinceLastUpdate", $"Property {propertyName} is not implemented");
                        throw new MethodNotImplementedException($"TimeSinceLastUpdate - Property {propertyName} is not implemented");
                    default:
                        LogMessage("TimeSinceLastUpdate", $"Invalid sensor name: {propertyName}");
                        throw new InvalidValueException($"TimeSinceLastUpdate - Invalid property name: {propertyName}");
                }
            }

            // Return the time since the most recent update to any sensor
            LogMessage("TimeSinceLastUpdate", $"The time since the most recent sensor update is not implemented");
            throw new MethodNotImplementedException("TimeSinceLastUpdate(" + propertyName + ")");
        }

        /// <summary>
        /// Wind direction at the observatory in degrees
        /// </summary>
        public double WindDirection
        {
            get
            {
                LogMessage("WindDirection", "get - not implemented");
                throw new PropertyNotImplementedException("WindDirection", false);
            }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
        /// </summary>
        public double WindGust
        {
            get
            {
                LogMessage("WindGust", "get - not implemented");
                throw new PropertyNotImplementedException("WindGust", false);
            }
        }

        /// <summary>
        /// Wind speed at the observatory in m/s
        /// </summary>
        public double WindSpeed
        {
            get
            {
                LogMessage("WindSpeed", "get - not implemented");
                throw new PropertyNotImplementedException("WindSpeed", false);
            }
        }

        #endregion

        #region Méthodes privées utilitaires

        /// <summary>
        /// Mise à jour des rafales comme le plus gros vent mesuré sur les dernières 2 minutes
        /// </summary>
        /// <param name="speed"></param>
        private void UpdateGusts(double speed)
        {
            Dictionary<DateTime, double> newWinds = new Dictionary<DateTime, double>();
            var last = DateTime.Now - TimeSpan.FromMinutes(2);
            winds.Add(DateTime.Now, speed);
            var gust = 0.0;
            foreach (var item in winds)
            {
                if (item.Key > last)
                {
                    newWinds.Add(item.Key, item.Value);
                    if (item.Value > gust)
                        gust = item.Value;
                }
            }
            gustStrength = gust;
            winds = newWinds;
        }

        /// <summary>
        /// Lis la configuration du périphérique dans la base de registre (ASCOM)
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Ecris la configuration du périphérique dans la base de registre (ASCOM)
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                if (!(comPort is null)) driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
            }
        }

        /// <summary>
        /// Trace via le TraceLogger
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }

        #endregion

        #region Méthode permettant le register du driver ASCOM dans le système

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "ObservingConditions";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion
    }
}
