namespace Project_Rem.Twitch
{
    partial class TwitchController
    {
        #region Handlers
        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void MessageHandler(Message message);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public MessageHandler MessageReceivedHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void JoinedRoom(string roomName);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public JoinedRoom JoinedRoomHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void LeftRoom(string roomName);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public LeftRoom LeftRoomHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void Disconnected();

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public Disconnected DisconnectedHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void Connected();

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public Connected ConnectedHandler;

        /// <summary>
        /// Teardown delegate from main callable within the bot for shutdown purposes
        /// </summary>
        public delegate void ChatLog(Message message);

        /// <summary>
        /// A handler to our main's teardown function
        /// </summary>
        public ChatLog ChatLogHandler;
        #endregion
    }
}
