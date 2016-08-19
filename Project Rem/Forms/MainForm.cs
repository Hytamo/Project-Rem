using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Project_Rem.Core;
using Twitch.Controller;
using NLog;
namespace Project_Rem
{
    /// <summary>
    /// this is our entry point for bot functionality.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// this is our bot. beep boop!
        /// </summary>
        RemBot rem;

        /// <summary>
        /// this is our primary twitch controller.
        /// </summary>
        TwitchController controller;

        /// <summary>
        /// our primary logger.
        /// </summary>
        public static Logger log;

        List<Button> roombuttons;

        /// <summary>
        /// constructs a new instance of this form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            AcceptButton = Button_Submit;
        }

        /// <summary>
        /// handles when twitch has connected.
        /// </summary>
        public void ConnectedHandler()
        {
            log.Info("Successfully connected to Twitch.");
        }

        /// <summary>
        /// handles when twitch disconnects.
        /// </summary>
        public void DisconnectedHandler()
        {
            log.Info("You have disconnected from Twitch.");
        }

        /// <summary>
        /// handles when the bot has left a specific room.
        /// </summary>
        /// <param name="roomName">the room that's been left.</param>
        public void LeftRoomHandler(string room)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(LeftRoomHandler), new object[] { room });
                return;
            }

            log.Info("You have left room: " + room);
            Button toRemove = roombuttons.Where(x => x.Text == room).FirstOrDefault();
            Controls.Remove(toRemove);

            for (int i = 0; i < roombuttons.Count; i++)
            {
                if (i == 0) roombuttons[i].Top = btn_system.Top + btn_system.Height + 5;
                else roombuttons[i].Top = roombuttons[i-1].Top + roombuttons[i-1].Height + 5;
                roombuttons[i].Invalidate();
            }
        }


        /// <summary>
        /// handles when a room has been joined by twitch controller.
        /// </summary>
        /// <param name="room">name of the room that's been joined.</param>
        public void JoinedRoomHandler(string room)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(JoinedRoomHandler), new object[] { room });
                return;
            }

            if (roombuttons != null && roombuttons.Where(x => x.Text == room).Count() == 1)
            {
                log.Warn("Tried to join room you were already in.");
                return;
            }

            log.Info("You have joined room: " + room);
            Button but = new Button();

            but.Width = btn_system.Width;
            but.Height = btn_system.Height;
            but.Left = btn_system.Left;
            if (roombuttons.Count == 0) but.Top = btn_system.Top + btn_system.Height + 5;
            else but.Top = roombuttons.LastOrDefault().Top + roombuttons.LastOrDefault().Height + 5;
            but.MouseUp += btn_generic_MouseUp;
            but.Text = room;
            but.Name = "btn_" + room;
            but.FlatStyle = FlatStyle.Flat;
            but.BackColor = System.Drawing.Color.Transparent;
            but.BackgroundImage = btn_system.BackgroundImage;
            but.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold); ;
            Controls.Add(but);
            roombuttons.Add(but);
        }

        /// <summary>
        /// handles when twitch has received a message from a user.
        /// </summary>
        /// <param name="message">the raw message.</param>
        /// <param name="room">the originating room. null if whisper.</param>
        /// <param name="user">the sending user.</param>
        /// <param name="messagetype">privmsg if normal chat, whisper if whisper.</param>
        public void MessageReceivedHandler(string message, string room, string user, string messagetype)
        { // TODO make this less placeholdery.
            if (user.ToLowerInvariant() == "hytamo")
            {
                if (message.StartsWith("-leave"))
                {
                    controller.LeaveRoom(message.Split(' ').LastOrDefault());
                    controller.SendWhisper("Leaving " + message.Split(' ').LastOrDefault() + "!", user);
                    LogManager.GetLogger("syslogger").Info("leaving room: " + message.Split(' ').LastOrDefault());
                    return;
                }
                if (message.StartsWith("-join"))
                {
                    controller.JoinRoom(message.Split(' ').LastOrDefault());
                    controller.SendWhisper("Joining " + message.Split(' ').LastOrDefault() + "!", user);
                    LogManager.GetLogger("syslogger").Info("joining room: " + message.Split(' ').LastOrDefault());
                    return;
                }
            }

            foreach (Message msg in rem.ParseMessage(new Message(message, room, user, (messagetype == "WHISPER"))))
            {
                if (msg.whisper) controller.SendWhisper(msg.message, user);
                else controller.SendMessage(msg.message, msg.room);
            }
        }

        /// <summary>
        /// this is where we'll connect to twitch.
        /// </summary>
        /// <param name="sender">originating sender.</param>
        /// <param name="e">event args.</param>
        private void Toolstrip_Connect_Click(object sender, EventArgs e)
        {
            if (controller != null && controller.IsConnected()) { log.Warn("You are already connected to Twitch."); return; }
            log.Info("Connecting to Twitch...");
            roombuttons = new List<Button>();
            controller = new TwitchController("remubot", "oauth:jumjklxvmvhgi6s4ae93ib5v8cyt4w");
            controller.ConnectedHandler += ConnectedHandler;
            controller.MessageReceivedHandler += MessageReceivedHandler;
            controller.JoinedRoomHandler += JoinedRoomHandler;
            controller.DisconnectedHandler += DisconnectedHandler;
            controller.LeftRoomHandler = LeftRoomHandler;
            //controller.GetUserListHandler           += GetUserListHandler;
            controller.Connect();
            if (rem == null) rem = new RemBot("RemuBot");
        }

        public void GetUserListHandler(Dictionary<string, List<string>> list, string room)
        {
            log.Info("Userlist updated for room: " + room);
            if (InvokeRequired)
            {
                Invoke(new Action<Dictionary<string, List<string>>, string>(GetUserListHandler), new object[] { list, room });
                return;
            }
        }

        /// <summary>
        /// handles when form sends a joinroom request. this blocks main form.
        /// </summary>
        /// <param name="sender">originating sender.</param>
        /// <param name="e">event args.</param>
        private void Toolstrip_JoinRoom_Click(object sender, EventArgs e)
        {
            if (controller == null) { log.Warn("Connect to Twitch to join a room."); return; }
            Forms.RoomJoinForm getRoomForm = new Forms.RoomJoinForm(controller.JoinRoom);
            var result = getRoomForm.ShowDialog();
        }

        /// <summary>
        /// handles sending a message to twitch from chat.
        /// </summary>
        /// <param name="sender">originating button click or enter.</param>
        /// <param name="e">event args.</param>
        private void Button_Submit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// starts tearing down the application when called.
        /// </summary>
        /// <param name="sender">originating sender.</param>
        /// <param name="e">event args.</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// called when we're tearing down.
        /// </summary>
        /// <param name="sender">originating sender.</param>
        /// <param name="e">form closing event args.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (controller != null && controller.IsConnected()) controller.Disconnect();
            else if (controller != null) controller = null;
        }

        /// <summary>
        /// handles when the disconnect button is pressed.
        /// </summary>
        /// <param name="sender">originating sender.</param>
        /// <param name="e">event args.</param>
        private void Toolstip_Disconnect_Click(object sender, EventArgs e)
        {
            if (controller == null) { log.Warn("Already disconnected."); return; }
            log.Info("Disconnecting from Twitch...");
            if (controller.IsConnected()) controller.Disconnect();
            controller = null;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            log = LogManager.GetLogger("systemlog");
            log.Info("Ohayo, goshujinsaama. Rem is here for you!");
        }

        private void btn_generic_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            if (e.Button == MouseButtons.Right)
            {
                controller.LeaveRoom(btn.Text);
                roombuttons.Remove(btn);
                Controls.Remove(btn);
            }
            else if (e.Button == MouseButtons.Left)
            {
            }
        }
    }
}
