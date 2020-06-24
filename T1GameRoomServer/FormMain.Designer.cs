namespace T1GameRoomServer
{
    partial class FormMain
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
            this.Console = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.BackColor = System.Drawing.Color.Black;
            this.Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Console.ForeColor = System.Drawing.Color.White;
            this.Console.Location = new System.Drawing.Point(0, 0);
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.Size = new System.Drawing.Size(800, 450);
            this.Console.TabIndex = 9;
            this.Console.Text = "";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Console);
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SKTB T1 Game Room Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox Console;
    }
}

