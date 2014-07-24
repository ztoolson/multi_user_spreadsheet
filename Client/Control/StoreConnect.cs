using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetClient
{
    /// <summary>
    ///  This class stores information entered into the authentication window.
    /// </summary>
    class StoreConnect
    {
        private static string Address = "";
        private static string Port = "";
        private static string Password = "";

        public static void setAddress(string address)
        {
            Address = address;
        }

        public static string getAddress()
        {
            return Address;
        }

        public static void setPort(string port)
        {
            Port = port;
        }

        public static string getPort()
        {
            return Port;
        }

        public static void SetPassword(string password)
        {
            Password = password;
        }

        public static string getPassword()
        {
            return Password;
        }
    }
}
