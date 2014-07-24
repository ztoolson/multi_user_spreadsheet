using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomNetworking;
using System.Net.Sockets;
using ParseMessage;
using System.Threading;
using System.Windows.Forms;      
using SS;
using SpreadsheetUtilities;

namespace SpreadsheetClient
{
    /// <summary>
    /// This class serves as a means of control between both windows --
    /// Authentication and Spreadsheet --
    /// and the Model for the Application.
    /// </summary>
    public class Control
    {
        // Flags for making windows vanish.
        private bool magicFlag = false;

        public Authentication parent;

        /// <summary>
        /// Socket to the server.
        /// </summary>
        public StringSocket connection;

        /// <summary>
        /// Object which will keep track of spreadsheet state.
        /// </summary>
        public SpreadsheetModel ssm
        {
            get;
            private set;
        }

        /// <summary>
        /// Tells if a connection exists.
        /// </summary>
        public Boolean IsConnection
        {
            get { return connection != null; }
        }

        /// <summary>
        /// A list of tokens parsed from a server message.
        /// </summary>
        protected List<string> parsedList; 

        /// <summary>
        /// Assigned to the name of the SS chosen by the user in the OpenFileWindow.
        /// </summary>
        private string FILELIST_ChosenName = "";

        /// <summary>
        /// 0 arg constructor which intitializes the connection to the server to null.
        /// </summary>
        public Control()
        {
            connection = null;
            parsedList = new List<string>();
        }

