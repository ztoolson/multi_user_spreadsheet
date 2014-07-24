using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SS;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;

namespace SpreadsheetClient
{
    /// <summary>
    /// This class handles the spreadsheet GUI.
    /// </summary>
    public partial class SpreadsheetWindow : Form
    {
        public delegate void SetText(string s);
        public delegate void SetPanel(int a, int b, string c);
        public SetText deleValue;
        public SetText deleContents;
        public SetPanel delePanel;
        private bool magicFlag = false;

        /// <summary>
        /// Control member. 
        /// </summary>
        public Control controller;

      /// <summary>
        /// Spreadsheet member.
      /// </summary>
        public Spreadsheet WindowSpreadsheet;

        // Cell Name validity Method for SS member initialization.
        private static bool WindowValidName(string name)
        {   
            return true;
        }

        /// <summary>
        /// Constructor for the GUI.
        /// </summary>
        public SpreadsheetWindow(String Version, Control Controller)   

        {
            // Initialize controller
            this.controller = Controller;

            //  Make the window.
            InitializeComponent();
            //  Initialize SS member
            WindowSpreadsheet = new Spreadsheet(s => WindowValidName(s), s => s.ToUpper(), Version); 

            // ...borrowed from the demo.
            spreadsheetPanel1.SelectionChanged += displaySelection;
            spreadsheetPanel1.SelectionChanged += DisplaySelectedCellContents;

            //  Initially select the most upper left cell.
            spreadsheetPanel1.SetSelection(0, 0);
            //  This is weak, but I'm doing it anyway.
            CellNameTextBox.Text = "A1";
            //  Every time a key is pressed while the CellContentsTextBox is selected,
            //      ApplyContentsBoxText method is called.
            this.CellContentsTextBox.KeyPress += new KeyPressEventHandler(ApplyContentsBoxText);

            deleValue = new SetText(SetCellValueTextBox);
            deleContents = new SetText(SetCellContentsTextBox);
            delePanel = new SetPanel(SpreadsheetPanel1SetValue);
        }

        /// <summary>
        /// Fills the appropriate text boxes and panel of the currently selected cell.
        /// </summary>
        /// <param name="ssp"></param>
        private void displaySelection(SpreadsheetPanel ssp)
        {//     helper guy
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //  out guys
            int row, col;
            string value;
            //  Get the coordinates of the currently selected Cell.
            ssp.GetSelection(out col, out row);
            //  Get the vlaue of the currently selected Cell.
            ssp.GetValue(col, row, out value);
            //  Will be assigned to currently selected Cell's contents.
            object cellContents;
            //  will be assigned to currently selected Cell's value.
            object cellValue;
            //  Will be assigned to a Cell's contents if said contents is a Formula.
            Formula cellContentFormula;

            //  If the cell is empty...
            if (value == "")
            {//     Display the currently selected cell's name in the "CellNameTextBox".
                this.CellNameTextBox.Text = alphabet[col] + (row + 1).ToString();   
                //Display the empty string the Cell value text box.     
                this.CellValueTextBox.Text = this.WindowSpreadsheet.GetCellValue(alphabet[col] + (row + 1).ToString()).ToString();
            }
            //  Else if the cell is not empty...
            else   
            {// Store the currently selected cell's contents.
                cellContents = this.WindowSpreadsheet.GetCellContents((alphabet[col] + (row + 1).ToString()));  
                //  If "cellContents" is a Formula...
                if (cellContents is Formula)
                {
                    cellContentFormula = (Formula)cellContents;
                    // slightly suspect.
                    if((cellValue = this.WindowSpreadsheet.GetCellValue(alphabet[col] + (row + 1).ToString())) is FormulaError)
                    {// get the coordinates of the currently selected cell.
                        spreadsheetPanel1.GetSelection(out col, out row);
                        // display cell name in "name" textbox.
                        this.CellNameTextBox.Text = (alphabet[col] + (row + 1).ToString()).ToString();  
                            //Display "ERROR" in the panel.
                        spreadsheetPanel1.SetValue(col, row, "ERROR");
                        
                        //  Display the reason for the formula error in the Value text box.
                        this.CellValueTextBox.Text = cellValue.ToString(); 

                        return;
                    }
                    //  Else if currently selected Cell's value is not a FormulaError...
                    else
                    {//    Set the panel
                        spreadsheetPanel1.SetValue(col, row, this.WindowSpreadsheet.GetCellValue(alphabet[col] + (row + 1).ToString()).ToString());
                         // Set the CellValueTextBox.
                        this.CellValueTextBox.Text = this.WindowSpreadsheet.GetCellValue(alphabet[col] + (row + 1).ToString()).ToString();
                        // Set the Cell Name text box.
                        this.CellNameTextBox.Text = (alphabet[col] + (row + 1).ToString()).ToString();
                        
                        return;
                    }
                }
                //Else if "cellContents" is not a Formula...
                else
                {
                    //     Display the current selected cell's value in the spreadsheet panel.      
                    spreadsheetPanel1.SetValue(col, row, this.WindowSpreadsheet.GetCellValue(alphabet[col] + (row + 1).ToString()).ToString());
                    //   Display the currently selected cell's value in the "CellValueTextBox". 
                    this.CellValueTextBox.Text = this.WindowSpreadsheet.GetCellValue(alphabet[col] + (row + 1).ToString()).ToString();
                    //  Display Cell Name.
                    this.CellNameTextBox.Text = (alphabet[col] + (row + 1).ToString()).ToString();
                    
                    return;
                }
            }
        } // <-- end of display selection. 

