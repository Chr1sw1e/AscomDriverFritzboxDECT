//tabs=4
// --------------------------------------------------------------------------------
// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Switch driver for Fritzbox DECT as Switch
//
// Description:	Fritzbox DECT Steckdosen als ASCOM - Switch Devices verwenden:    
//				Einstellung der Geräte im Setup 
//				Alle verfügbaren Geräte werden angezeigt und können über 
//				die Checkboxen ausgewählt werden. Ausgewählte werden im Profile
//				Abgespeichtert - Neue Auswahl -> Geräte neu einlesen.
//
// Implements:	ASCOM Switch interface version: 1.0
// Author:		(Chriswie) <Chriswie@a-city.de>
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 07-05-2022	ChW	6.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//

#define Switch

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;

namespace ASCOM.fb
{
    //
    // Your driver's DeviceID is ASCOM.fb.Switch
    //
    // The Guid attribute sets the CLSID for ASCOM.fb.Switch
    // The ClassInterface/None attribute prevents an empty interface called
    // _fb from being created and used as the [default] interface
    //


    /// <summary>
    /// ASCOM Switch Driver for fb.
    /// </summary>
    [Guid("9ba84f60-36f3-4b9e-9793-7fb43e67f9d5")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Switch : ISwitchV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.fb.Switch";
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "Fritzbox DECT als Switch";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM1";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "false";
        internal static string comPort; // Variables to hold the current device configuration

        #region Variables Stored in Profile
        /// <summary>
        /// Variable for Fritzbox IP adress stored in Profile- default "Fritz.box"
        /// </summary>
        internal static string IP_FRITZBOX;
        /// <summary>
        /// Variable for Fritzbox Username stored in Profile (Config on your Fritzbox)
        /// </summary>
        internal static string FB_User;
        /// <summary>
        /// Variable for Fritzbox Username stored in Profile (Config on your Fritzbox)
        /// </summary>
        internal static string FB_Password;
        /// <summary>
        /// Variable for Fritzbox AIN Device Number stored in Profile (Config on your Fritzbox)
        /// </summary>
        internal static string[] AINs = new String[16];
        /// <summary>
        /// Variable for Fritzbox Device Name stored in Profile (Config on your Fritzbox)
        /// Max 16 Devices
        /// </summary>
        internal static string[] AIN_name = new String[16];
        /// <summary>
        /// Variable for sum of selected Devices to use as selected by Setup Dialog
        /// Max 16 Devices
        /// </summary>
        internal static int AIN_anz;
        #endregion

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;
        /// <summary>
        /// Private variable to hold the status of switches (Less traffic)
        /// </summary>
        private bool[] aktiv = new bool[16];
        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;
        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;
        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal TraceLogger tl;

        #region Fritzbox Connection
        public void Set_Dect(short id, bool state)

        {
            // SessionID ermitteln
            string sid = GetSessionId(FB_User, FB_Password);
            string kom;

            if (state) kom = "setswitchon";
            else kom = "setswitchoff";
            
            string seite = SeiteEinlesen($@"http://{IP_FRITZBOX}/webservices/homeautoswitch.lua?ain={AINs[id]}&switchcmd={kom}", sid);
        }
        public void Get_Dect()

        {
            // SessionID ermitteln
            string sid = GetSessionId(FB_User, FB_Password);
            string kom;            

            kom = "getswitchstate";

            for (int i = 0; i < AIN_anz; i++)
            {
                string seite = SeiteEinlesen($@"http://{IP_FRITZBOX}/webservices/homeautoswitch.lua?ain={AINs[i]}&switchcmd={kom}", sid);

                if (seite.StartsWith("1"))
                {
                    aktiv[i] = true;
                }
                else 
                { 
                    aktiv[i] = false; 
                }
            }
        }
        public string SeiteEinlesen(string url, string sid)
        {
            Uri uri = new Uri(url + "&sid=" + sid);
            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadToEnd();

            return str;
        }
        public string GetSessionId(string benutzername, string kennwort)

        {
            XDocument doc = XDocument.Load($@"http://{IP_FRITZBOX}/login_sid.lua");

            string sid = GetValue(doc, "SID");
            if (sid == "0000000000000000")
            {
                string challenge = GetValue(doc, "Challenge");
                string uri = $@"http://{IP_FRITZBOX}/login_sid.lua?username=" +
                benutzername + @"&response=" + GetResponse(challenge, kennwort);
                doc = XDocument.Load(uri);
                sid = GetValue(doc, "SID");
            }
            return sid;
        }
        public string GetResponse(string challenge, string kennwort)

        {
            return challenge + "-" + GetMD5Hash(challenge + "-" + kennwort);
        }
        public string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data =
            md5Hasher.ComputeHash(Encoding.Unicode.GetBytes(input));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public string GetValue(XDocument doc, string name)
        {
            XElement info = doc.FirstNode as XElement;
            return info.Element(name).Value;
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="fb"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Switch()
        {
            tl = new TraceLogger("", "fb");
            ReadProfile(); // Read device configuration from the ASCOM Profile store
            tl.LogMessage("Switch", "Starting initialisation");
                                
            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object
            
            numSwitch = Convert.ToInt16(AIN_anz); // Get sum of Devices          

            if (numSwitch == 0) //Devices here ?
            {
                MessageBox.Show($"Keine Schalter vorhanden oder nicht konfiguriert\nBitte Konfiguration überprüfen", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tl.LogMessage("Switch", "No Switches");
            }
            else
            {
                Get_Dect();
            }
            tl.LogMessage("Switch", "Completed initialisation");
        }
        //
        // PUBLIC COM INTERFACE ISwitchV2 IMPLEMENTATION
        //
        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Verbunden.. bitte erst trennen");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

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

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    // TODO connect to the device
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from port {0}", comPort);
                    // TODO disconnect from the device
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "2");
                return Convert.ToInt16("2");
            }
        }

