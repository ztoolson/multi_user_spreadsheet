using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SS;

namespace SpreadsheetClient
{
    public class SpreadsheetModel
    {
        // TODO: Which members do we need?
        //                  An actual Spreadsheet?
        //                  Seeting the SpreadsheetGUI as start up makes a Spreadsheet pop up on the screen when ran.

        /// <summary>
        /// The name of  the current spreadsheet
        /// </summary>
        public String SpreadsheetName /*{ get; private set; }*/; // TODO: how is this going to work with running multiple Spreadsheets on the same client?

        /// <summary>
        /// The Model member Spreadsheet.
        /// </summary>
        public Spreadsheet SpreadSheet;

        /// <summary>
        /// A list of tokens parsed from a message from the server.
        /// </summary>
        public List<string> messageList;  // TODO: public for now.......

        public OpenFileList = OpenFileWindow;

        /// <summary>
        /// Model for the application
        /// </summary>
        /// <param name="spreadsheetName"></param>
        public SpreadsheetModel(/*string spreadsheetName*/)  // TODO: for now 0 argument constructor.
        {
            SpreadsheetName = "";  // TODO: having the name as part of the model will be convenient, but this structrue is supect.

            this.SpreadSheet = new Spreadsheet();  // TODO: for now we will just go with the 0 argument constructor.
            // TODO:     public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) :
                                        //base(isValid, normalize, version)
            //                      THIS SEEMS LIKE THE BEST CONSTRUCTOR TO CALL HERE, 
            //                          IF WE GO WITH THIS CONSTRUCTOR WE WILL NEEK TO DEFINE isValid & normalize Funcs,
            //                          OR MAYBE WE CAN JUST PASS LAMBDAS.

            messageList = new List<string>();
        }

    }
}