        /// <summary>
        /// Event handler for choosing the close option in the file menu,
        ///     adheres to outlined closing protocol.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseOption_Click(object sender, EventArgs e) 
        {
            // TODO: we need to make this method call the onFormClosing method instead somehow.
            
            this.OnFormClosing(new FormClosingEventArgs(new CloseReason(), true));
            if (magicFlag)
                this.Close();
        }  

        /// <summary>
        /// Method that handles clicking the close (X) button.
        /// I tried to combine the X button with the Close option in the File menu,
        /// but instead you have two nearly identical methods.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.WindowSpreadsheet.Changed && magicFlag == false)
            {
                // Show the save file dialog.
                DialogResult wishToSave = MessageBox.Show("Do you wish to save the current Spreadsheet before closing? If you choose not to save the current Spreadsheet, data will be lost.", "Save Before Closing?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                //If No is chosen.
                if (wishToSave == DialogResult.No)
                {
                    //----------------------------------------------------------------------------------------------------------------------------------------------
                    // UNCOMMENT WHEN TESTING IS DONE
                    // notify the server client closing, and close client connection.
                   SendClose();   
                    //----------------------------------------------------------------------------------------------------------------------------------------------
                   magicFlag = true;
                    e.Cancel = false;
                    //this.Close();
                    return;
                }
                    // If Yes is chosen.
                if (wishToSave == DialogResult.Yes)
                {
                    this.controller.SendSave(this.WindowSpreadsheet.getVersion());
                    //    //----------------------------------------------------------------------------------------------------------------------------------------------
                    //    // UNCOMMENT WHEN TESTING IS DONE
                    //    // notify the server client closing, and close client connection. 
                    SendClose();
                    magicFlag = true;
                    //    //----------------------------------------------------------------------------------------------------------------------------------------------
                    //    //  Then close.
                       e.Cancel = false;
                       //this.Close();
                    //    return;
                }
                //  If cancel was chosen..
                else
                {
                    magicFlag = false;
                    e.Cancel = true;
                    //  Do nothing, return.
                    return;
                }
                //Else if cancel is chosen, do nothing, return.
                //e.Cancel = true;
                //return;
            }
            else if (magicFlag == false)
            {
                SendClose();
                e.Cancel = false;
                //this.Close();
                return;
            }
            base.OnFormClosing(e);               
        }

