namespace cs340project
{
    partial class NetworkGUI
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
            this.listenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createLocalNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshHypeerwebDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtDump = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listenToolStripMenuItem,
            this.nodesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(583, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // listenToolStripMenuItem
            // 
            this.listenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem});
            this.listenToolStripMenuItem.Name = "listenToolStripMenuItem";
            this.listenToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.listenToolStripMenuItem.Text = "Network";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
            this.connectToolStripMenuItem.Text = "Connect && Create Local Node";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // nodesToolStripMenuItem
            // 
            this.nodesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createLocalNodeToolStripMenuItem,
            this.refreshHypeerwebDumpToolStripMenuItem});
            this.nodesToolStripMenuItem.Name = "nodesToolStripMenuItem";
            this.nodesToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.nodesToolStripMenuItem.Text = "Nodes";
            // 
            // createLocalNodeToolStripMenuItem
            // 
            this.createLocalNodeToolStripMenuItem.Name = "createLocalNodeToolStripMenuItem";
            this.createLocalNodeToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.createLocalNodeToolStripMenuItem.Text = "Create local node";
            this.createLocalNodeToolStripMenuItem.Click += new System.EventHandler(this.createLocalNodeToolStripMenuItem_Click);
            // 
            // refreshHypeerwebDumpToolStripMenuItem
            // 
            this.refreshHypeerwebDumpToolStripMenuItem.Name = "refreshHypeerwebDumpToolStripMenuItem";
            this.refreshHypeerwebDumpToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.refreshHypeerwebDumpToolStripMenuItem.Text = "Refresh hypeerweb dump";
            this.refreshHypeerwebDumpToolStripMenuItem.Click += new System.EventHandler(this.refreshHypeerwebDumpToolStripMenuItem_Click);
            // 
            // txtDump
            // 
            this.txtDump.Location = new System.Drawing.Point(12, 126);
            this.txtDump.Multiline = true;
            this.txtDump.Name = "txtDump";
            this.txtDump.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDump.Size = new System.Drawing.Size(559, 378);
            this.txtDump.TabIndex = 1;
            // 
            // NetworkGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 516);
            this.Controls.Add(this.txtDump);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "NetworkGUI";
            this.Text = "NetworkGUI";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem listenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createLocalNodeToolStripMenuItem;
        private System.Windows.Forms.TextBox txtDump;
        private System.Windows.Forms.ToolStripMenuItem refreshHypeerwebDumpToolStripMenuItem;
    }
}