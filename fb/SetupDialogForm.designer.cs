namespace ASCOM.fb
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.lb_title1 = new System.Windows.Forms.Label();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.lb_IP = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.cmdReadDevices = new System.Windows.Forms.Button();
            this.lb_User = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPasswd = new System.Windows.Forms.TextBox();
            this.cklbDevices = new System.Windows.Forms.CheckedListBox();
            this.lb_title2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(303, 369);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(73, 20);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(303, 395);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Abbrechen";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lb_title1
            // 
            this.lb_title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_title1.Location = new System.Drawing.Point(42, 9);
            this.lb_title1.Name = "lb_title1";
            this.lb_title1.Size = new System.Drawing.Size(237, 24);
            this.lb_title1.TabIndex = 2;
            this.lb_title1.Text = "Verbindung zur Fritzbox:";
            this.lb_title1.Click += new System.EventHandler(this.label1_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.fb.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(330, 9);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // lb_IP
            // 
            this.lb_IP.AutoSize = true;
            this.lb_IP.Location = new System.Drawing.Point(17, 46);
            this.lb_IP.Name = "lb_IP";
            this.lb_IP.Size = new System.Drawing.Size(100, 13);
            this.lb_IP.TabIndex = 5;
            this.lb_IP.Text = "IP Adresse Fritzbox:";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(174, 399);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(69, 17);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // tbIP
            // 
            this.tbIP.AutoCompleteCustomSource.AddRange(new string[] {
            "http://"});
            this.tbIP.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbIP.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbIP.CausesValidation = false;
            this.tbIP.Location = new System.Drawing.Point(123, 43);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(156, 20);
            this.tbIP.TabIndex = 2;
            this.tbIP.Click += new System.EventHandler(this.TB_IP_Clicked);
            this.tbIP.TextChanged += new System.EventHandler(this.TB_IP_TextChanged);
            // 
            // cmdReadDevices
            // 
            this.cmdReadDevices.Location = new System.Drawing.Point(15, 395);
            this.cmdReadDevices.Name = "cmdReadDevices";
            this.cmdReadDevices.Size = new System.Drawing.Size(136, 23);
            this.cmdReadDevices.TabIndex = 5;
            this.cmdReadDevices.Text = "Geräte neu einlesen";
            this.cmdReadDevices.UseVisualStyleBackColor = true;
            this.cmdReadDevices.Click += new System.EventHandler(this.FB_einlesen_Click);
            // 
            // lb_User
            // 
            this.lb_User.AutoSize = true;
            this.lb_User.Location = new System.Drawing.Point(17, 75);
            this.lb_User.Name = "lb_User";
            this.lb_User.Size = new System.Drawing.Size(52, 13);
            this.lb_User.TabIndex = 12;
            this.lb_User.Text = "Benutzer:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Passwort:";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(123, 72);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(156, 20);
            this.tbUser.TabIndex = 3;
            // 
            // tbPasswd
            // 
            this.tbPasswd.Location = new System.Drawing.Point(123, 101);
            this.tbPasswd.Name = "tbPasswd";
            this.tbPasswd.PasswordChar = '*';
            this.tbPasswd.Size = new System.Drawing.Size(156, 20);
            this.tbPasswd.TabIndex = 4;
            this.tbPasswd.UseSystemPasswordChar = true;
            // 
            // cklbDevices
            // 
            this.cklbDevices.FormattingEnabled = true;
            this.cklbDevices.Location = new System.Drawing.Point(20, 175);
            this.cklbDevices.Name = "cklbDevices";
            this.cklbDevices.Size = new System.Drawing.Size(259, 199);
            this.cklbDevices.TabIndex = 6;
            // 
            // lb_title2
            // 
            this.lb_title2.AutoSize = true;
            this.lb_title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_title2.Location = new System.Drawing.Point(42, 142);
            this.lb_title2.Name = "lb_title2";
            this.lb_title2.Size = new System.Drawing.Size(169, 18);
            this.lb_title2.TabIndex = 14;
            this.lb_title2.Text = "Auswahl Steckdosen:";
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 436);
            this.Controls.Add(this.lb_title2);
            this.Controls.Add(this.cklbDevices);
            this.Controls.Add(this.tbPasswd);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lb_User);
            this.Controls.Add(this.cmdReadDevices);
            this.Controls.Add(this.tbIP);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.lb_IP);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.lb_title1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fritzbox Switch Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label lb_title1;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label lb_IP;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.Button cmdReadDevices;
        private System.Windows.Forms.Label lb_User;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPasswd;
        private System.Windows.Forms.CheckedListBox cklbDevices;
        private System.Windows.Forms.Label lb_title2;
    }
}