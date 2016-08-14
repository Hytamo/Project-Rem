using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Project_Rem.Core;
using Project_Rem.Twitch;
using System.Threading;

namespace Project_Rem
{
    public partial class MainForm : Form
    {
        RemBot Rem;
        TwitchController controller;
        object chatupdateLocker;
        public MainForm()
        {
            InitializeComponent();
            this.AcceptButton = Button_Submit;
            chatupdateLocker = new object();
        }

        public void ConnectedHandler()
        {

        }

        public void DisconnectedHandler()
        {

        }

        public void LeftRoomHandler(string roomName)
        {
            TabPage toRemove = null;
            foreach (TabPage tab in RoomTabControl.TabPages)
            {
                if (tab.Text.ToLowerInvariant().Contains(roomName.ToLowerInvariant()))
                {
                    toRemove = tab;
                    break;
                }
            }
            if (toRemove != null)
                RoomTabControl.TabPages.Remove(toRemove);
        }

        public void JoinedRoomHandler(string roomName)
        {
            foreach (TabPage tab in RoomTabControl.TabPages)
            {
                if (tab.Text.ToLowerInvariant().Contains(roomName.ToLowerInvariant()))
                {
                    return;
                }
            }

            TabPage newTab = new TabPage(roomName);
            newTab.BackColor = Color.White;
            TabPage sysTab = RoomTabControl.TabPages[0];
            RichTextBox chatBox = new RichTextBox();
            chatBox.Height = sysTab.Height - 12;
            chatBox.Width = sysTab.Width - 12;
            chatBox.Left = sysTab.Left + 2;
            chatBox.Top = sysTab.Top - 16;
            chatBox.BackColor = Color.White;
            chatBox.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
            chatBox.Name = roomName + "chatbox";
            chatBox.ReadOnly = true;
            chatBox.ResetText();
            newTab.Controls.Add(chatBox);
            RoomTabControl.TabPages.Add(newTab);
            RoomTabControl.Invalidate();
            RoomTabControl.SelectedTab = RoomTabControl.TabPages[RoomTabControl.TabCount - 1];
        }

        public void AppendChatBox(Message message)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Message>(AppendChatBox), new object[] { message });
                return;
            }

            foreach (TabPage tab in RoomTabControl.TabPages)
            {
                if (tab.Controls[0].GetType() == typeof(RichTextBox))
                {
                    if ((tab.Controls[0] as RichTextBox).Name.ToLowerInvariant().Contains(message.room.Replace("#", "").ToLowerInvariant()))
                    {
                        if ((tab.Controls[0] as RichTextBox).TextLength > 0)
                        {
                            (tab.Controls[0] as RichTextBox).AppendText(Environment.NewLine);
                        }
                        if (message.system)
                        {
                            (tab.Controls[0] as RichTextBox).AppendText(DateTime.Now + " : " + message.message);
                        }
                        else
                        {
                            (tab.Controls[0] as RichTextBox).AppendText(DateTime.Now + " : " + message.room + " : " + message.sender + " : " + message.message);
                        }
                        tab.Controls[0].Invalidate();
                        break;
                    }
                }
            }
        }

        public void MessageReceivedHandler(Message message)
        {
            List<Message> toSend = Rem.ParseMessage(message);
            controller.AddMessagesToSend(toSend);
        }

        private void Toolstrip_Connect_Click(object sender, EventArgs e)
        {
            if (Rem != null)
            {
                List<Message> toSend = new List<Message>();
                toSend.Add(new Message("Already Connected!", "System", null, false, "void", true));
                controller.AddMessagesToSend(toSend);
                return;
            }

            Rem = new RemBot("RemuBot");
            //controller = new TwitchController("NaolinBot", "oauth:ob2wapvoj74l74aclhynrh2r0kcq4z");
            controller = new TwitchController(Rem.GetBotName(), "oauth:jumjklxvmvhgi6s4ae93ib5v8cyt4w");
            
            controller.MessageReceivedHandler       += MessageReceivedHandler;
            controller.JoinedRoomHandler            += JoinedRoomHandler;
            controller.LeftRoomHandler              += LeftRoomHandler;
            controller.DisconnectedHandler          += DisconnectedHandler;
            controller.ConnectedHandler             += ConnectedHandler;
            controller.ChatLogHandler               += AppendChatBox;
            controller.Connect();
        }

        private void Toolstrip_JoinRoom_Click(object sender, EventArgs e)
        {
            if (controller.IsConnected())
            {
                controller.JoinRoom("Hytamo");
            }
        }

        private void Button_Submit_Click(object sender, EventArgs e)
        {
            if (controller.IsConnected())
            {
                if (TextBox_Message.Text.Length > 0)
                {
                    bool system = RoomTabControl.SelectedTab.Text == "System";
                    if (!system)
                    {
                        List<Message> toSend = new List<Message>();
                        Message tMes = new Message(TextBox_Message.Text, RoomTabControl.SelectedTab.Text, Rem.GetBotName());
                        toSend.Add(tMes);
                        controller.AddMessagesToSend(toSend);
                        TextBox_Message.Text = "";
                    }
                }
            }
        }
    }
}
