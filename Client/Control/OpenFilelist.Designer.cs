namespace SpreadsheetClient
{
    partial class OpenFileList
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FIleListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.NewNameLabel = new System.Windows.Forms.Label();
            this.CreateSpreadsheetTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FIleListBox
            // 
            this.FIleListBox.FormattingEnabled = true;
            this.FIleListBox.ItemHeight = 16;
            this.FIleListBox.Location = new System.Drawing.Point(0, 0);
            this.FIleListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FIleListBox.Name = "FIleListBox";
            this.FIleListBox.ScrollAlwaysVisible = true;
            this.FIleListBox.Size = new System.Drawing.Size(944, 436);
            this.FIleListBox.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(945, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(17, 443);
            this.OpenFileButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(157, 28);
            this.OpenFileButton.TabIndex = 2;
            this.OpenFileButton.Text = "Open Spreadsheet";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // NewNameLabel
            // 
            this.NewNameLabel.AutoSize = true;
            this.NewNameLabel.Location = new System.Drawing.Point(258, 449);
            this.NewNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NewNameLabel.Name = "NewNameLabel";
            this.NewNameLabel.Size = new System.Drawing.Size(226, 16);
            this.NewNameLabel.TabIndex = 3;
            this.NewNameLabel.Text = "Enter New Spreadsheet Name Here:";
            // 
            // CreateSpreadsheetTextBox
            // 
            this.CreateSpreadsheetTextBox.Location = new System.Drawing.Point(492, 447);
            this.CreateSpreadsheetTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CreateSpreadsheetTextBox.Name = "CreateSpreadsheetTextBox";
            this.CreateSpreadsheetTextBox.Size = new System.Drawing.Size(436, 22);
            this.CreateSpreadsheetTextBox.TabIndex = 4;
            // 
            // OpenFileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 486);
            this.Controls.Add(this.CreateSpreadsheetTextBox);
            this.Controls.Add(this.NewNameLabel);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.FIleListBox);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "OpenFileList";
            this.Text = "Select Or Create A File";
            this.Load += new System.EventHandler(this.OpenFileList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox FIleListBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.Label NewNameLabel;
        private System.Windows.Forms.TextBox CreateSpreadsheetTextBox;
    }
}

