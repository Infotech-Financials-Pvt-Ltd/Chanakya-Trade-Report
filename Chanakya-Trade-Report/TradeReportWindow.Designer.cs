namespace Chanakya_Trade_Report
{
    partial class TradeReportWindow
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TotalBuyValueLabel = new System.Windows.Forms.Label();
            this.TotalSellValueLabel = new System.Windows.Forms.Label();
            this.TotalBuyValueTextBox = new System.Windows.Forms.TextBox();
            this.TotalSellValueTextBox = new System.Windows.Forms.TextBox();
            this.TotalTradeValueLabel = new System.Windows.Forms.Label();
            this.TotalTradeValueTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TotalTradesTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshButton = new System.Windows.Forms.Button();
            this.bindingSource_main = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.lastUpdatedTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.countValue = new System.Windows.Forms.Label();
            this.statusDisplay = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(12, 106);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(836, 384);
            this.dataGridView1.TabIndex = 0;
            // 
            // TotalBuyValueLabel
            // 
            this.TotalBuyValueLabel.AutoSize = true;
            this.TotalBuyValueLabel.Location = new System.Drawing.Point(13, 39);
            this.TotalBuyValueLabel.Name = "TotalBuyValueLabel";
            this.TotalBuyValueLabel.Size = new System.Drawing.Size(82, 13);
            this.TotalBuyValueLabel.TabIndex = 1;
            this.TotalBuyValueLabel.Text = "Total Buy Value";
            // 
            // TotalSellValueLabel
            // 
            this.TotalSellValueLabel.AutoSize = true;
            this.TotalSellValueLabel.Location = new System.Drawing.Point(12, 72);
            this.TotalSellValueLabel.Name = "TotalSellValueLabel";
            this.TotalSellValueLabel.Size = new System.Drawing.Size(81, 13);
            this.TotalSellValueLabel.TabIndex = 2;
            this.TotalSellValueLabel.Text = "Total Sell Value";
            // 
            // TotalBuyValueTextBox
            // 
            this.TotalBuyValueTextBox.Location = new System.Drawing.Point(101, 36);
            this.TotalBuyValueTextBox.Name = "TotalBuyValueTextBox";
            this.TotalBuyValueTextBox.ReadOnly = true;
            this.TotalBuyValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.TotalBuyValueTextBox.TabIndex = 3;
            // 
            // TotalSellValueTextBox
            // 
            this.TotalSellValueTextBox.Location = new System.Drawing.Point(101, 69);
            this.TotalSellValueTextBox.Name = "TotalSellValueTextBox";
            this.TotalSellValueTextBox.ReadOnly = true;
            this.TotalSellValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.TotalSellValueTextBox.TabIndex = 4;
            // 
            // TotalTradeValueLabel
            // 
            this.TotalTradeValueLabel.AutoSize = true;
            this.TotalTradeValueLabel.Location = new System.Drawing.Point(212, 39);
            this.TotalTradeValueLabel.Name = "TotalTradeValueLabel";
            this.TotalTradeValueLabel.Size = new System.Drawing.Size(92, 13);
            this.TotalTradeValueLabel.TabIndex = 5;
            this.TotalTradeValueLabel.Text = "Total Trade Value";
            // 
            // TotalTradeValueTextBox
            // 
            this.TotalTradeValueTextBox.Location = new System.Drawing.Point(310, 36);
            this.TotalTradeValueTextBox.Name = "TotalTradeValueTextBox";
            this.TotalTradeValueTextBox.ReadOnly = true;
            this.TotalTradeValueTextBox.Size = new System.Drawing.Size(100, 20);
            this.TotalTradeValueTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Total Trades";
            // 
            // TotalTradesTextBox
            // 
            this.TotalTradesTextBox.Location = new System.Drawing.Point(310, 69);
            this.TotalTradesTextBox.Name = "TotalTradesTextBox";
            this.TotalTradesTextBox.ReadOnly = true;
            this.TotalTradesTextBox.Size = new System.Drawing.Size(100, 20);
            this.TotalTradesTextBox.TabIndex = 8;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(860, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.toolStripSeparator2});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(109, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItem1.Text = "Logout";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(109, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(688, 67);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 10;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(422, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Last Updated : ";
            // 
            // lastUpdatedTime
            // 
            this.lastUpdatedTime.AutoSize = true;
            this.lastUpdatedTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lastUpdatedTime.Location = new System.Drawing.Point(518, 72);
            this.lastUpdatedTime.MaximumSize = new System.Drawing.Size(135, 15);
            this.lastUpdatedTime.MinimumSize = new System.Drawing.Size(135, 15);
            this.lastUpdatedTime.Name = "lastUpdatedTime";
            this.lastUpdatedTime.Size = new System.Drawing.Size(135, 15);
            this.lastUpdatedTime.TabIndex = 12;
            this.lastUpdatedTime.Text = "                                          ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(422, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Total Count : ";
            // 
            // countValue
            // 
            this.countValue.AutoSize = true;
            this.countValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.countValue.Location = new System.Drawing.Point(518, 39);
            this.countValue.MaximumSize = new System.Drawing.Size(135, 15);
            this.countValue.MinimumSize = new System.Drawing.Size(135, 15);
            this.countValue.Name = "countValue";
            this.countValue.Size = new System.Drawing.Size(135, 15);
            this.countValue.TabIndex = 14;
            this.countValue.Text = "                                          ";
            // 
            // statusDisplay
            // 
            this.statusDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.statusDisplay.AutoSize = true;
            this.statusDisplay.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.statusDisplay.Location = new System.Drawing.Point(748, 499);
            this.statusDisplay.MinimumSize = new System.Drawing.Size(100, 15);
            this.statusDisplay.Name = "statusDisplay";
            this.statusDisplay.Size = new System.Drawing.Size(100, 15);
            this.statusDisplay.TabIndex = 15;
            // 
            // TradeReportWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 523);
            this.Controls.Add(this.statusDisplay);
            this.Controls.Add(this.countValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lastUpdatedTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.TotalTradesTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TotalTradeValueTextBox);
            this.Controls.Add(this.TotalTradeValueLabel);
            this.Controls.Add(this.TotalSellValueTextBox);
            this.Controls.Add(this.TotalBuyValueTextBox);
            this.Controls.Add(this.TotalSellValueLabel);
            this.Controls.Add(this.TotalBuyValueLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TradeReportWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TradeReportWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TradeReportWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label TotalBuyValueLabel;
        private System.Windows.Forms.Label TotalSellValueLabel;
        private System.Windows.Forms.TextBox TotalBuyValueTextBox;
        private System.Windows.Forms.TextBox TotalSellValueTextBox;
        private System.Windows.Forms.Label TotalTradeValueLabel;
        private System.Windows.Forms.TextBox TotalTradeValueTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TotalTradesTextBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource_main;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lastUpdatedTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label countValue;
        private System.Windows.Forms.Label statusDisplay;
    }
}