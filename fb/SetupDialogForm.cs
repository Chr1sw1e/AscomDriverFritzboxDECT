using ASCOM.fb;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace ASCOM.fb
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        TraceLogger tl; // Holder for a reference to the driver's trace logger

        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();
            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }
        #region Fritbox Connection
        //TODO - Use Functions from SwitchDriver.cs 
        /// <summary>
        /// Read Devices from Fritzbox and ADD to CheckedListbox for Selection
        /// </summary>
        public void Get_dect()

        {
            string benutzername = fb.Switch.FB_User;
            string kennwort = fb.Switch.FB_Password;

            // SessionID ermitteln
            string sid = GetSessionId(benutzername, kennwort);
            string kom;
            string seite;

            kom = "getswitchlist";
            seite = SeiteEinlesen($@"http://{fb.Switch.IP_FRITZBOX}/webservices/homeautoswitch.lua?switchcmd={kom}", sid);

            if (seite != "")
            {
                fb.Switch.AINs = seite.Replace("\n", "").Split(',');
                fb.Switch.AIN_anz = fb.Switch.AINs.GetLength(0);

                kom = "getswitchname";
                fb.Switch.AIN_name = new string[fb.Switch.AIN_anz];
                for (int i = 0; i < fb.Switch.AIN_anz; i++)
                {
                    seite = SeiteEinlesen($@"http://{fb.Switch.IP_FRITZBOX}/webservices/homeautoswitch.lua?switchcmd={kom}&ain={ fb.Switch.AINs[i]}", sid);
                    fb.Switch.AIN_name[i] = seite.Replace("\n", "");
                    cklbDevices.Items.Add($"Name:{ fb.Switch.AIN_name[i]} AIN:{ fb.Switch.AINs[i]}");
                }
            }
        }
        public string SeiteEinlesen(string url, string sid)

        {
            try //Check if all settings right
            {
                Uri uri = new Uri(url + "&sid=" + sid);
                HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string str = reader.ReadToEnd();

                return str;
            }
            catch (Exception ex) //Something went wrong .. not possible to connect to Fritzbox
            {
                MessageBox.Show($"Zugriffsfehler auf Fritzbox\nBitte Daten überprüfen", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return "";
            }

        }
        public string GetSessionId(string benutzername, string kennwort)

        {

            XDocument doc = XDocument.Load($@"http://{fb.Switch.IP_FRITZBOX}/login_sid.lua");

            string sid = GetValue(doc, "SID");

            if (sid == "0000000000000000")

            {

                string challenge = GetValue(doc, "Challenge");

                string uri = $@"http://{fb.Switch.IP_FRITZBOX}/login_sid.lua?username=" +

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
        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
 
            fb.Switch.IP_FRITZBOX = tbIP.Text;
            fb.Switch.FB_User = tbUser.Text;
            fb.Switch.FB_Password = tbPasswd.Text;   
            tl.Enabled = chkTrace.Checked;

            for (int i = fb.Switch.AIN_anz - 1; i > -1; i--)
            {
                if (cklbDevices.GetItemCheckState(i) == CheckState.Unchecked) //Delete Unchecked Items from Array
                {
                    fb.Switch.AIN_name = fb.Switch.AIN_name.Where(w => w != fb.Switch.AIN_name[i]).ToArray();
                    fb.Switch.AINs = fb.Switch.AINs.Where(w => w != fb.Switch.AINs[i]).ToArray();
                }
            }
            fb.Switch.AIN_anz = fb.Switch.AINs.Length; //Update new sum of devices
        }
        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }
        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
        private void InitUI()
        {
            chkTrace.Checked = tl.Enabled;
            // set the list of com ports to those that are currently available
        
            this.tbIP.Text = fb.Switch.IP_FRITZBOX;
            this.tbUser.Text = fb.Switch.FB_User;
            this.tbPasswd.Text = fb.Switch.FB_Password;
            if (tbIP.Text == "")
            {                               
                tbIP.Text = "Fritz.box"; //set Default 
            }

            for (int i = 0; i < fb.Switch.AIN_anz; i++)
            {
                cklbDevices.Items.Add($"Name:{fb.Switch.AIN_name[i]} AIN:{fb.Switch.AINs[i]}");
                this.cklbDevices.SetItemChecked(i, true);                
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //Delete
        }
        private void TB_IP_TextChanged(object sender, EventArgs e)
        {            
            fb.Switch.IP_FRITZBOX= this.Text;
        }
        private void TB_IP_Clicked(object sender, EventArgs e)
        {
            //Delete
        }
        private void FB_einlesen_Click(object sender, EventArgs e)
        {
            this.cklbDevices.Items.Clear();

            fb.Switch.FB_User = this.tbUser.Text;
            fb.Switch.FB_Password = this.tbPasswd.Text;
            fb.Switch.IP_FRITZBOX = this.tbIP.Text;

            if (fb.Switch.FB_User != "" & fb.Switch.FB_Password != "" & fb.Switch.IP_FRITZBOX != "")
            {
                Get_dect();
            }
            else 
            {
                string Message = "";
                if (fb.Switch.FB_User == "") Message = Message + "Kein Benutzername\n";
                if (fb.Switch.FB_Password == "") Message = Message + "Kein Passwort\n ";
                if (fb.Switch.IP_FRITZBOX == "")  Message = Message + "Keine IP-Adresse\n";

                MessageBox.Show($"Fehlerhafte Eingabe:\n{Message}","Fehler",MessageBoxButtons.OK,MessageBoxIcon.Hand);
            }
        }
    }
}