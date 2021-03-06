﻿namespace Project_Rem
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Toolstrip_Connect = new System.Windows.Forms.ToolStripMenuItem();
            this.Toolstrip_JoinRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.Toolstrip_SaveLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.Toolstrip_ViewLogs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.Toolstip_Disconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.Toolstrip_LeaveRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TextBox_Message = new System.Windows.Forms.TextBox();
            this.Button_Submit = new System.Windows.Forms.Button();
            this.btn_system = new System.Windows.Forms.Button();
            this.rtb_system = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.menuStrip1.Size = new System.Drawing.Size(748, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Toolstrip_Connect,
            this.Toolstrip_JoinRoom,
            this.toolStripSeparator,
            this.Toolstrip_SaveLogs,
            this.Toolstrip_ViewLogs,
            this.toolStripSeparator1,
            this.Toolstip_Disconnect,
            this.Toolstrip_LeaveRoom,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // Toolstrip_Connect
            // 
            this.Toolstrip_Connect.Image = ((System.Drawing.Image)(resources.GetObject("Toolstrip_Connect.Image")));
            this.Toolstrip_Connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolstrip_Connect.Name = "Toolstrip_Connect";
            this.Toolstrip_Connect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.Toolstrip_Connect.Size = new System.Drawing.Size(201, 22);
            this.Toolstrip_Connect.Text = "&Connect";
            this.Toolstrip_Connect.Click += new System.EventHandler(this.Toolstrip_Connect_Click);
            // 
            // Toolstrip_JoinRoom
            // 
            this.Toolstrip_JoinRoom.Image = ((System.Drawing.Image)(resources.GetObject("Toolstrip_JoinRoom.Image")));
            this.Toolstrip_JoinRoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolstrip_JoinRoom.Name = "Toolstrip_JoinRoom";
            this.Toolstrip_JoinRoom.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.Toolstrip_JoinRoom.Size = new System.Drawing.Size(201, 22);
            this.Toolstrip_JoinRoom.Text = "&Join Room";
            this.Toolstrip_JoinRoom.Click += new System.EventHandler(this.Toolstrip_JoinRoom_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(198, 6);
            // 
            // Toolstrip_SaveLogs
            // 
            this.Toolstrip_SaveLogs.Image = ((System.Drawing.Image)(resources.GetObject("Toolstrip_SaveLogs.Image")));
            this.Toolstrip_SaveLogs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolstrip_SaveLogs.Name = "Toolstrip_SaveLogs";
            this.Toolstrip_SaveLogs.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.Toolstrip_SaveLogs.Size = new System.Drawing.Size(201, 22);
            this.Toolstrip_SaveLogs.Text = "&Save Room Logs";
            // 
            // Toolstrip_ViewLogs
            // 
            this.Toolstrip_ViewLogs.Name = "Toolstrip_ViewLogs";
            this.Toolstrip_ViewLogs.Size = new System.Drawing.Size(201, 22);
            this.Toolstrip_ViewLogs.Text = "View Room Logs";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // Toolstip_Disconnect
            // 
            this.Toolstip_Disconnect.Image = ((System.Drawing.Image)(resources.GetObject("Toolstip_Disconnect.Image")));
            this.Toolstip_Disconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolstip_Disconnect.Name = "Toolstip_Disconnect";
            this.Toolstip_Disconnect.Size = new System.Drawing.Size(201, 22);
            this.Toolstip_Disconnect.Text = "Disconnect";
            this.Toolstip_Disconnect.Click += new System.EventHandler(this.Toolstip_Disconnect_Click);
            // 
            // Toolstrip_LeaveRoom
            // 
            this.Toolstrip_LeaveRoom.Image = ((System.Drawing.Image)(resources.GetObject("Toolstrip_LeaveRoom.Image")));
            this.Toolstrip_LeaveRoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Toolstrip_LeaveRoom.Name = "Toolstrip_LeaveRoom";
            this.Toolstrip_LeaveRoom.Size = new System.Drawing.Size(201, 22);
            this.Toolstrip_LeaveRoom.Text = "Leave Room";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(198, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // TextBox_Message
            // 
            this.TextBox_Message.BackColor = System.Drawing.Color.Azure;
            this.TextBox_Message.Location = new System.Drawing.Point(12, 422);
            this.TextBox_Message.Name = "TextBox_Message";
            this.TextBox_Message.Size = new System.Drawing.Size(405, 20);
            this.TextBox_Message.TabIndex = 0;
            // 
            // Button_Submit
            // 
            this.Button_Submit.BackColor = System.Drawing.Color.Transparent;
            this.Button_Submit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Button_Submit.BackgroundImage")));
            this.Button_Submit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_Submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Submit.ForeColor = System.Drawing.Color.Black;
            this.Button_Submit.Location = new System.Drawing.Point(423, 421);
            this.Button_Submit.Name = "Button_Submit";
            this.Button_Submit.Size = new System.Drawing.Size(86, 22);
            this.Button_Submit.TabIndex = 2;
            this.Button_Submit.Text = "Submit";
            this.Button_Submit.UseVisualStyleBackColor = false;
            this.Button_Submit.Click += new System.EventHandler(this.Button_Submit_Click);
            // 
            // btn_system
            // 
            this.btn_system.BackColor = System.Drawing.Color.Transparent;
            this.btn_system.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_system.BackgroundImage")));
            this.btn_system.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_system.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_system.Location = new System.Drawing.Point(423, 27);
            this.btn_system.Name = "btn_system";
            this.btn_system.Size = new System.Drawing.Size(86, 26);
            this.btn_system.TabIndex = 3;
            this.btn_system.Text = "system";
            this.btn_system.UseVisualStyleBackColor = false;
            // 
            // rtb_system
            // 
            this.rtb_system.BackColor = System.Drawing.Color.Azure;
            this.rtb_system.Location = new System.Drawing.Point(12, 27);
            this.rtb_system.Name = "rtb_system";
            this.rtb_system.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rtb_system.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtb_system.Size = new System.Drawing.Size(405, 377);
            this.rtb_system.TabIndex = 4;
            this.rtb_system.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(748, 456);
            this.Controls.Add(this.rtb_system);
            this.Controls.Add(this.btn_system);
            this.Controls.Add(this.Button_Submit);
            this.Controls.Add(this.TextBox_Message);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Rem";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Toolstrip_Connect;
        private System.Windows.Forms.ToolStripMenuItem Toolstrip_JoinRoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem Toolstrip_SaveLogs;
        private System.Windows.Forms.ToolStripMenuItem Toolstrip_ViewLogs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem Toolstip_Disconnect;
        private System.Windows.Forms.ToolStripMenuItem Toolstrip_LeaveRoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TextBox TextBox_Message;
        private System.Windows.Forms.Button Button_Submit;
        private System.Windows.Forms.Button btn_system;
        private System.Windows.Forms.RichTextBox rtb_system;
    }
}

