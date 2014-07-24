namespace SpreadsheetClient
{
    partial class SpreadsheetWindow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveOption = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenOption = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseOption = new System.Windows.Forms.ToolStripMenuItem();
            this.CellNameLabel = new System.Windows.Forms.Label();
            this.CellNameTextBox = new System.Windows.Forms.TextBox();
            this.CellValueLabel = new System.Windows.Forms.Label();
            this.CellValueTextBox = new System.Windows.Forms.TextBox();
            this.CellContentsLabel = new System.Windows.Forms.Label();
            this.CellContentsTextBox = new System.Windows.Forms.TextBox();
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.UndoButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(877, 42);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileMenu
            // 
            this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenOption,
            this.SaveOption,
            this.CloseOption});
            this.FileMenu.Font = new System.Drawing.Font("Stencil", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileMenu.Name = "FileMenu";
            this.FileMenu.Size = new System.Drawing.Size(86, 38);
            this.FileMenu.Text = "File";
            // 
            // SaveOption
            // 
            this.SaveOption.Name = "SaveOption";
            this.SaveOption.Size = new System.Drawing.Size(173, 38);
            this.SaveOption.Text = "Save";
            this.SaveOption.Click += new System.EventHandler(this.SaveOption_Click);
            // 
            // OpenOption
            // 
            this.OpenOption.Name = "OpenOption";
            this.OpenOption.Size = new System.Drawing.Size(173, 38);
            this.OpenOption.Text = "Open";
            this.OpenOption.Click += new System.EventHandler(this.OpenOption_Click);
            // 
            // CloseOption
            // 
            this.CloseOption.Name = "CloseOption";
            this.CloseOption.Size = new System.Drawing.Size(173, 38);
            this.CloseOption.Text = "Close";
            this.CloseOption.Click += new System.EventHandler(this.CloseOption_Click);
            // 
            // CellNameLabel
            // 
            this.CellNameLabel.AutoSize = true;
            this.CellNameLabel.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellNameLabel.Location = new System.Drawing.Point(12, 33);
            this.CellNameLabel.Name = "CellNameLabel";
            this.CellNameLabel.Size = new System.Drawing.Size(86, 19);
            this.CellNameLabel.TabIndex = 1;
            this.CellNameLabel.Text = "Cell Name";
            // 
            // CellNameTextBox
            // 
            this.CellNameTextBox.Enabled = false;
            this.CellNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellNameTextBox.Location = new System.Drawing.Point(96, 29);
            this.CellNameTextBox.Name = "CellNameTextBox";
            this.CellNameTextBox.ReadOnly = true;
            this.CellNameTextBox.Size = new System.Drawing.Size(53, 26);
            this.CellNameTextBox.TabIndex = 2;
            // 
            // CellValueLabel
            // 
            this.CellValueLabel.AutoSize = true;
            this.CellValueLabel.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellValueLabel.Location = new System.Drawing.Point(171, 33);
            this.CellValueLabel.Name = "CellValueLabel";
            this.CellValueLabel.Size = new System.Drawing.Size(51, 19);
            this.CellValueLabel.TabIndex = 3;
            this.CellValueLabel.Text = "Value";
            // 
            // CellValueTextBox
            // 
            this.CellValueTextBox.Enabled = false;
            this.CellValueTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellValueTextBox.Location = new System.Drawing.Point(218, 29);
            this.CellValueTextBox.Name = "CellValueTextBox";
            this.CellValueTextBox.ReadOnly = true;
            this.CellValueTextBox.Size = new System.Drawing.Size(104, 26);
            this.CellValueTextBox.TabIndex = 4;
            // 
            // CellContentsLabel
            // 
            this.CellContentsLabel.AutoSize = true;
            this.CellContentsLabel.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellContentsLabel.Location = new System.Drawing.Point(328, 33);
            this.CellContentsLabel.Name = "CellContentsLabel";
            this.CellContentsLabel.Size = new System.Drawing.Size(80, 19);
            this.CellContentsLabel.TabIndex = 5;
            this.CellContentsLabel.Text = "Contents";
            // 
            // CellContentsTextBox
            // 
            this.CellContentsTextBox.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellContentsTextBox.Location = new System.Drawing.Point(414, 30);
            this.CellContentsTextBox.Name = "CellContentsTextBox";
            this.CellContentsTextBox.Size = new System.Drawing.Size(407, 26);
            this.CellContentsTextBox.TabIndex = 6;
            this.CellContentsTextBox.TextChanged += new System.EventHandler(this.CellContentsTextBox_TextChanged);
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 68);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.spreadsheetPanel1.Size = new System.Drawing.Size(877, 340);
            this.spreadsheetPanel1.TabIndex = 8;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "ss";
            this.saveFileDialog1.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files (*.*)|*.*";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "ss";
            this.openFileDialog1.Filter = "Spreadsheet Files (*.ss)|*.ss|All Files (*.*)|*.*";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(98, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(51, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Help";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UndoButton.Location = new System.Drawing.Point(332, 3);
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(75, 23);
            this.UndoButton.TabIndex = 10;
            this.UndoButton.Text = "Undo";
            this.UndoButton.UseVisualStyleBackColor = true;
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // SpreadsheetWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(877, 408);
            this.Controls.Add(this.UndoButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.CellContentsTextBox);
            this.Controls.Add(this.CellContentsLabel);
            this.Controls.Add(this.CellValueTextBox);
            this.Controls.Add(this.CellValueLabel);
            this.Controls.Add(this.CellNameTextBox);
            this.Controls.Add(this.CellNameLabel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Spreadsheet";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileMenu;
        private System.Windows.Forms.ToolStripMenuItem SaveOption;
        private System.Windows.Forms.ToolStripMenuItem OpenOption;
        private System.Windows.Forms.ToolStripMenuItem CloseOption;
        private System.Windows.Forms.Label CellNameLabel;
        private System.Windows.Forms.TextBox CellNameTextBox;
        private System.Windows.Forms.Label CellValueLabel;
        public System.Windows.Forms.TextBox CellValueTextBox;
        private System.Windows.Forms.Label CellContentsLabel;
        private System.Windows.Forms.TextBox CellContentsTextBox;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        //private Microsoft.VisualBasic.PowerPacks.LineShape DividingLine;
        public SS.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button UndoButton;

    }
}

