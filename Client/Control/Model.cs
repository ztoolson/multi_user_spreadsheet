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
        /// <summary>
        /// The name of  the current spreadsheet
        /// </summary>
        public String SpreadsheetName; 

        /// <summary>
        /// The Model member Spreadsheet.
        /// </summary>
        public SpreadsheetWindow SpreadSheetWindow;

        /// <summary>
        /// A list of tokens parsed from a message from the server.
        /// </summary>
        public List<string> messageList;  

        /// <summary>
        /// Open/Create file window member.
        /// </summary>
        public OpenFileList OpenFileWindow;

        /// <summary>
        ///  AuthenticationWindow member.
        /// </summary>
        public Authentication AuthenticationWindow;

        /// <summary>
        /// Model for the application
        /// </summary>
        /// <param name="spreadsheetName"></param>
        public SpreadsheetModel()  
        {
            SpreadsheetName = "";  

            messageList = new List<string>();

            OpenFileWindow = new OpenFileList();

            AuthenticationWindow = new Authentication(new Control());   // THIS IS AS SUSPECT AS IT GETS!
        }
    }
}