        /// <summary>
        /// Method that applies the text within the CellContentsTextBox to the appropriate panel,
        ///     once the Enter key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplyContentsBoxText(object sender, KeyPressEventArgs e)
        {   //  out guys.
            int row, col;
            string value;
            //  helper guy.
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //  Will be assigned the currently selected cell's contents.
            object cellContents;
            object cellValue;

            // POSSIBLY STRIKE
            //  In case of FormulaError.
            object previousCellContents = null;

            // Upon the user entering an enter char into the cell contents text box...
            if (e.KeyChar == (char)13)        
            {

                //  Try block for for various exceptions.
                try
                {

                    if (this.CellContentsTextBox.Text.StartsWith("="))
                    {
                        // Check to see if the contents throws any exceptions.
                        Formula checkFormula = new Formula(this.CellContentsTextBox.Text.ToString().Substring(1));
                    }
                    //----------------------------------------------------------------------------------------------------------------------------------------------------------
                    // UNCOMMENT THIS WHEN TESTING IS DONE.
                    // send the appropriate message to the server.
                    controller.SendEnter(this.WindowSpreadsheet.Version, this.CellNameTextBox.Text, this.CellContentsTextBox.Text);
                    //----------------------------------------------------------------------------------------------------------------------------------------------------------

                }  //<-- end of try.

                    // MOST OR ALL OF THIS EXCEPTION CATCHING CAN GO.
                    // I'M PRETTY SURE IT IS NOT POSSIBLE TO TRIGGER ALL OF THESE!
                // STRIKE.
                //  Catch various exceptions.
                //catch (CircularException circ)
                //{
                //    MessageBox.Show("Attempted assignment results in a circular dependency", "Circular Dependency Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    RestoreCellContentsTextBox(previousCellContents);
                //}
                    // PROBABLY STRIKE.
                //catch (InvalidNameException n)//    UPON A LITTLE TESTING, IT DOES NOT SEEM POSSIBLE TO TRIGGER THIS EXCEPTION
                //{                                                                           //  THIS MAY OR MAY NOT BE SOMETHING WE NEED TO WORRY ABOUT.
                //    MessageBox.Show("You have either provided an invalid cell name or formula.", "Invalid Cell Name Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    RestoreCellContentsTextBox(previousCellContents);
                //}
                    //PROBABLY STRIKE.
                    //  This exception is thrown from Evaluator.dll
                catch (FormatException f)
                {
                    MessageBox.Show("Proposed dependency is invalid.", "Invalid Dependency Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RestoreCellContentsTextBox(previousCellContents);
                }
                //    // PROBABLY SRIKE.
                //catch (ArgumentException a)
                //{
                //    MessageBox.Show("At this point, you should know better than to try that.", "Invalid Cell Name Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    RestoreCellContentsTextBox(previousCellContents);
                // }
                    // PROBABLY STRIKE.
                catch (FormulaFormatException f)
                {                                                                                                      
                    MessageBox.Show("That right 'cher, is what we call an invalid formula!", "Invalid Formula Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //RestoreCellContentsTextBox(previousCellContents);
                }

            }// <-- end if enter key is pressed
        
        }// <- end of ApplyContentsBoxText


        /// <summary>
        /// Method/Event Handler that display's the currently selected cell's 
        ///     contents in the Contents Text Box.
        /// </summary>
        /// <param name="sspForNoReason">Maybe I should make this 0 argument??</param>
        private void DisplaySelectedCellContents(SpreadsheetPanel sspForNoReason)
        {
            String Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //  column corresponds to the leading letter of a cell name.
            int col; 
            //  Row corresponds to the trailing number of a cell name.
            int row;
            //  Assigned to the currently selected cell's contents.
            object cellContents;

            //  Get the currently selected cell's information.
            spreadsheetPanel1.GetSelection(out col, out row);
            //  Get the currently selected cell's contents.
            cellContents = this.WindowSpreadsheet.GetCellContents(Alphabet[col] + (row + 1).ToString());
            //  If the currently selected cell's contents is a Formula...
            if (cellContents is Formula)
            {
                this.CellContentsTextBox.Text = "=" + cellContents.ToString();
            }
            //  Else if the currently selected cell's contents is not a formula...
            else
            {
                //  Display the currently selected cell's contents in the "CellContentsTextBox".
                this.CellContentsTextBox.Text = this.WindowSpreadsheet.GetCellContents(Alphabet[col] + (row + 1).ToString()).ToString();
            }
        }

        /// <summary>
        /// Event handler for opening a Spreadsheet file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenOption_Click(object sender, EventArgs e)
        {
            Control newC = new Control();
            newC.parent = this.controller.parent;
            newC.OpenNewConnection();  
        }

        /// <summary>
        /// Event handler for saving a Spreadsheet to a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveOption_Click(object sender, EventArgs e)
        {
            // Send the appropriate message to the server
            this.controller.SendSave(this.WindowSpreadsheet.getVersion());
        }

        //************************************** Added for 3505 ***************************************
        /// <summary>
        /// Event handler for undo button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UndoButton_Click(object sender, EventArgs e)
        {
            // Send UNDO message to server.
            this.controller.SendUndo(this.WindowSpreadsheet.getVersion());   // TODO: test this.
        }

        /// <summary>
        /// This method helps to show updated dependencies in the panel.
        /// </summary>
        /// <param name=updatetees></param>
        private void RefreshDisplay(ISet<String> updatees)
        {//     out Guys.
            char column;
            int row;
            // helper guy
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            object cellValue;

            //foreach cell name in updatees..
            foreach (string cellName in updatees)
            {
                //Parse cell name into column and row.
                column = cellName[0];
                Int32.TryParse(cellName.Substring(1, cellName.Length - 1), out row);

                cellValue = this.WindowSpreadsheet.GetCellValue(cellName);

                if(cellValue is FormulaError)
                {//     Set the panel.
                    this.spreadsheetPanel1.SetValue(alphabet.LastIndexOf(column.ToString()), row - 1, "ERROR");
                   //************************* NEW CHANGES MADE BETWEEN 3500 & 3505 ****************************************************
                    //  Set the Value TextBox.
                    this.CellValueTextBox.Text = (this.WindowSpreadsheet.GetCellValue(cellName)).ToString();
                    continue;
                }
                // call setDisplay on parsed column and row.                                                           
                this.spreadsheetPanel1.SetValue(alphabet.LastIndexOf(column.ToString()), row-1, cellValue.ToString());
            }        
         }

        /// <summary>
        /// Little helper method that restores the CellContentsTextBox to its propers state in case of an error or loading a file.
        ///   This method is executed whenever an exception is thrown within this class.
        /// </summary>
        /// <param name="previousCC">The contents of a cell, usually the contents of a cell prior to an error, 
        /// except in the case of loading a Spreadsheet file</param>
        private void RestoreCellContentsTextBox(object previousCC) 
        {
            if (previousCC is Formula)
            {
                this.CellContentsTextBox.Text = "=" + previousCC.ToString();
                return;
            }
            else
                this.CellContentsTextBox.Text = previousCC.ToString();
            return;
        }

        /// <summary>
        /// Event handler that displays the help menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a our Spreadsheet.\nYou can select a cell by clicking on it.\nYou can set a cell's contents by entering a value in the text box labeled contents.\nYou can set a cell equal to another cell by preceding the assignment with = in the Contents text box.\nOpening a new Spreadsheet, saving a Spreadsheet, loading a Spreadsheet, and closing a Spreadsheet are all accomplished in the File menu.\nYou also have the option of opening a new Spreadsheet without creating a new window in the File menu.\nThis program assumes no liability for lost data or hurt feelings because it is better than other Spreadsheets.", "How To Use");                               
        }

        //****************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Populates the member spreadsheet with cells from the server.
        /// </summary>
        /// <param name="cell"></param>
        public void ServerPopulate(Tuple<string, string> cell)
        {
            this.WindowSpreadsheet.SetContentsOfCell(cell.Item1, cell.Item2);
        }

        //****************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Displays a cell from the server onto the Spreadsheet gui.
        /// </summary>
        /// <param name="cell"></param>
        public void ServerDisplayCell(Tuple<string, string> cell)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // parse the tuples cellname
            int col = alphabet.LastIndexOf(cell.Item1[0]);
            int row = 0;
            Int32.TryParse(cell.Item1.Substring(1), out row);
            // Update the panel display.    
            spreadsheetPanel1.SetValue(col, row - 1, this.WindowSpreadsheet.GetCellValue(cell.Item1).ToString());
        }

