using System;
using System.Windows.Forms;

namespace Project_Rem.Forms
{
    public partial class RoomJoinForm : Form
    {
        public delegate bool SetRoomFunc(string s);
        public SetRoomFunc SetRoomHandler;
        public RoomJoinForm(SetRoomFunc s)
        {
            InitializeComponent();
            this.AcceptButton = Button_JoinRoom;
            SetRoomHandler = s;

        }

        private void Button_JoinRoom_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Textbox_RoomToJoin.Text))
            {
                SetRoomHandler(Textbox_RoomToJoin.Text.ToLowerInvariant());
                Close();
            }
        }
    }
}
