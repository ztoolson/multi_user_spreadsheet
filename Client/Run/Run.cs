using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SpreadsheetClient

{
    /// <summary>
    /// This is where the application will start.
    /// </summary>
    class Run
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            IntPtr hWnd = FindWindow(null, Console.Title);
            ShowWindow(hWnd, 0);
            SpreadsheetClient.ConnectWindow.Main();
        }
    }
}