        //****************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Displays the proper values in the Spreadsheet text boxes when the Spreadsheet first shows.
        /// The Spreadsheet starts in the default cell position A1.
        /// </summary>
        public void DisplayStartUpTextBoxes()
        {
            this.CellValueTextBox.Text = this.WindowSpreadsheet.GetCellValue("A1").ToString();
            //if A1's contents is a formula
            if (this.WindowSpreadsheet.GetCellContents("A1") is Formula)
            {
                // preappend =
                this.CellContentsTextBox.Text = "=" + this.WindowSpreadsheet.GetCellContents("A1").ToString();
            }
            // else if A1's contents is not a formula....
            else
            {
                this.CellContentsTextBox.Text = this.WindowSpreadsheet.GetCellContents("A1").ToString();
            }
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Closes the window and the connection to the server.
        /// </summary>
        private void SendClose()
        {
            // Send the DICONNECT message to the server.
            this.controller.SendDisconnect();
            // Donesies!
            return;
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Another Getter for the contoller
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public object getWindowSpreadsheetCellContents2(string cellName)
        {
            return this.WindowSpreadsheet.GetCellContents(cellName);
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        ///  Another getter for the controller
        /// </summary>
        /// <returns></returns>
        public object getWindowSpreadsheetCellValue(string cellName)
        {
            return this.WindowSpreadsheet.GetCellValue(cellName);
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Another Getter for the controller.
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="cellContents"></param>
        /// <returns></returns>
        public ISet<string> ReturnSetContentsOfCell(string cellName, string cellContents)
        {
            return this.WindowSpreadsheet.SetContentsOfCell(cellName, cellContents);
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// A setter for the Controler.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="message"></param>
        public void SpreadsheetPanel1SetValue(int col, int row, string message)
        {
            this.spreadsheetPanel1.SetValue(col, row, message);
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Another setter for the controler.
        /// </summary>
        /// <param name="message"></param>
        public void SetCellValueTextBox(string message)
        {
            this.CellValueTextBox.Text = message;
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Another setter for the controller.
        /// </summary>
        /// <param name="message"></param>
        public void SetCellContentsTextBox(string message)
        {
            this.CellContentsTextBox.Text = message;
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        /// Another setter for the controller
        /// </summary>
        /// <param name="changlings"></param>
        public void RefreshSpreadsheetWindowDisplay(ISet<string> changlings)
        {
            RefreshDisplay(changlings);
        }

        //*********************************** ADDED FOR 3505 ********************************************************
        /// <summary>
        ///  Another setter for the controller.
        /// </summary>
        public void SetSpreadsheetWindowVersion(string version)
        {
            this.WindowSpreadsheet.setVersion(version);
        }

        /// <summary>
        /// Gets the current version
        /// </summary>
        /// <returns></returns>
        public string GetSpreadsheetWindowVersion()
        {
            return this.WindowSpreadsheet.getVersion();
        }

        /// <summary>
        /// Event handler for opening a new SS in the same window.
        /// This is my interesting added thingy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newInThisWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {}

        // --------------------- THIS IS NO LONGER OF ANY USE ---------------------------
        /// <summary>
        /// TODO: THIS METHOD NEEDS TO BE CHANGED FOR 3505.
        /// Handler for the save window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //string fileSavename = saveFileDialog1.FileName;
        }

        /// <summary>
        /// Event handler for choosing to open a new Spreadsheet in a separate window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newInSeperateWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {}

        public void setController(Control c)
        {
            this.controller = c;
        }

        //nah
        private void thatsRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newInThisWindowToolStripMenuItem_Click(sender, e);
        }

        //this is not used.     
        private void CellContentsTextBox_TextChanged(object sender, EventArgs e) { }
        
    }//<--- end of class
}
