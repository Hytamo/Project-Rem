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

namespace Project_Rem
{
    public partial class MainForm : Form
    {
        RemBot Rem;
        TwitchController controller;
        public MainForm()
        {
            InitializeComponent();
        }

        public void ConnectedHandler()
        {

        }

        public void DisconnectedHandler()
        {

        }

        public void LeftRoomHandler(string roomName)
        {

        }

        public void JoinedRoomHandler(string roomName)
        {

        }

        public void MessageReceivedHandler(Message message)
        {
            List<Message> toSend = Rem.ParseMessage(message);
            controller.AddMessagesToSend(toSend);
        }

        private void Toolstrip_Connect_Click(object sender, EventArgs e)
        {
            Rem = new RemBot();
            //controller = new TwitchController("NaolinBot", "oauth:ob2wapvoj74l74aclhynrh2r0kcq4z");
            controller = new TwitchController("RemuBot", "oauth:jumjklxvmvhgi6s4ae93ib5v8cyt4w");
            
            controller.MessageReceivedHandler       += MessageReceivedHandler;
            controller.JoinedRoomHandler            += JoinedRoomHandler;
            controller.LeftRoomHandler              += LeftRoomHandler;
            controller.DisconnectedHandler          += DisconnectedHandler;
            controller.ConnectedHandler             += ConnectedHandler;

            controller.Connect();
        }

        private void Toolstrip_JoinRoom_Click(object sender, EventArgs e)
        {
            controller.JoinRoom("Hytamo");
        }
    }
}