        /// <summary>
        /// This method connects to the Server and authenticates the client.
        /// </summary>
        /// <param name="Address">Address of the server</param>
        /// <param name="Port">Port to conect to on the server</param>
        /// <param name="Password">Password provided by the user</param>
        public void InitiateConnection(string Address, int Port, string Password)
        {
            try
            {
                // password message to be sent to server.
                string passwordMessage = "PASSWORD" + (char)27 + Password + '\n';
                // make the base client.
                TcpClient tcpClient = new TcpClient();
                // connect to the given ip address
                tcpClient.Connect(System.Net.IPAddress.Parse(Address), Port);
                // wrap the socket into a string socket.
                this.connection = new StringSocket(tcpClient.Client, UTF8Encoding.Default);  
                // send PASSWORD message to the server to signal a connection request.
                connection.BeginSend(passwordMessage, (e, o) => { }, connection);
                // socket listen for a response from the server with a FILELIST message, designate "MesssageReceived" as callback.
                connection.BeginReceive(MessageReceived, connection);
            }
            catch(Exception i)
            {
                MessageBox.Show(i.Message, "", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// This method is used to load or create a new spreadsheet in a separate window.
        /// </summary>
        public void OpenNewConnection()
        {
            try
            {
                // correct password initially entered by user.
                string passwordMessage = "PASSWORD" + (char)27 + StoreConnect.getPassword() + '\n';
                // make the base client.
                TcpClient tcpClient = new TcpClient();
                // connect to the stored ip address
                tcpClient.Connect(System.Net.IPAddress.Parse(StoreConnect.getAddress()), Int32.Parse(StoreConnect.getPort()));
                // wrap the socket into a string socket.
                
                this.connection = new StringSocket(tcpClient.Client, UTF8Encoding.Default);   
                // send PASSWORD message to the server to signal a connection request.
                //   send the stringsocket as payload.
                connection.BeginSend(passwordMessage, (e, o) => { }, connection);
                // socket listen for a response from the server with a FILELIST message, designate "MesssageReceived" as callback.
                connection.BeginReceive(MessageReceived, connection);
            }
            catch (Exception i)
            {
                MessageBox.Show(i.Message, "", MessageBoxButtons.OK);
            }
        }

        /// <summary>
        /// Sends a request to the server to open an existing spreadsheet.
        /// 
        /// // ***OPEN[esc]spreadsheet_name\n ***
        /// 
        /// </summary>
        /// <param name="SpreadsheetName">The name of the spreadsheet the user requests to open</param>
        public void SendOpen(string SpreadsheetName)  
        {
            // make the appropriate message.
            string openString = "OPEN" + (char)27 + SpreadsheetName + '\n';

            // Debug
           // MessageBox.Show("Open String:" + openString, "", MessageBoxButtons.OK);
            
            // if there is a connection...
            if(IsConnection)
            {// send the message to the server.
                connection.BeginSend(openString, (e, o) => { }, connection);
                // wait for server to respond with UPDATE message.
                connection.BeginReceive(MessageReceived, connection);
            }
        }

        /// <summary>
        /// Sends a request to the server to create a spreadsheet with the given name. 
        ///
        ///  **CREATE[esc]spreadsheet_name\n **
        ///
        /// </summary>
        /// <param name="SpreadsheetName">The name of the spreadsheet to be created</param>
        public void SendCreate(string SpreadsheetName)  
        {
            string createString = "CREATE" + (char)27 + SpreadsheetName + '\n';

            // If there is a connection...
            if (IsConnection)
            {
                // send the message to the server.
                connection.BeginSend(createString, (e, o) => { }, connection);
                // wait for server to respond with UPDATE message.
                connection.BeginReceive(MessageReceived, connection);
            }
        }

        /// <summary>
        /// Sends the requested update to the server.
        /// 
        /// // *** ENTER[esc]version_number[esc]cell_name[esc]cell_content\n 
        /// 
        /// </summary>
        /// <param name="CellName">Name of the cell to be updated</param>
        /// <param name="CellContents">Contents of the cell to be updated</param>
        /// <param name="spreadsheetName">The name of the spreadsheet to change</param>
        public void SendEnter(string Version, string CellName, string CellContents)
        {
            // make the ENTER string
            if (CellContents.StartsWith("="))
                CellContents = CellContents.ToUpper();
            string enterString = "ENTER" + (char)27 + Version + (char)27 + CellName.ToUpper() + (char)27 + CellContents + '\n';
        
            // if there is a connection...
            if(IsConnection)                               
            {
                // send the message to the server.
                connection.BeginSend(enterString, (e, o) => { }, connection);
                // wait for server to respond with UPDATE message.
                connection.BeginReceive(MessageReceived, connection);
            }
        }

        /// <summary>
        /// Sends UNDO message to server.
        /// 
        ///  UNDO[esc]version_number\n 
        /// 
        /// </summary>
        /// <param name="SpreadsheetName">The name of the spreadsheet of the last move to undo</param>
        public void SendUndo(string Version)
        {
            // Make UNDO string
            string undoString = "UNDO" + (char)27 + Version + '\n';

            // if there is a connection...
            if (IsConnection)
            {
                // send the message to the server.
                connection.BeginSend(undoString, (e, o) => { }, connection);
                // wait for server to respond with UPDATE message.
                connection.BeginReceive(MessageReceived, connection);
            }
        }

        /// <summary>
        /// Send the SAVE message to the server.
        /// 
        /// SAVE[esc]version_number\n 
        /// 
        /// </summary>
        /// <param name="Version"> Current version of the Spreadsheet.</param>
        public void SendSave(string Version)
        {
            // Make SAVE string
            string saveString = "SAVE" + (char)27 + Version+ '\n';

            // if there is a connection...
            if (IsConnection)
            {
                // send the message to the server.
                connection.BeginSend(saveString, (e, o) => { }, connection);
                // wait for server to respond with SAVED message.
                connection.BeginReceive(MessageReceived, connection);
            }
        }

        /// <summary>
        /// Send the DISCONNECT message to the server.
        /// </summary>
        /// <param name="SpreadsheetName">Name of the spreadsheet.</param>
        public void SendDisconnect()
        {
            // if there is a connection...
            if (IsConnection)
            {
                // send the message to the server.
                connection.BeginSend("DISCONNECT\n", Disconnect, connection);
            }
        }

        /// <summary>
        /// Cleanly dissconnects from the server.
        /// A callback for the begin send method.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        private void Disconnect(Exception e, object payload)
        {
            connection.Close();
            parent.ssCount--;
            
            if (parent.ssCount == 0)
                parent.Invoke(new Action(() => {parent.Close();}));  // TODO: might be causing all clients to close.
        }

        /// <summary>
        /// Method sends a request to send the most current version of the spreadsheet.
        /// </summary>
        public void SendResync()
        {
            // if there is a connection...
            if (IsConnection)
            {
                // send the message to the server.
                connection.BeginSend("RESYNC\n", (e, o) => { }, connection);
                // wait for server to respond with SYNC message.
                connection.BeginReceive(MessageReceived, connection);
            }
        }

        /// <summary>
        ///  Callback which will handle messages sent from the server.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        /*private*/ public void MessageReceived(String s, Exception e, Object payload) // TODO: CHANGE BACK TO PRIVATE ******************
        {
            try
            {
            // First thing, get a list of tokens from the message received by the socket
                this.parsedList = Parse.MessageParse(s);  

            // If the message sent from the server is in response to a PASSWORD authentication request from the client...
            if(s.StartsWith("FILELIST"))
            {
                parent.ssCount++;
                // Initialize model, here and only here.
                ssm = new SpreadsheetModel();                                         
                                // Set the correct display for the open file window.
                ssm.OpenFileWindow.SetListBoxDataSource(Parse.MessageParse(s));  
                // Show the open file window
                parent.Invoke(new Action(() => {parent.Hide();}));
                ssm.OpenFileWindow.ShowDialog();   // This window closes itself.
                // Assign chosen name member
                FILELIST_ChosenName = ssm.OpenFileWindow.GetSelectedName();
                //If the name chosen from the open file window ends with newline...
                if(ssm.OpenFileWindow.GetSelectedName().EndsWith("\n"))               
                {// ... Send the selcted name to the server as a file to open. Trim newline.
                    SendOpen(ssm.OpenFileWindow.GetSelectedName().Trim());
                    // DEBUG
                    //MessageBox.Show("Open SS:" + ssm.OpenFileWindow.GetSelectedName().Trim(), "", MessageBoxButtons.OK);
                }
                //If the name chosen from the open file window ends with tab...
                else if(ssm.OpenFileWindow.GetSelectedName().EndsWith("\t"))
                {
                    SendCreate(ssm.OpenFileWindow.GetSelectedName().Trim());
                    // DEBUG
                   // MessageBox.Show("Create SS:" + ssm.OpenFileWindow.GetSelectedName().Trim(), "", MessageBoxButtons.OK);
                }
                // else something went wrong in the Open File Window .cs
                else
                {
                    throw new FormatException("Open File window did not return a correctly formatted file name.");
                }
            }
            // if the message sent from the server is in response to an invalid password from the client...
            else if (s.StartsWith("INVALID"))
            {
                MessageBox.Show("The password provided is invalid, please enter a valid password.", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);  
                ssm.AuthenticationWindow.ShowDialog();  
            }
            // if the message sent from the server will edit the clients spreadsheet...
            else if (s.StartsWith("UPDATE"))
            {
                //*****************************************************************************************************************************
                // DEBUG: THIS INITIALIZATION IS ONLY HERE TO TEST THE UPDATE PORTION OF THE MESSAGE RECEIVED METHOD.
                //this.ssm = new SpreadsheetModel();
                //****************************************************************************************************************************

                // DEBUG
                //MessageBox.Show("Server responded UPDATE", "", MessageBoxButtons.OK);

                FormatException ue = new FormatException("UPDATE message was not correctly formated.");

                // if the number of items in list is greater than three,
                //  UPDATE message was sent from server in response to client requesting to open a file.
                if (magicFlag == false)
                {
                    magicFlag = true;
                    if (parsedList.Count > 2)
                    {
                        // UPDATE[esc]current_version[esc]cell_name1[esc]cell_content1[esc]cell_name2[esc]…\n

                        this.ssm.SpreadSheetWindow = new SpreadsheetWindow(parsedList[1], this);   // SUSPECT: this is happening when with real-time updates.
                        // Display the chosen name of the spreadsheet at the top of the window.\
                        if (this.ssm.SpreadSheetWindow.InvokeRequired)
                            this.ssm.SpreadSheetWindow.Invoke(new Action(() => { this.ssm.SpreadSheetWindow.Text = FILELIST_ChosenName; }));   
                        else
                            this.ssm.SpreadSheetWindow.Text = FILELIST_ChosenName;
                        // Set version
                        this.ssm.SpreadSheetWindow.SetSpreadsheetWindowVersion(parsedList[1]);
                        // MARK!!
                        this.ssm.SpreadSheetWindow.setController(this);
                        // Ensure the Spreadsheet is displayed properly when shown initially.
                        ServerLoadSync(this.parsedList, "UPDATE");          
                    }
                    // if there is 2 items in the list,
                    // the message sent from the server  is in response to the client requesting to create a new speadsheet.
                    else if (parsedList.Count == 2)
                    {
                        // UPDATE[esc]current_version\n

                        // Update the model's SS member with the version supplied from the message.
                        ssm.SpreadSheetWindow = new SpreadsheetWindow(this.parsedList[1], this);
                        
                        // Display the chosen name of the spreadsheet at the top of the window.
                        if (this.ssm.SpreadSheetWindow.InvokeRequired)
                            this.ssm.SpreadSheetWindow.Invoke(new Action(() => { this.ssm.SpreadSheetWindow.Text = FILELIST_ChosenName; }));  
                        else
                            this.ssm.SpreadSheetWindow.Text = FILELIST_ChosenName;

                        // Show the newly created SpreadsheetWindow
                        SpreadsheetApplicationContext.getSSAppContext().RunSpreadsheetForm(this.ssm.SpreadSheetWindow);
                    }
                }
                // if 4 items are in the list, the message sent from server is in response to the client 
                //  either requesting to change the spreadsheet or undo the last change made in the spreadsheet.
                else if (magicFlag)
                {
                    // UPDATE[esc]current_version[esc]cell_name[esc]cell_content\n

                    // IF the version from the server is expected...
                    if (Int32.Parse(this.ssm.SpreadSheetWindow.WindowSpreadsheet.Version) + 1 == Int32.Parse((parsedList[1])))   
                    {
                        // update the spreadsheets current version to the version sent by the server.
                        this.ssm.SpreadSheetWindow.WindowSpreadsheet.Version = parsedList[1];
                       
                        // DEBUG
                       // MessageBox.Show(this.ssm.SpreadSheetWindow.WindowSpreadsheet.Version, "", MessageBoxButtons.OK);
                        
                        // Populate the underlying spreadsheet with the server message
                        PopulateSpreadsheet(parsedList);
                    }
                    // Else if the version from ths server is not expected...
                    else
                    {
                        // Send resync message to the server
                        SendResync();
                    }
                }
                // else the UPDATE message sent from the server is invalid
                else
                {
                    throw ue;
                }
            }
            // if the message sent from the server is in response to a save request by the client...
            else if (s.StartsWith("SAVED"))
            {
                // DEBUG
                //MessageBox.Show("Server responded SAVED", "", MessageBoxButtons.OK);
            }
            // if the message sent from the server will sync the client's spreadsheet...
            else if (s.StartsWith("SYNC"))
            {
                List<string> tempList = new List<string>();

                foreach(string item in this.parsedList)
                {
                    tempList.Add(item);
                }

                // DEBUG
               // MessageBox.Show("Server responded SYNC", "", MessageBoxButtons.OK);

                for(int pos = 2; pos < tempList.Count ; pos += 2)
                {
                    List<string> messageList = new List<string>();
                    int version = Int32.Parse(this.ssm.SpreadSheetWindow.WindowSpreadsheet.Version) + 1;
                    
                    messageList.Add("UPDATE");
                     messageList.Add(version.ToString());
                     messageList.Add(tempList[pos]);
                     messageList.Add(tempList[pos + 1]);
                    
                     PopulateSpreadsheet(messageList);
                }

                // update the spreadsheets current version to the version sent by the server.
                this.ssm.SpreadSheetWindow.WindowSpreadsheet.Version = parsedList[1];
                // Ensure the Spreadsheet is displayed properly when synced.
                //ServerLoadSync(this.parsedList, "SYNC");
            }
            // if the message sent from the server is in response to a errant message sent by the client...
            else if (s.StartsWith("ERROR"))
            {
                // Display the error sent by the server in a message box
                MessageBox.Show("There was an error:\n\t" + this.parsedList[1], "Server Responded with Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //**************************************************************************************************************
            //**************************************************************************************************************
                // else if the message sent by the server is unrecognized by the client...
            else
            {
                // DEBUG: THIS NEEDS TO GO AT SOME POINT.
                if (s != "")
                {
                    // DEBUG
                    MessageBox.Show("Server responded with something quite unnexpected", "", MessageBoxButtons.OK);
                }
            }
                //**************************************************************************************************************
                //**************************************************************************************************************
        }
            catch(Exception t) 
            {
              MessageBox.Show(t.Message, "Error", MessageBoxButtons.OK);
            }

            try
            {
                // Never stop listening!
                connection.BeginReceive(MessageReceived, connection);
            }
            catch
            {}

        } // <-- end of MessageReceived()

        /// <summary>
        /// Updates the spreadsheet GUI upon the server signaling the client to either load a spreadsheet or sync a spreadsheet.
        /// </summary>
        /// <param name="ParsedList"></param>
        private void ServerLoadSync(List<string> ParsedList, string message)
        {
            // Update the model's SS member with the version supplied from the message.
            //ssm.SpreadSheetWindow = new SpreadsheetWindow(ParsedList[1], this);                      
            ssm.SpreadSheetWindow.WindowSpreadsheet.Version = ParsedList[1];
            // apply the chosen name of the SS to the name property of the SpreadSheetWindow.
            ssm.SpreadSheetWindow.Name = FILELIST_ChosenName;
            // Populate the underlying spreadsheet with the server message
            PopulateSpreadsheet(ParsedList);
            // Display cell A1's info  upon the spreadsheet first showing.
            ssm.SpreadSheetWindow.DisplayStartUpTextBoxes();
            // if the messsage sent by the server was UPDATE....
            if (message == "UPDATE")
            {
                // show the newly populated spreadsheet.
                SpreadsheetApplicationContext.getSSAppContext().RunSpreadsheetForm(this.ssm.SpreadSheetWindow);
            }
        }

        /// <summary>
        /// Populates the underlying spreadsheet with the message sent by the server.
        /// </summary>
        /// <param name="parsedList">List representing the message sent by the server</param>
        private void PopulateSpreadsheet(List<string> parsedList)
        {
            // UPDATE[esc]current_version[esc]cell_name1[esc]cell_content1[esc]cell_name2[esc]…\n

            //  guys.
            int row, col;
            string value;
            object cellValue;

            for (int pos = 2; pos < parsedList.Count; pos += 2 )
            {
                //  place current cell name and cell contents into a tuple.
                Tuple<string, string> cell = new Tuple<string, string>(parsedList[pos], parsedList[pos + 1]);
                
                //Get the row and column for the cell;
                col = cell.Item1[0] - 65;
                Int32.TryParse(cell.Item1.Substring(1), out row);
                //  In case of FormulaError.
                object previousCellContents = null;
                //  help with updating
                ISet<string> changlings;
                
                // Before calling SCC, get the Cell's current contents for safe keeping.     
                //  This check also informs the program whether or not the previous Cell's contents 
                // was a value that could be depended upon by other Cells.
                previousCellContents = this.ssm.SpreadSheetWindow.getWindowSpreadsheetCellContents2(cell.Item1);
                //  Put values in SS member, trim just in case.
                changlings = this.ssm.SpreadSheetWindow.ReturnSetContentsOfCell(cell.Item1, cell.Item2);

                //  If the currently selected Cell's contents is a Formula...
                    if(cell.Item2.StartsWith("="))
                    {
                        //Check to see if the cell's value is a Formula error.      
                        if ((cellValue = this.ssm.SpreadSheetWindow.getWindowSpreadsheetCellValue(cell.Item1)) is FormulaError)
                        {
                            // put "error" in panel
                            if (this.ssm.SpreadSheetWindow.InvokeRequired)
                                this.ssm.SpreadSheetWindow.Invoke(ssm.SpreadSheetWindow.delePanel, new Object[] {col, row - 1, "ERROR"});
                            else
                                this.ssm.SpreadSheetWindow.SpreadsheetPanel1SetValue(col, row -1, "ERROR");  
                            //  and put an explanatory message in value text box.
                            continue;
                        }
                            //  Else if the cell value was not a FormulaError, 
                            //      Display the contents with pre-appended "=" in the CellContentsTextBox.
                        else
                        {
                            /*
                            if (this.ssm.SpreadSheetWindow.InvokeRequired)
                                this.ssm.SpreadSheetWindow.Invoke(ssm.SpreadSheetWindow.deleContents, new Object[] {"=" + cell.Item2});
                            else
                                this.ssm.SpreadSheetWindow.SetCellContentsTextBox("=" + cell.Item2);*/
                        }
                        int outrow, outcol;
                        this.ssm.SpreadSheetWindow.spreadsheetPanel1.GetSelection(out outcol, out outrow);
                        if (outcol == col && outrow == row - 1)
                        {
                            if (this.ssm.SpreadSheetWindow.InvokeRequired)
                                this.ssm.SpreadSheetWindow.Invoke(ssm.SpreadSheetWindow.deleValue, new Object[] { cellValue.ToString() });
                            else
                                this.ssm.SpreadSheetWindow.SetCellValueTextBox(cellValue.ToString());  
                        }
                    }
                        //  Else if the currently selected Cell's Contents is not a formula...
                    else
                    {
                        //  check to see if the previous contents was dependable.
                        if (previousCellContents is Formula || previousCellContents is double)
                        { // if the cell we are trying to change who's previous contents was a formula, also had cells that were dependent upon it...
                            foreach (string cellName in changlings)
                            {//     If any cellName in "changlings" has a contents that matches the current cell...
                                if (this.ssm.SpreadSheetWindow.getWindowSpreadsheetCellContents2(cellName) == cell.Item2)
                                {// ...Update dependent cells both back and front
                                    changlings = this.ssm.SpreadSheetWindow.ReturnSetContentsOfCell(cell.Item1, cell.Item2);  
                                }
                            }
                        }
                            // Else if the previous contents was a string...
                        else
                        {
                            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                            // TODO: **We need to decide how to handel a string contents chosen by the containing [esc] and \n chars.**
                            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                            
                            // Display "cellContents" as is.
                        }
                        int outrow, outcol;
                        this.ssm.SpreadSheetWindow.spreadsheetPanel1.GetSelection(out outcol, out outrow);
                        if (outcol == col && outrow == row - 1)
                        {
                            if (this.ssm.SpreadSheetWindow.InvokeRequired)
                                this.ssm.SpreadSheetWindow.Invoke(ssm.SpreadSheetWindow.deleValue, new Object[] { cell.Item2 });
                            else
                                this.ssm.SpreadSheetWindow.SetCellValueTextBox(cell.Item2);
                        }
                    }
                //  Put the value of the current cell in the selected panel.  
                if (ssm.SpreadSheetWindow.InvokeRequired)
                    this.ssm.SpreadSheetWindow.Invoke(ssm.SpreadSheetWindow.delePanel, new Object[] {col, row - 1, this.ssm.SpreadSheetWindow.getWindowSpreadsheetCellValue(cell.Item1).ToString()});
                else
                    this.ssm.SpreadSheetWindow.SpreadsheetPanel1SetValue(col, row - 1, this.ssm.SpreadSheetWindow.getWindowSpreadsheetCellValue(cell.Item1).ToString());  // SUSpect
                //  Get the value of the current cell.
                value = this.ssm.SpreadSheetWindow.getWindowSpreadsheetCellValue(cell.Item1).ToString();  
                //  Display Currently Selected Cell's value in the "CellValueTextBox".
                //this.ssm.SpreadSheetWindow.Invoke(new Action(() => { ssm.SpreadSheetWindow.CellValueTextBox.Text = value; }));

                //if (ssm.SpreadSheetWindow.InvokeRequired)
                //    this.ssm.SpreadSheetWindow.Invoke(ssm.SpreadSheetWindow.deleValue, new Object[] { value });
                //else
                //    this.ssm.SpreadSheetWindow.SetCellValueTextBox(value);

                //  Refresh the display.
                this.ssm.SpreadSheetWindow.RefreshSpreadsheetWindowDisplay(changlings);
            }
        }

    }  // <-- end of control class
}
