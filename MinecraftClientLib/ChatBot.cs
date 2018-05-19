using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace MinecraftClient
{
    ///
    /// Welcome to the Bot API file !
    /// The virtual class "ChatBot" contains anything you need for creating chat bots
    /// Inherit from this class while adding your bot class to the "ChatBots" folder.
    /// Override the methods you want for handling events: Initialize, Update, GetText.
    ///
    /// For testing your bot you can add it in McTcpClient.cs (see comment at line ~119).
    /// Your bot will be loaded everytime MCC is started so that you can test/debug.
    ///
    /// Once your bot is fully written and tested, you can export it a standalone script.
    /// This way it can be loaded in newer MCC builds, without modifying MCC itself.
    /// See config/sample-script-with-chatbot.cs for a ChatBot script example.
    ///

    /// <summary>
    /// The virtual class containing anything you need for creating chat bots.
    /// </summary>
    public abstract class ChatBot
    {
        public enum DisconnectReason { InGameKick, LoginRejected, ConnectionLost };

        /// <summary>
        /// Verify that a string contains only a-z A-Z 0-9 and _ characters.
        /// </summary>
        public static bool IsValidName(string username)
        {
            if (String.IsNullOrEmpty(username))
                return false;

            foreach (char c in username)
                if (!((c >= 'a' && c <= 'z')
                      || (c >= 'A' && c <= 'Z')
                      || (c >= '0' && c <= '9')
                      || c == '_'))
                    return false;

            return true;
        }
    }
}
