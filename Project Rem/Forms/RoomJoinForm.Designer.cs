namespace Project_Rem.Forms
{
    partial class RoomJoinForm
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
            this.Textbox_RoomToJoin = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Button_JoinRoom = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Textbox_RoomToJoin
            // 
            this.Textbox_RoomToJoin.Location = new System.Drawing.Point(12, 12);
            this.Textbox_RoomToJoin.Name = "Textbox_RoomToJoin";
            this.Textbox_RoomToJoin.Size = new System.Drawing.Size(181, 20);
            this.Textbox_RoomToJoin.TabIndex = 0;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(281, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Auto Join";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // Button_JoinRoom
            // 
            this.Button_JoinRoom.Location = new System.Drawing.Point(199, 10);
            this.Button_JoinRoom.Name = "Button_JoinRoom";
            this.Button_JoinRoom.Size = new System.Drawing.Size(75, 23);
            this.Button_JoinRoom.TabIndex = 3;
            this.Button_JoinRoom.Text = "Join Room";
            this.Button_JoinRoom.UseVisualStyleBackColor = true;
            this.Button_JoinRoom.Click += new System.EventHandler(this.Button_JoinRoom_Click);
            // 
            // RoomJoinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 51);
            this.Controls.Add(this.Button_JoinRoom);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Textbox_RoomToJoin);
            this.Name = "RoomJoinForm";
            this.Text = "Enter Room to Join";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Textbox_RoomToJoin;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button Button_JoinRoom;
    }
}