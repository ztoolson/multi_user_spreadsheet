using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetClient
{
     public static class ConnectWindow
    {
        /// <summary>
        /// Entry point to aunthentication window.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Authentication a = new Authentication(new Control());
            Application.Run(a); 
        }
    }
}
