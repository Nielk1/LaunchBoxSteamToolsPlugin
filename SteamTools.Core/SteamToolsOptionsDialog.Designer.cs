namespace SteamTools
{
    partial class SteamToolsOptionsDialog
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
            this.cbHide = new System.Windows.Forms.CheckBox();
            this.cbShow = new System.Windows.Forms.CheckBox();
            this.nudPollRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtVersion = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPollRate)).BeginInit();
            this.SuspendLayout();
            // 
            // cbHide
            // 
            this.cbHide.AutoSize = true;
            this.cbHide.Location = new System.Drawing.Point(15, 33);
            this.cbHide.Name = "cbHide";
            this.cbHide.Size = new System.Drawing.Size(164, 17);
            this.cbHide.TabIndex = 2;
            this.cbHide.Text = "Auto Hide Uninstalled Games";
            this.cbHide.UseVisualStyleBackColor = true;
            // 
            // cbShow
            // 
            this.cbShow.AutoSize = true;
            this.cbShow.Location = new System.Drawing.Point(15, 56);
            this.cbShow.Name = "cbShow";
            this.cbShow.Size = new System.Drawing.Size(168, 17);
            this.cbShow.TabIndex = 3;
            this.cbShow.Text = "Auto Un-Hide Installed Games";
            this.cbShow.UseVisualStyleBackColor = true;
            // 
            // nudPollRate
            // 
            this.nudPollRate.Location = new System.Drawing.Point(118, 7);
            this.nudPollRate.Maximum = new decimal(new int[] {
            86400,
            0,
            0,
            0});
            this.nudPollRate.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudPollRate.Name = "nudPollRate";
            this.nudPollRate.Size = new System.Drawing.Size(65, 20);
            this.nudPollRate.TabIndex = 1;
            this.nudPollRate.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Steam Re-Poll Rate";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(31, 112);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(112, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel ";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtVersion
            // 
            this.txtVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVersion.Location = new System.Drawing.Point(12, 79);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.Size = new System.Drawing.Size(171, 20);
            this.txtVersion.TabIndex = 6;
            // 
            // SteamToolsOptionsDialog
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(195, 147);
            this.Controls.Add(this.txtVersion);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudPollRate);
            this.Controls.Add(this.cbShow);
            this.Controls.Add(this.cbHide);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(211, 185);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(211, 185);
            this.Name = "SteamToolsOptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Steam Tools Options";
            ((System.ComponentModel.ISupportInitialize)(this.nudPollRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbHide;
        private System.Windows.Forms.CheckBox cbShow;
        private System.Windows.Forms.NumericUpDown nudPollRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtVersion;
    }
}