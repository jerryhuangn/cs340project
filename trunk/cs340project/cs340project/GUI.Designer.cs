namespace cs340project
{
    partial class GUI
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
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonBroadcast = new System.Windows.Forms.Button();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.textBoxConsole = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 241);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.clickAdd);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(93, 241);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.clickRemove);
            // 
            // buttonBroadcast
            // 
            this.buttonBroadcast.Location = new System.Drawing.Point(174, 241);
            this.buttonBroadcast.Name = "buttonBroadcast";
            this.buttonBroadcast.Size = new System.Drawing.Size(75, 23);
            this.buttonBroadcast.TabIndex = 2;
            this.buttonBroadcast.Text = "Broadcast";
            this.buttonBroadcast.UseVisualStyleBackColor = true;
            this.buttonBroadcast.Click += new System.EventHandler(this.clickBroadcast);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(255, 241);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 3;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.clickSend);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(13, 270);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(317, 20);
            this.textBoxMessage.TabIndex = 4;
            this.textBoxMessage.Text = "type message here";
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.Location = new System.Drawing.Point(12, 13);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConsole.Size = new System.Drawing.Size(318, 222);
            this.textBoxConsole.TabIndex = 5;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 303);
            this.Controls.Add(this.textBoxConsole);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonBroadcast);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Name = "GUI";
            this.Text = "GUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonBroadcast;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.TextBox textBoxConsole;
    }
}