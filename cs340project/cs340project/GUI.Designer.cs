namespace cs340project
{
    /// <summary>
    /// GUI class
    /// </summary>
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
            this.textBoxDisplay = new System.Windows.Forms.TextBox();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabDebugConsole = new System.Windows.Forms.TabPage();
            this.TabTextView = new System.Windows.Forms.TabPage();
            this.SplitMain = new System.Windows.Forms.SplitContainer();
            this.SplitDisplay = new System.Windows.Forms.SplitContainer();
            this.TbNodeInfo = new System.Windows.Forms.TextBox();
            this.SplitLower = new System.Windows.Forms.SplitContainer();
            this.BtAddNodeFromNode = new System.Windows.Forms.Button();
            this.BtRemoveThisNode = new System.Windows.Forms.Button();
            this.CbListOfNodes = new System.Windows.Forms.ComboBox();
            this.TcNodeTabs = new System.Windows.Forms.TabControl();
            this.TpThisNode = new System.Windows.Forms.TabPage();
            this.TpFromThisNode = new System.Windows.Forms.TabPage();
            this.lblToNode = new System.Windows.Forms.Label();
            this.BtRmove = new System.Windows.Forms.Button();
            this.tbMessageFromNode = new System.Windows.Forms.TextBox();
            this.btSendMessageFromNode = new System.Windows.Forms.Button();
            this.btBroadcastMessageFromNode = new System.Windows.Forms.Button();
            this.tbBroadCastFromNode = new System.Windows.Forms.TextBox();
            this.cbAckBroadcastFrom = new System.Windows.Forms.CheckBox();
            this.Tabs.SuspendLayout();
            this.TabDebugConsole.SuspendLayout();
            this.TabTextView.SuspendLayout();
            this.SplitMain.Panel1.SuspendLayout();
            this.SplitMain.Panel2.SuspendLayout();
            this.SplitMain.SuspendLayout();
            this.SplitDisplay.Panel2.SuspendLayout();
            this.SplitDisplay.SuspendLayout();
            this.SplitLower.Panel1.SuspendLayout();
            this.SplitLower.Panel2.SuspendLayout();
            this.SplitLower.SuspendLayout();
            this.TcNodeTabs.SuspendLayout();
            this.TpThisNode.SuspendLayout();
            this.TpFromThisNode.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAdd
            // 
            this.buttonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAdd.Location = new System.Drawing.Point(16, 8);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.clickAdd);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRemove.Location = new System.Drawing.Point(97, 8);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.clickRemove);
            // 
            // buttonBroadcast
            // 
            this.buttonBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonBroadcast.Location = new System.Drawing.Point(178, 8);
            this.buttonBroadcast.Name = "buttonBroadcast";
            this.buttonBroadcast.Size = new System.Drawing.Size(75, 23);
            this.buttonBroadcast.TabIndex = 2;
            this.buttonBroadcast.Text = "Broadcast";
            this.buttonBroadcast.UseVisualStyleBackColor = true;
            this.buttonBroadcast.Click += new System.EventHandler(this.clickBroadcast);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSend.Location = new System.Drawing.Point(259, 8);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 3;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.clickSend);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxMessage.Location = new System.Drawing.Point(17, 37);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(317, 20);
            this.textBoxMessage.TabIndex = 4;
            this.textBoxMessage.Text = "type message here";
            // 
            // textBoxConsole
            // 
            this.textBoxConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxConsole.Location = new System.Drawing.Point(3, 3);
            this.textBoxConsole.Multiline = true;
            this.textBoxConsole.Name = "textBoxConsole";
            this.textBoxConsole.ReadOnly = true;
            this.textBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxConsole.Size = new System.Drawing.Size(699, 204);
            this.textBoxConsole.TabIndex = 5;
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDisplay.Location = new System.Drawing.Point(3, 3);
            this.textBoxDisplay.Multiline = true;
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDisplay.Size = new System.Drawing.Size(655, 204);
            this.textBoxDisplay.TabIndex = 6;
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabDebugConsole);
            this.Tabs.Controls.Add(this.TabTextView);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(713, 236);
            this.Tabs.TabIndex = 7;
            // 
            // TabDebugConsole
            // 
            this.TabDebugConsole.Controls.Add(this.textBoxConsole);
            this.TabDebugConsole.Location = new System.Drawing.Point(4, 22);
            this.TabDebugConsole.Name = "TabDebugConsole";
            this.TabDebugConsole.Padding = new System.Windows.Forms.Padding(3);
            this.TabDebugConsole.Size = new System.Drawing.Size(705, 210);
            this.TabDebugConsole.TabIndex = 0;
            this.TabDebugConsole.Text = "Output";
            this.TabDebugConsole.UseVisualStyleBackColor = true;
            // 
            // TabTextView
            // 
            this.TabTextView.Controls.Add(this.textBoxDisplay);
            this.TabTextView.Location = new System.Drawing.Point(4, 22);
            this.TabTextView.Name = "TabTextView";
            this.TabTextView.Padding = new System.Windows.Forms.Padding(3);
            this.TabTextView.Size = new System.Drawing.Size(661, 210);
            this.TabTextView.TabIndex = 1;
            this.TabTextView.Text = "Nodes String";
            this.TabTextView.UseVisualStyleBackColor = true;
            // 
            // SplitMain
            // 
            this.SplitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitMain.Location = new System.Drawing.Point(0, 0);
            this.SplitMain.Name = "SplitMain";
            this.SplitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitMain.Panel1
            // 
            this.SplitMain.Panel1.Controls.Add(this.SplitDisplay);
            // 
            // SplitMain.Panel2
            // 
            this.SplitMain.Panel2.Controls.Add(this.SplitLower);
            this.SplitMain.Size = new System.Drawing.Size(713, 613);
            this.SplitMain.SplitterDistance = 306;
            this.SplitMain.TabIndex = 8;
            // 
            // SplitDisplay
            // 
            this.SplitDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitDisplay.Location = new System.Drawing.Point(0, 0);
            this.SplitDisplay.Name = "SplitDisplay";
            // 
            // SplitDisplay.Panel1
            // 
            this.SplitDisplay.Panel1.AutoScroll = true;
            // 
            // SplitDisplay.Panel2
            // 
            this.SplitDisplay.Panel2.Controls.Add(this.TcNodeTabs);
            this.SplitDisplay.Panel2.Controls.Add(this.TbNodeInfo);
            this.SplitDisplay.Size = new System.Drawing.Size(713, 306);
            this.SplitDisplay.SplitterDistance = 437;
            this.SplitDisplay.TabIndex = 0;
            // 
            // TbNodeInfo
            // 
            this.TbNodeInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TbNodeInfo.Location = new System.Drawing.Point(0, 149);
            this.TbNodeInfo.Multiline = true;
            this.TbNodeInfo.Name = "TbNodeInfo";
            this.TbNodeInfo.ReadOnly = true;
            this.TbNodeInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbNodeInfo.Size = new System.Drawing.Size(272, 157);
            this.TbNodeInfo.TabIndex = 0;
            // 
            // SplitLower
            // 
            this.SplitLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitLower.IsSplitterFixed = true;
            this.SplitLower.Location = new System.Drawing.Point(0, 0);
            this.SplitLower.Name = "SplitLower";
            this.SplitLower.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitLower.Panel1
            // 
            this.SplitLower.Panel1.Controls.Add(this.Tabs);
            // 
            // SplitLower.Panel2
            // 
            this.SplitLower.Panel2.Controls.Add(this.buttonSend);
            this.SplitLower.Panel2.Controls.Add(this.buttonAdd);
            this.SplitLower.Panel2.Controls.Add(this.textBoxMessage);
            this.SplitLower.Panel2.Controls.Add(this.buttonRemove);
            this.SplitLower.Panel2.Controls.Add(this.buttonBroadcast);
            this.SplitLower.Size = new System.Drawing.Size(713, 303);
            this.SplitLower.SplitterDistance = 236;
            this.SplitLower.TabIndex = 0;
            // 
            // BtAddNodeFromNode
            // 
            this.BtAddNodeFromNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtAddNodeFromNode.Location = new System.Drawing.Point(6, 94);
            this.BtAddNodeFromNode.Name = "BtAddNodeFromNode";
            this.BtAddNodeFromNode.Size = new System.Drawing.Size(75, 23);
            this.BtAddNodeFromNode.TabIndex = 1;
            this.BtAddNodeFromNode.Text = "Add Node";
            this.BtAddNodeFromNode.UseVisualStyleBackColor = true;
            this.BtAddNodeFromNode.Click += new System.EventHandler(this.BtAddNodeFromNode_Click);
            // 
            // BtRemoveThisNode
            // 
            this.BtRemoveThisNode.Location = new System.Drawing.Point(6, 6);
            this.BtRemoveThisNode.Name = "BtRemoveThisNode";
            this.BtRemoveThisNode.Size = new System.Drawing.Size(113, 23);
            this.BtRemoveThisNode.TabIndex = 2;
            this.BtRemoveThisNode.Text = "Remove This Node";
            this.BtRemoveThisNode.UseVisualStyleBackColor = true;
            this.BtRemoveThisNode.Click += new System.EventHandler(this.BtRemoveThisNode_Click);
            // 
            // CbListOfNodes
            // 
            this.CbListOfNodes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CbListOfNodes.FormattingEnabled = true;
            this.CbListOfNodes.Location = new System.Drawing.Point(137, 6);
            this.CbListOfNodes.Name = "CbListOfNodes";
            this.CbListOfNodes.Size = new System.Drawing.Size(121, 21);
            this.CbListOfNodes.TabIndex = 3;
            // 
            // TcNodeTabs
            // 
            this.TcNodeTabs.Controls.Add(this.TpThisNode);
            this.TcNodeTabs.Controls.Add(this.TpFromThisNode);
            this.TcNodeTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcNodeTabs.Enabled = false;
            this.TcNodeTabs.Location = new System.Drawing.Point(0, 0);
            this.TcNodeTabs.Name = "TcNodeTabs";
            this.TcNodeTabs.SelectedIndex = 0;
            this.TcNodeTabs.Size = new System.Drawing.Size(272, 149);
            this.TcNodeTabs.TabIndex = 0;
            // 
            // TpThisNode
            // 
            this.TpThisNode.Controls.Add(this.cbAckBroadcastFrom);
            this.TpThisNode.Controls.Add(this.btBroadcastMessageFromNode);
            this.TpThisNode.Controls.Add(this.tbBroadCastFromNode);
            this.TpThisNode.Controls.Add(this.BtAddNodeFromNode);
            this.TpThisNode.Controls.Add(this.BtRemoveThisNode);
            this.TpThisNode.Location = new System.Drawing.Point(4, 22);
            this.TpThisNode.Name = "TpThisNode";
            this.TpThisNode.Padding = new System.Windows.Forms.Padding(3);
            this.TpThisNode.Size = new System.Drawing.Size(264, 123);
            this.TpThisNode.TabIndex = 0;
            this.TpThisNode.Text = "Act On";
            this.TpThisNode.UseVisualStyleBackColor = true;
            // 
            // TpFromThisNode
            // 
            this.TpFromThisNode.Controls.Add(this.btSendMessageFromNode);
            this.TpFromThisNode.Controls.Add(this.tbMessageFromNode);
            this.TpFromThisNode.Controls.Add(this.BtRmove);
            this.TpFromThisNode.Controls.Add(this.lblToNode);
            this.TpFromThisNode.Controls.Add(this.CbListOfNodes);
            this.TpFromThisNode.Location = new System.Drawing.Point(4, 22);
            this.TpFromThisNode.Name = "TpFromThisNode";
            this.TpFromThisNode.Padding = new System.Windows.Forms.Padding(3);
            this.TpFromThisNode.Size = new System.Drawing.Size(264, 123);
            this.TpFromThisNode.TabIndex = 1;
            this.TpFromThisNode.Text = "Act From";
            this.TpFromThisNode.UseVisualStyleBackColor = true;
            // 
            // lblToNode
            // 
            this.lblToNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToNode.AutoSize = true;
            this.lblToNode.Location = new System.Drawing.Point(12, 9);
            this.lblToNode.Name = "lblToNode";
            this.lblToNode.Size = new System.Drawing.Size(119, 13);
            this.lblToNode.TabIndex = 4;
            this.lblToNode.Text = "Node to receive Action:";
            // 
            // BtRmove
            // 
            this.BtRmove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtRmove.Location = new System.Drawing.Point(3, 94);
            this.BtRmove.Name = "BtRmove";
            this.BtRmove.Size = new System.Drawing.Size(75, 23);
            this.BtRmove.TabIndex = 5;
            this.BtRmove.Text = "Remove";
            this.BtRmove.UseVisualStyleBackColor = true;
            this.BtRmove.Click += new System.EventHandler(this.BtRmove_Click);
            // 
            // tbMessageFromNode
            // 
            this.tbMessageFromNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessageFromNode.Location = new System.Drawing.Point(6, 68);
            this.tbMessageFromNode.Name = "tbMessageFromNode";
            this.tbMessageFromNode.Size = new System.Drawing.Size(250, 20);
            this.tbMessageFromNode.TabIndex = 6;
            // 
            // btSendMessageFromNode
            // 
            this.btSendMessageFromNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSendMessageFromNode.Location = new System.Drawing.Point(161, 94);
            this.btSendMessageFromNode.Name = "btSendMessageFromNode";
            this.btSendMessageFromNode.Size = new System.Drawing.Size(95, 23);
            this.btSendMessageFromNode.TabIndex = 7;
            this.btSendMessageFromNode.Text = "Send Message";
            this.btSendMessageFromNode.UseVisualStyleBackColor = true;
            this.btSendMessageFromNode.Click += new System.EventHandler(this.btSendMessageFromNode_Click);
            // 
            // btBroadcastMessageFromNode
            // 
            this.btBroadcastMessageFromNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btBroadcastMessageFromNode.Location = new System.Drawing.Point(87, 94);
            this.btBroadcastMessageFromNode.Name = "btBroadcastMessageFromNode";
            this.btBroadcastMessageFromNode.Size = new System.Drawing.Size(95, 23);
            this.btBroadcastMessageFromNode.TabIndex = 9;
            this.btBroadcastMessageFromNode.Text = "Broadcast";
            this.btBroadcastMessageFromNode.UseVisualStyleBackColor = true;
            this.btBroadcastMessageFromNode.Click += new System.EventHandler(this.btBroadcastMessageFromNode_Click);
            // 
            // tbBroadCastFromNode
            // 
            this.tbBroadCastFromNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBroadCastFromNode.Location = new System.Drawing.Point(8, 68);
            this.tbBroadCastFromNode.Name = "tbBroadCastFromNode";
            this.tbBroadCastFromNode.Size = new System.Drawing.Size(250, 20);
            this.tbBroadCastFromNode.TabIndex = 8;
            // 
            // cbAckBroadcastFrom
            // 
            this.cbAckBroadcastFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAckBroadcastFrom.AutoSize = true;
            this.cbAckBroadcastFrom.Location = new System.Drawing.Point(205, 100);
            this.cbAckBroadcastFrom.Name = "cbAckBroadcastFrom";
            this.cbAckBroadcastFrom.Size = new System.Drawing.Size(51, 17);
            this.cbAckBroadcastFrom.TabIndex = 10;
            this.cbAckBroadcastFrom.Text = "Ack?";
            this.cbAckBroadcastFrom.UseVisualStyleBackColor = true;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 613);
            this.Controls.Add(this.SplitMain);
            this.Name = "GUI";
            this.Text = "GUI";
            this.ResizeEnd += new System.EventHandler(this.GUI_ResizeEnd);
            this.Tabs.ResumeLayout(false);
            this.TabDebugConsole.ResumeLayout(false);
            this.TabDebugConsole.PerformLayout();
            this.TabTextView.ResumeLayout(false);
            this.TabTextView.PerformLayout();
            this.SplitMain.Panel1.ResumeLayout(false);
            this.SplitMain.Panel2.ResumeLayout(false);
            this.SplitMain.ResumeLayout(false);
            this.SplitDisplay.Panel2.ResumeLayout(false);
            this.SplitDisplay.Panel2.PerformLayout();
            this.SplitDisplay.ResumeLayout(false);
            this.SplitLower.Panel1.ResumeLayout(false);
            this.SplitLower.Panel2.ResumeLayout(false);
            this.SplitLower.Panel2.PerformLayout();
            this.SplitLower.ResumeLayout(false);
            this.TcNodeTabs.ResumeLayout(false);
            this.TpThisNode.ResumeLayout(false);
            this.TpThisNode.PerformLayout();
            this.TpFromThisNode.ResumeLayout(false);
            this.TpFromThisNode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonBroadcast;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.TextBox textBoxConsole;
        private System.Windows.Forms.TextBox textBoxDisplay;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabDebugConsole;
        private System.Windows.Forms.TabPage TabTextView;
        private System.Windows.Forms.SplitContainer SplitMain;
        private System.Windows.Forms.SplitContainer SplitLower;
        private System.Windows.Forms.SplitContainer SplitDisplay;
        private System.Windows.Forms.TextBox TbNodeInfo;
        private System.Windows.Forms.Button BtAddNodeFromNode;
        private System.Windows.Forms.Button BtRemoveThisNode;
        private System.Windows.Forms.TabControl TcNodeTabs;
        private System.Windows.Forms.TabPage TpThisNode;
        private System.Windows.Forms.TabPage TpFromThisNode;
        private System.Windows.Forms.Label lblToNode;
        private System.Windows.Forms.ComboBox CbListOfNodes;
        private System.Windows.Forms.CheckBox cbAckBroadcastFrom;
        private System.Windows.Forms.Button btBroadcastMessageFromNode;
        private System.Windows.Forms.TextBox tbBroadCastFromNode;
        private System.Windows.Forms.Button btSendMessageFromNode;
        private System.Windows.Forms.TextBox tbMessageFromNode;
        private System.Windows.Forms.Button BtRmove;
    }
}