        public string Name
        {
            get
            {
                string name = "Fritzbox DECT switch";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ISwitchV2 Implementation

        private short numSwitch;
      


        /// <summary>
        /// The number of switches managed by this driver
        /// </summary>
        public short MaxSwitch
        {
            get
            {
                tl.LogMessage("MaxSwitch Get", numSwitch.ToString());
                return this.numSwitch;
            }
        }

        /// <summary>
        /// Return the name of switch n
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// The name of the switch
        /// </returns>
        public string GetSwitchName(short id)
        {               
            return $"Fritzdect:{AIN_name[id]}";                         
        }

        /// <summary>
        /// Sets a switch name to a specified value
        /// </summary>
        /// <param name="id">The number of the switch whose name is to be set</param>
        /// <param name="name">The name of the switch</param>
        public void SetSwitchName(short id, string name)
        {
            Validate("SetSwitchName", id);
            tl.LogMessage("SetSwitchName", $"SetSwitchName({id}) = {name} - not implemented");
            throw new MethodNotImplementedException("SetSwitchName");
        }

        /// <summary>
        /// Gets the description of the specified switch. This is to allow a fuller description of the switch to be returned, for example for a tool tip.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        /// <returns></returns>
        public string GetSwitchDescription(short id)
        {
            return $"Fritzbox DECT Steckdose {AIN_name[id]} mit AIN: {AINs[id]}";            
        }

        /// <summary>
        /// Reports whether the specified switch can be written to.
        /// Returns false if the switch cannot be written to, for example a limit switch or a sensor.
        /// </summary>
        /// <param name="id">The number of the switch whose write state is to be returned</param>
        /// <returns>
        /// <c>true</c> if the switch can be written to, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="MethodNotImplementedException">If the method is not implemented</exception>
        /// <exception cref="InvalidValueException">If id is outside the range 0 to MaxSwitch - 1</exception>
        public bool CanWrite(short id)
        {
            bool writable = true;
            Validate("CanWrite", id);
            // default behavour is to report true
            tl.LogMessage("CanWrite", $"CanWrite({id}): {writable}");
            return true;
        }

        #region Boolean switch members

        /// <summary>
        /// Return the state of switch n.
        /// A multi-value switch must throw a not implemented exception
        /// </summary>
        /// <param name="id">The switch number to return</param>
        /// <returns>
        /// True or false
        /// </returns>
        public bool GetSwitch(short id)
        {
            return this.aktiv[id];          
        }

        /// <summary>
        /// Sets a switch to the specified state
        /// If the switch cannot be set then throws a MethodNotImplementedException.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        public void SetSwitch(short id, bool state)
        {
            Validate("SetSwitch", id);
            this.aktiv[id]=state;
            Set_Dect(id,state);
        }

        #endregion

        #region Analogue members

        /// <summary>
        /// Returns the maximum value for this switch
        /// Boolean switches must return 1.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MaxSwitchValue(short id)
        {
            Validate("MaxSwitchValue", id);
            tl.LogMessage("MaxSwitchValue", $"MaxSwitchValue({id}) - not implemented");
            throw new MethodNotImplementedException("MaxSwitchValue");
        }

        /// <summary>
        /// Returns the minimum value for this switch
        /// Boolean switches must return 0.0
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double MinSwitchValue(short id)
        {
            Validate("MinSwitchValue", id);
            tl.LogMessage("MinSwitchValue", $"MinSwitchValue({id}) - not implemented");
            throw new MethodNotImplementedException("MinSwitchValue");
        }

        /// <summary>
        /// Returns the step size that this switch supports. This gives the difference between successive values of the switch.
        /// The number of values is ((MaxSwitchValue - MinSwitchValue) / SwitchStep) + 1
        /// boolean switches must return 1.0, giving two states.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double SwitchStep(short id)
        {
            Validate("SwitchStep", id);
            tl.LogMessage("SwitchStep", $"SwitchStep({id}) - not implemented");
            throw new MethodNotImplementedException("SwitchStep");
        }

        /// <summary>
        /// Returns the analogue switch value for switch id.
        /// Boolean switches must return either 0.0 (false) or 1.0 (true).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetSwitchValue(short id)
        {
            Validate("GetSwitchValue", id);
            tl.LogMessage("GetSwitchValue", $"GetSwitchValue({id}) - not implemented");
            throw new MethodNotImplementedException("GetSwitchValue");
        }

        /// <summary>
        /// Set the analogue value for this switch.
        /// A MethodNotImplementedException should be thrown if CanWrite returns False
        /// If the value is not between the maximum and minimum then throws an InvalidValueException
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetSwitchValue(short id, double value)
        {
            Validate("SetSwitchValue", id, value);
            if (!CanWrite(id))
            {
                tl.LogMessage("SetSwitchValue", $"SetSwitchValue({id}) - Cannot write");
                throw new ASCOM.MethodNotImplementedException($"SetSwitchValue({id}) - Cannot write");
            }
            tl.LogMessage("SetSwitchValue", $"SetSwitchValue({id}) = {value} - not implemented");
            throw new MethodNotImplementedException("SetSwitchValue");
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Checks that the switch id is in range and throws an InvalidValueException if it isn't
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        private void Validate(string message, short id)
        {
            if (id < 0 || id >= numSwitch)
            {
                tl.LogMessage(message, string.Format("Switch {0} not available, range is 0 to {1}", id, numSwitch - 1));
                throw new InvalidValueException(message, id.ToString(), string.Format("0 to {0}", numSwitch - 1));
            }
        }

        /// <summary>
        /// Checks that the switch id and value are in range and throws an
        /// InvalidValueException if they are not.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        private void Validate(string message, short id, double value)
        {
            Validate(message, id);
            var min = MinSwitchValue(id);
            var max = MaxSwitchValue(id);
            if (value < min || value > max)
            {
                tl.LogMessage(message, string.Format("Value {1} for Switch {0} is out of the allowed range {2} to {3}", id, value, min, max));
                throw new InvalidValueException(message, value.ToString(), string.Format("Switch({0}) range {1} to {2}", id, min, max));
            }
        }

        /// <summary>
        /// Checks that the number of states for the switch is correct and throws a methodNotImplemented exception if not.
        /// Boolean switches must have 2 states and multi-value switches more than 2.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="expectBoolean"></param>
        //private void Validate(string message, short id, bool expectBoolean)
        //{
        //    Validate(message, id);
        //    var ns = (int)(((MaxSwitchValue(id) - MinSwitchValue(id)) / SwitchStep(id)) + 1);
        //    if ((expectBoolean && ns != 2) || (!expectBoolean && ns <= 2))
        //    {
        //        tl.LogMessage(message, string.Format("Switch {0} has the wriong number of states", id, ns));
        //        throw new MethodNotImplementedException(string.Format("{0}({1})", message, id));
        //    }
        //}

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development
        #region ASCOM Registration

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
                P.DeviceType = "Switch";
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
        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }
        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }
        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Switch";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));                
                IP_FRITZBOX = driverProfile.GetValue(driverID, "IP-Adresse", string.Empty, "");
                FB_User = driverProfile.GetValue(driverID, "Benutzer", string.Empty, "");
                //Simple Base64 Coded Password to save as non plaintext
                FB_Password = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(driverProfile.GetValue(driverID, "Kennwort", string.Empty, "")));
                AIN_anz =  Convert.ToInt32(driverProfile.GetValue(driverID, "Anzahl AIN", string.Empty, "0"));
                numSwitch = Convert.ToInt16(AIN_anz);

                for (int i = 0; i < AIN_anz; i++)
                {
                    AINs[i] = driverProfile.GetValue(driverID, $"AIN{i}", "AIN", "");
                    AIN_name[i] = driverProfile.GetValue(driverID, $"Name{i}", "AIN", "");                   
                }

            }
        }
        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                
                driverProfile.DeviceType = "Switch";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, "IP-Adresse", IP_FRITZBOX);
                driverProfile.WriteValue(driverID, "Benutzer", FB_User);
                //Simple Base64 Coded Password to save as non plaintext
                driverProfile.WriteValue(driverID, "Kennwort", Convert.ToBase64String(Encoding.UTF8.GetBytes(FB_Password) ) ) ;
                driverProfile.WriteValue(driverID, "Anzahl AIN", Convert.ToString(AIN_anz));
                driverProfile.CreateSubKey(driverID, "AIN");
                
                for (int i = 0; i < AIN_anz; i++)
                {
                    driverProfile.WriteValue(driverID, $"Name{i}", AIN_name[i],"AIN");
                    driverProfile.WriteValue(driverID, $"AIN{i}", AINs[i],"AIN");
                }

            }
        }
        /// <summary>
        /// Log helper function that takes formatted strings and arguments
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
    }
}
