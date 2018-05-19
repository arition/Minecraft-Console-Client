using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MinecraftClient;
using MinecraftClient.Mapping;
using MinecraftClient.Protocol;
using MinecraftClient.Protocol.Handlers.Forge;

namespace Test
{
    public class MCClient : IMinecraftComHandler
    {
        private string _hostname;
        private ushort _serverPort;
        private string _username;
        private string _userUuid;
        private string _sessionId;
        private World _world = new World();
        private Location _location;
        private readonly Dictionary<Guid, string> onlinePlayers = new Dictionary<Guid, string>();

        public MCClient(string host, ushort port, string login, string password)
        {
            _serverPort = port;
            _hostname = host;
            var session = new SessionToken();
            var loginResult = ProtocolHandler.GetLogin(login, password, out session);
            if(loginResult != ProtocolHandler.LoginResult.Success)throw new Exception();
            _userUuid = session.PlayerID;
            _sessionId = session.ID;
            _username = session.PlayerName;
            var protocolversion = 0;
            ForgeInfo forgeInfo = null;
            ProtocolHandler.GetServerInfo(host, port, ref protocolversion, ref forgeInfo);
            var tcpClient = new TcpClient(host, port);
            tcpClient.ReceiveBufferSize = 1024 * 1024;
            var handler = ProtocolHandler.GetProtocolHandler(tcpClient, protocolversion, forgeInfo, this);
            handler.Login();
            handler.SendChatMessage("hello");
            Thread.Sleep(2000);
            Console.WriteLine(String.Join(", ", GetOnlinePlayers()));
        }

        public int GetServerPort()
        {
            return _serverPort;
        }

        public string GetServerHost()
        {
            return _hostname;
        }

        public string GetUsername()
        {
            return _username;
        }

        public string GetUserUUID()
        {
            return _userUuid;
        }

        public string GetSessionID()
        {
            return _sessionId;
        }

        /// <summary>
        /// Get a set of online player names
        /// </summary>
        /// <returns>Online player names</returns>
        public string[] GetOnlinePlayers()
        {
            lock (onlinePlayers)
            {
                return onlinePlayers.Values.Distinct().ToArray();
            }
        }

        public Location GetCurrentLocation()
        {
            return _location;
        }

        public World GetWorld()
        {
            return _world;
        }

        public void OnGameJoined()
        {
        }

        public void OnTextReceived(string text, bool isJson)
        {
            var links = new List<string>();
            string json = null;
            if (isJson)
            {
                json = text;
                text = ChatParser.ParseText(json, links);
            }
            Console.WriteLine(text);
        }

        /// <summary>
        /// Triggered when a new player joins the game
        /// </summary>
        /// <param name="uuid">UUID of the player</param>
        /// <param name="name">Name of the player</param>
        public void OnPlayerJoin(Guid uuid, string name)
        {
            //Ignore placeholders eg 0000tab# from TabListPlus
            if (!ChatBot.IsValidName(name))
                return;

            lock (onlinePlayers)
            {
                onlinePlayers[uuid] = name;
            }
        }

        /// <summary>
        /// Triggered when a player has left the game
        /// </summary>
        /// <param name="uuid">UUID of the player</param>
        public void OnPlayerLeave(Guid uuid)
        {
            lock (onlinePlayers)
            {
                onlinePlayers.Remove(uuid);
            }
        }

        public void UpdateLocation(Location location, byte[] yawpitch)
        {
        }

        public void OnConnectionLost(ChatBot.DisconnectReason reason, string message)
        {
        }

        public void OnUpdate()
        {
        }

        public void RegisterPluginChannel(string channel, ChatBot bot)
        {
        }

        public void UnregisterPluginChannel(string channel, ChatBot bot)
        {
        }

        public bool SendPluginChannelMessage(string channel, byte[] data, bool sendEvenIfNotRegistered = false)
        {
            return true;
        }

        public void OnPluginChannelMessage(string channel, byte[] data)
        {
        }
    }
}
