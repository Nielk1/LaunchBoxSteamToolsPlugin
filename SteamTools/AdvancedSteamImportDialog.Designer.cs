namespace SteamTools
{
    partial class AdvancedSteamImportDialog
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
            this.cbPlatforms = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.lvGames = new System.Windows.Forms.ListView();
            this.chTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // cbPlatforms
            // 
            this.cbPlatforms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPlatforms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlatforms.FormattingEnabled = true;
            this.cbPlatforms.Location = new System.Drawing.Point(97, 332);
            this.cbPlatforms.Name = "cbPlatforms";
            this.cbPlatforms.Size = new System.Drawing.Size(500, 21);
            this.cbPlatforms.Sorted = true;
            this.cbPlatforms.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 335);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Target Platform";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(585, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(12, 12);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(585, 23);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "Scan Steam";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lvGames
            // 
            this.lvGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvGames.CheckBoxes = true;
            this.lvGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTitle,
            this.chID,
            this.chType});
            this.lvGames.FullRowSelect = true;
            this.lvGames.GridLines = true;
            this.lvGames.Location = new System.Drawing.Point(12, 41);
            this.lvGames.Name = "lvGames";
            this.lvGames.OwnerDraw = true;
            this.lvGames.Size = new System.Drawing.Size(585, 285);
            this.lvGames.TabIndex = 4;
            this.lvGames.UseCompatibleStateImageBehavior = false;
            this.lvGames.View = System.Windows.Forms.View.Details;
            this.lvGames.VirtualMode = true;
            this.lvGames.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGames_ColumnClick);
            this.lvGames.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.lvGames_DrawColumnHeader);
            this.lvGames.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.lvGames_DrawItem);
            this.lvGames.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.lvGames_DrawSubItem);
            this.lvGames.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvGames_RetrieveVirtualItem);
            this.lvGames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvGames_MouseClick);
            this.lvGames.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvGames_MouseDoubleClick);
            // 
            // chTitle
            // 
            this.chTitle.Text = "Title";
            this.chTitle.Width = 250;
            // 
            // chID
            // 
            this.chID.Text = "AppID";
            this.chID.Width = 150;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 100;
            // 
            // AdvancedSteamImportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 394);
            this.Controls.Add(this.lvGames);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPlatforms);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedSteamImportDialog";
            this.Text = "Advanced Steam Import";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbPlatforms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.ListView lvGames;
        private System.Windows.Forms.ColumnHeader chTitle;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chID;
    }
}