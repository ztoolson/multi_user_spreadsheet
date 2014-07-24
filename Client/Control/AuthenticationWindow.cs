using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CustomNetworking;

namespace SpreadsheetClient
{
    public partial class Authentication : Form
    {
        /// <summary>
        /// Controller member.
        /// </summary>
        private Control controller;
        public int ssCount;

        // Some members
        IPAddress address;
        string port;
        int portParse;
        string password;
        string passwordMessage;
        // 3 flags: if they are all true, then the user will be able to connect to the server via the ConnectButton.
        bool IpFlag = false, PortFlag = false, PasswordFlag = false;

        public Authentication(Control c)
        {
            ssCount = 0;
            this.controller = c;
            c.parent = this;
            

            InitializeComponent();
            // Guys
            address = null;
            port = "";
            portParse = 0;
            password = "";
            passwordMessage = "";
            // both port and password textboxes will be disabled initially.
            this.PortTextBox.Enabled = false;
            this.PasswordTextBox.Enabled = false;
            // button will remain disabled until all 3 three text boxes have valid input
            this.ConnectButton.Enabled = false;
            // event handlers for three text boxes
            this.IPTextBox.KeyPress += new KeyPressEventHandler(HandleIPTextBox);
            this.PortTextBox.KeyPress += new KeyPressEventHandler(HandlePortTextBox);
            this.PasswordTextBox.KeyPress += new KeyPressEventHandler(HandlePasswordTextBox);
        }
  
        /// <summary>
        ///  Handles text entered into the IP Address text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleIPTextBox(object sender, KeyPressEventArgs e)
        {// if enter is pressed...
            if (e.KeyChar == (char)13)
            {
                // Atempt to parse the text boxes contents into an IP Address.
                if(IPAddress.TryParse(this.IPTextBox.Text, out address))
                {// if the IP text box contents was successfully parsed,
                    //  enable ande shift focus to the Port text box.
                    this.PortTextBox.Enabled = true;
                    this.PortTextBox.Focus();
                    // set flag
                    IpFlag = true;
                  
                }
                // else if the text box cannot be parsed into an IPAddress...
                else
                {
                    // notifiy user
                    MessageBox.Show("Please provide a valid IP Address.", "Invalid IP Address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // clear text box
                    IPTextBox.Text = "";
                    // disable the other text boxes.
                    this.PortTextBox.Enabled = false;
                    this.PasswordTextBox.Enabled = false;
                    // set flag
                    IpFlag = false;
                }
            }
            // if all three fields have been entered, enable the connect button
            if (IpFlag == true && PortFlag == true && PasswordFlag == true)
            {
                this.ConnectButton.Enabled = true;
            }
        }

        /// <summary>
        /// Handles text entered into the Port text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlePortTextBox(object sender, KeyPressEventArgs e)
        {
            // if enter is pressed...
            if (e.KeyChar == (char)13)
            {
                // try to parse the text box's contents to an int, and verify if parsed string is in valid port range
                if (int.TryParse(this.PortTextBox.Text, out portParse) && portParse > 1024 && portParse <= 50009)
                {
                    // enable and shift focus to  the password text box
                    this.PasswordTextBox.Enabled = true;
                    this.PasswordTextBox.Focus();
                    //              set flag
                    PortFlag = true;
                  
                }
                // else if the text box cannot be parsed to an int
                else
                {
                    //  notify the user
                    MessageBox.Show("Please provide a valid port number.", "Invalid Port Number", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Clear text box
                    PortTextBox.Text = "";
                    // set flag to false
                    PortFlag = false;
                    // disable the password text box.
                    this.PasswordTextBox.Enabled = false;
                } 
            }
            // if all three flags are true...
            //      enable the connect button
            if (IpFlag == true && PortFlag == true && PasswordFlag == true)
            {
                this.ConnectButton.Enabled = true;
            }
        }

        /// <summary>
        /// handles text entered into the password text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandlePasswordTextBox(object sender, KeyPressEventArgs e)
        {
            // if enter is pressed...
            if (e.KeyChar == (char)13)
            {
                PasswordFlag = true;
                // Store Password in StoreConnect class.
               // StoreConnect.SetPassword(this.PasswordTextBox.Text);
            }
            if (IpFlag == true && PortFlag == true && PasswordFlag == true)
            {
                this.ConnectButton.Enabled = true;
            }
        }

        /// <summary>
        /// Handles clicking the conncet button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            // store in storeConnect class
            StoreConnect.setAddress(this.IPTextBox.Text);
            // Store port in storeConnnect class
            StoreConnect.setPort(this.PortTextBox.Text);
            // Store Password in StoreConnect class.
            StoreConnect.SetPassword(this.PasswordTextBox.Text);
                // Request a connection from the server with information provided by the user.
                this.controller.InitiateConnection(this.IPTextBox.Text, this.portParse, this.PasswordTextBox.Text);
               
        }

    } // <-- end of class
}
