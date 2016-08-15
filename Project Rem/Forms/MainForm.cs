using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Project_Rem.Core;
using Project_Rem.Twitch;
using System.Threading;
using System.Runtime.InteropServices;
using System.Linq;

namespace Project_Rem
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr wnd, uint Msg, IntPtr wParam, IntPtr lParam);
        public const uint WM_VSCROLL = 0x0115;
        public const uint SB_BOTTOM = 7;

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
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(DisconnectedHandler), new object[] { });
                    return;
                }

                if (RoomTabControl != null)
                {
                    foreach (TabPage tab in RoomTabControl.TabPages)
                    {
                        if (tab.Text.ToLowerInvariant() != "System".ToLowerInvariant())
                        {
                            RoomTabControl.TabPages.Remove(tab);
                        }
                    } 
                    RoomTabControl.Invalidate();
                    Thread.Sleep(100);
                    controller = null;
                }
            }
            catch(System.ObjectDisposedException)
            {

            }
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

        private void tabClicked(object sender, MouseEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabs = tabControl.TabPages;

            if (e.Button == MouseButtons.Middle)
            {
                TabPage theTab = tabs.Cast<TabPage>().Where((t, i) => tabControl.GetTabRect(i).Contains(e.Location)).First();
                controller.LeaveRoom(theTab.Text.ToLowerInvariant());
                theTab.Controls.Clear();
                tabs.Remove(theTab);
            }
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
            chatBox.Height = sysTab.Height - 7;
            chatBox.Width = sysTab.Width - 12;
            chatBox.Left = sysTab.Left + 2;
            chatBox.Top = sysTab.Top - 18;
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
                            PostMessage((tab.Controls[0] as RichTextBox).Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, (IntPtr)IntPtr.Zero);

                        }
                        if (message.system)
                        {
                            (tab.Controls[0] as RichTextBox).AppendText(DateTime.Now + " : " + message.message.TrimEnd("\r\n".ToCharArray()));
                            PostMessage((tab.Controls[0] as RichTextBox).Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, (IntPtr)IntPtr.Zero);
                        }
                        else
                        {
                            (tab.Controls[0] as RichTextBox).AppendText(DateTime.Now + " - " + message.sender + ": " + message.message);
                            PostMessage((tab.Controls[0] as RichTextBox).Handle, WM_VSCROLL, (IntPtr)SB_BOTTOM, (IntPtr)IntPtr.Zero);
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
            if (Rem != null && controller != null)
            {
                List<Message> toSend = new List<Message>();
                toSend.Add(new Message("Already Connected!", "System", null, false, "void", true));
                controller.AddMessagesToSend(toSend);
                return;
            }

            if (Rem == null)
            {
                Rem = new RemBot("RemuBot");
            }

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
            if (controller == null)
            {
                AppendChatBox(new Message("Please connect to Twitch first.", "system", null, false, "void", true));
                return;
            }
            if (controller.IsConnected())
            {
                Forms.RoomJoinForm getRoomForm = new Forms.RoomJoinForm(controller.JoinRoom);
                var result = getRoomForm.ShowDialog();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (controller != null && controller.IsConnected())
            {
                List<Message> toSend = new List<Message>();
                Message tMes = new Message("-disconnect", "System", null, false, null, true);
                toSend.Add(tMes);
                controller.AddMessagesToSend(toSend);
            }
            else if (controller != null)
            {
                controller = null;
            }
        }

        private void Toolstip_Disconnect_Click(object sender, EventArgs e)
        {
            if (controller != null && controller.IsConnected())
            {
                List<Message> toSend = new List<Message>();
                Message tMes = new Message("-disconnect", "System", null, false, null, true);
                toSend.Add(tMes);
                controller.AddMessagesToSend(toSend);
            }
            else if (controller != null)
            {
                controller = null;
            }
        }
    }
}
