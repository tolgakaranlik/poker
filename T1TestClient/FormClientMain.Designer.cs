namespace T1TestClient
{
    partial class FormClientMain
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtRoomID = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtSessionToken = new System.Windows.Forms.TextBox();
            this.txtRaiseAmount = new System.Windows.Forms.TextBox();
            this.button10 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAutoResponse = new System.Windows.Forms.ComboBox();
            this.txtAmountToSit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAutoTopOff = new System.Windows.Forms.CheckBox();
            this.chkAutoRebuy = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect To:";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(118, 13);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(294, 20);
            this.txtServer.TabIndex = 10;
            // 
            // Console
            // 
            this.Console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Console.BackColor = System.Drawing.Color.Black;
            this.Console.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Console.ForeColor = System.Drawing.Color.White;
            this.Console.Location = new System.Drawing.Point(118, 101);
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.Size = new System.Drawing.Size(495, 296);
            this.Console.TabIndex = 2;
            this.Console.Text = "";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(420, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Join To:";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtRoomID
            // 
            this.txtRoomID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRoomID.Location = new System.Drawing.Point(521, 13);
            this.txtRoomID.Name = "txtRoomID";
            this.txtRoomID.Size = new System.Drawing.Size(91, 20);
            this.txtRoomID.TabIndex = 20;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 101);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Sit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 130);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "Stand Up";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 159);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(95, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "Leave Room";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.Location = new System.Drawing.Point(12, 200);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(95, 23);
            this.button6.TabIndex = 8;
            this.button6.Text = "Call";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(12, 229);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(95, 23);
            this.button7.TabIndex = 9;
            this.button7.Text = "Fold";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Enabled = false;
            this.button8.Location = new System.Drawing.Point(12, 316);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(95, 23);
            this.button8.TabIndex = 10;
            this.button8.Text = "Raise...";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Enabled = false;
            this.button9.Location = new System.Drawing.Point(12, 258);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(95, 23);
            this.button9.TabIndex = 11;
            this.button9.Text = "All In";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserID.Location = new System.Drawing.Point(118, 39);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(294, 20);
            this.txtUserID.TabIndex = 30;
            // 
            // txtSessionToken
            // 
            this.txtSessionToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSessionToken.Location = new System.Drawing.Point(118, 65);
            this.txtSessionToken.Name = "txtSessionToken";
            this.txtSessionToken.Size = new System.Drawing.Size(294, 20);
            this.txtSessionToken.TabIndex = 40;
            // 
            // txtRaiseAmount
            // 
            this.txtRaiseAmount.Location = new System.Drawing.Point(12, 345);
            this.txtRaiseAmount.Name = "txtRaiseAmount";
            this.txtRaiseAmount.Size = new System.Drawing.Size(95, 20);
            this.txtRaiseAmount.TabIndex = 14;
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button10.Location = new System.Drawing.Point(420, 64);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(192, 22);
            this.button10.TabIndex = 15;
            this.button10.Text = "Load Test Script...";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Auto Response:";
            // 
            // cmbAutoResponse
            // 
            this.cmbAutoResponse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAutoResponse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutoResponse.FormattingEnabled = true;
            this.cmbAutoResponse.Items.AddRange(new object[] {
            "None",
            "Call",
            "Random"});
            this.cmbAutoResponse.Location = new System.Drawing.Point(204, 407);
            this.cmbAutoResponse.Name = "cmbAutoResponse";
            this.cmbAutoResponse.Size = new System.Drawing.Size(408, 21);
            this.cmbAutoResponse.TabIndex = 17;
            // 
            // txtAmountToSit
            // 
            this.txtAmountToSit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAmountToSit.Location = new System.Drawing.Point(521, 39);
            this.txtAmountToSit.Name = "txtAmountToSit";
            this.txtAmountToSit.Size = new System.Drawing.Size(91, 20);
            this.txtAmountToSit.TabIndex = 60;
            this.txtAmountToSit.Text = "5000";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(418, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Table Small:";
            // 
            // chkAutoTopOff
            // 
            this.chkAutoTopOff.AutoSize = true;
            this.chkAutoTopOff.Location = new System.Drawing.Point(12, 67);
            this.chkAutoTopOff.Name = "chkAutoTopOff";
            this.chkAutoTopOff.Size = new System.Drawing.Size(87, 17);
            this.chkAutoTopOff.TabIndex = 20;
            this.chkAutoTopOff.Text = "Auto Top Off";
            this.chkAutoTopOff.UseVisualStyleBackColor = true;
            // 
            // chkAutoRebuy
            // 
            this.chkAutoRebuy.AutoSize = true;
            this.chkAutoRebuy.Location = new System.Drawing.Point(12, 41);
            this.chkAutoRebuy.Name = "chkAutoRebuy";
            this.chkAutoRebuy.Size = new System.Drawing.Size(82, 17);
            this.chkAutoRebuy.TabIndex = 21;
            this.chkAutoRebuy.Text = "Auto Rebuy";
            this.chkAutoRebuy.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label3.Location = new System.Drawing.Point(9, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 61;
            this.label3.Text = "v0.21";
            // 
            // FormClientMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkAutoRebuy);
            this.Controls.Add(this.chkAutoTopOff);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAmountToSit);
            this.Controls.Add(this.cmbAutoResponse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.txtRaiseAmount);
            this.Controls.Add(this.txtSessionToken);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtRoomID);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.button1);
            this.Location = new System.Drawing.Point(800, 100);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "FormClientMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "T1 Test Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.RichTextBox Console;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtRoomID;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtSessionToken;
        private System.Windows.Forms.TextBox txtRaiseAmount;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAutoResponse;
        private System.Windows.Forms.TextBox txtAmountToSit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAutoTopOff;
        private System.Windows.Forms.CheckBox chkAutoRebuy;
        private System.Windows.Forms.Label label3;
    }
}

