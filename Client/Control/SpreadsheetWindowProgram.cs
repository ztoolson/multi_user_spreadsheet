using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetClient
{
    /// <summary>
    /// Portion of the solution that handls each visible spreadsheet window
    /// </summary>
    /*public*/ class SpreadsheetApplicationContext : ApplicationContext  // TODO: change back to private.
    {
        // Open window counter.
        private int ssWindowCount = 0;
        // AppContext member ??
        private static SpreadsheetApplicationContext ssAppContext;
        //  AppContext constructor ??
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <returns></returns>
        public static SpreadsheetApplicationContext getSSAppContext()
        {
            if (ssAppContext == null)
                ssAppContext = new SpreadsheetApplicationContext();

            return ssAppContext;
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="ssWindow"></param>
        public void RunSpreadsheetForm(SpreadsheetWindow ssWindow)
        {// Increment window count member upon launching of new SS window.
            ssWindowCount++;
            //  Event handler for closing of an SS window.  ????
            //      I don't think this is actuall
            ssWindow.FormClosed += (e, o) => { if (--ssWindowCount <= 0) ExitThread(); };
        
            //  Run Form.
            ssWindow.ShowDialog();
        }

        //public void RunNewSameSpreadsheetForm(SpreadsheetWindow ssWindow)
        //{
        //    //  Event handler for closing of an SS window.  ????
        //    ssWindow.FormClosed += (e, o) => { if (--ssWindowCount <= 0) ExitThread(); };

        //    ssWindow = new SpreadsheetWindow(null);  // TODO: change param from null to the appropriate controller

        //    //ssWindow.Show();
        //}
    }

    /// <summary>
    /// Portion of the solution that handles the actually running of the GUI.
    /// </summary>
    public static class SpreadsheetWindowProgram // TODO: change back to private
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()  // TODO: change back to private
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //  Start an SS application context and run a form inside of it.
            SpreadsheetApplicationContext mainSSAppContext = SpreadsheetApplicationContext.getSSAppContext();
            //  ???
            mainSSAppContext.RunSpreadsheetForm(new SpreadsheetWindow(null, new Control())); // TODO: change param from null to the appropriate controller
            //  ??
            Application.Run(mainSSAppContext);
        }
    }
}
