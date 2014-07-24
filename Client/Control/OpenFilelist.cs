using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;  

namespace SpreadsheetClient
{
    /// <summary>
    ///  Represents the window that lets the user choose the file to open or create.
    /// </summary>
    public partial class OpenFileList : Form
    {
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Disable the X button.
        //CITE: http://www.c-sharpcorner.com/uploadfile/itsraj007/how-do-i-disable-the-close-x-button-in-a-windows-form-using-C-Sharp/
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Control member. 
        /// </summary>
        public SpreadsheetClient.Control controller;  // TODO: do I really need this member?
        
        /// <summary>
        /// The name of gthe file selected from the list box
        /// </summary>
        private string SelectedName = "";
     
        public OpenFileList()
        {
            controller = new Control();
            InitializeComponent();
            this.OpenFileButton.Click += new EventHandler(OpenFileButton_Click);
            this.CreateSpreadsheetTextBox.KeyPress += new KeyPressEventHandler(HandleCreateSpreadsheetTextBox);

            // Close this window via the controller.
            //    controller.CloseOpenFile += Close;
        }

        /// <summary> 
        /// When the open button is clicked,
        ///   the currently selected file in the FileList box will be chosen as the file to open from the server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            // set Selected file name member. Append a newline for the controllers benifit.
            SelectedName = this.FIleListBox.SelectedItem.ToString() + '\n';
            // Close this window.
            this.Close();
        }

        /// <summary>
        /// Method for handling "CreateSpreadsheetTextBox".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleCreateSpreadsheetTextBox(object sender, KeyPressEventArgs e)
        {
            // if enter is pressed...
            if (e.KeyChar == (char)13)
            {
                SelectedName = this.CreateSpreadsheetTextBox.Text;
                if (!SelectedName.EndsWith(".ss"))
                    SelectedName += ".ss";
                SelectedName += '\t';

                this.Close();
            }
        }

        /// <summary>
        /// Sets the Data source member of the Lists box to member FileList.
        /// </summary>
        public void SetListBoxDataSource(List<string> DataSource)
        {
            // Remove the message label from the list.
            DataSource.Remove(DataSource[0]);
            // Remove any instance of an empty string from the list.
            DataSource.RemoveAll(x => x == "");
            // Set "DataSource" as the list box's data source.
            this.FIleListBox.DataSource = DataSource;    
        }

        // don't think we are using this.  But removing it causes build errors.
        private void OpenFileList_Load(object sender, EventArgs e)
        {
            //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // CITE: http://www.c-sharpcorner.com/uploadfile/itsraj007/how-do-i-disable-the-close-x-button-in-a-windows-form-using-C-Sharp/
            // Disable the X button.
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        }

        /// <summary>
        /// Returns the Name selected from the text box.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedName()
        {
            return SelectedName;
        }
    }
}
