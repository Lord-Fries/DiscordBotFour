using Discord;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TCNDiscordBotThree
{
    class Program
    {
        public static void Main(string[] args)
         => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;


        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.Log += Log;

            // You can assign your bot Token to a string and pass that in to connect.
            // This is, however insecure, particularly if you plan to have your code hsoted in a public repository.
            var token = File.ReadAllText("token.txt");


            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            //block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandHandler(SocketMessage message)
        {
            string command = "";
            int lengthOfCommand = -1;


            //Filtering Messages begin here
            if (!message.Content.StartsWith('_')) //This is your prefix
                return Task.CompletedTask;

            if (message.Author.IsBot) //This ignores all commands from bots
                return Task.CompletedTask;

            if (message.Content.Contains(' '))
                lengthOfCommand = message.Content.IndexOf(' ');
            else
                lengthOfCommand = message.Content.Length;

            command = message.Content.Substring(1, lengthOfCommand - 1).ToLower();

            //Commands begin Here
            if (command.Equals("ping"))
            {
                message.Channel.SendMessageAsync($@"Pong! {message.Author.Mention}");
            }
            else if (command.Equals("age"))
            {
                message.Channel.SendMessageAsync($@"Your Account was created at {message.Author.CreatedAt.DateTime.Date}");
            }
            else if (command.Equals("help"))
            {
                message.Channel.SendMessageAsync($@"Here is your list of commands that you can use {message.Author.Mention}");
                // message.Channel.SendMessageAsync("HelpCommand.txt");
                message.Channel.SendMessageAsync("```1. _help - This command shows you the list of commands you can use" +
                    "\n2. _ping - Pong! " +
                    "\n3. _age - Shows when you account was first created```   ");
            }


            return Task.CompletedTask;
        }
    }
}
