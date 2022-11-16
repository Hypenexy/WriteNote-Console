
using System;
using System.Net;
using Newtonsoft.Json;

internal class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static async Task Main(string[] args)
    {
        string Version = "Console-1.0";
        var Settings = new Dictionary<string, string>
        {
            { "version", Version}
        };
        //string Server = "https://writenote.midelight.net";
        string Server = "http://localhost/WriteNoteApp/";
        while (true)
        {
            Console.WriteLine("WriteNote Command Interface");
            Console.WriteLine("Midelight account login");
            string Username = "";
            while (true)
            {
                Console.Write("Username: ");
                Username = Console.ReadLine();
                if (Username.Length < 1) {
                    Console.WriteLine("Username can't be empty!");
                }
                else
                {
                    if (Username.Length > 20)
                    {
                        Console.WriteLine("Username can't be that long!");
                    }
                    else
                    {
                        //maybe check for special characters some day..
                        break;
                    }
                }
            }
            Console.Write("Password: ");
            string Password = "";
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && Password.Length > 0)
                {
                    Password = Password.Remove(Password.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace || key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.RightArrow)
                    {
                        //preventDefault() here
                    }
                    else
                    {
                        Password += key.KeyChar;
                        Console.Write('*');
                    }
                }
            }
            Console.WriteLine();
            var values = new Dictionary<string, string>
            {
                {"version", Version },
                {"username", Username },
                {"password", Password }
            };
            var content = new FormUrlEncodedContent(values);
            string responseString = "WNCError1";
            var cts = new CancellationTokenSource();
            bool networksuccess = true ;

            try
            {
                var response = await client.PostAsync(Server + "app/getnotes.php", content);
                responseString = await response.Content.ReadAsStringAsync();
                responseString = JsonConvert.SerializeObject(responseString);
            }
            catch (WebException ex)
            {
                // handle web exception
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken == cts.Token)
                {
                    // a real cancellation, triggered by the caller
                }
                else
                {
                    // a web request timeout
                }
            }
            catch
            {
                //networksuccess = false; DEBUG PURPOSE ONLY
                //Console.WriteLine("Could not connect to server: " + Server);
                //Console.WriteLine("Check your internet connection or the server's status...");
                //Console.WriteLine();
            }

            if (networksuccess == true)
            {
                break;
            }
            //Console.WriteLine(responseString);
        }

        var receivedUsername = "Hypenexy";
        Console.WriteLine();
        Console.WriteLine("Welcome, " + receivedUsername);
        Console.WriteLine("The time in Plovdiv is 24.4 Degrees Celsius with a light mist");
        Console.WriteLine();

        void printHelp()
        {
            Console.WriteLine("List of commands:");
            Console.WriteLine(" get {file_name} - Downloads and displays the specified file from your account.");
            Console.WriteLine(" create {file_name} {content} - Creates a new file with the specified name containing the content if specified.");
            Console.WriteLine(" delete {file_name} - Deletes permamently the specified file from your account.");
            Console.WriteLine(" help - Displays this list.");
            Console.WriteLine(" exit - Exits the application.");
            Console.WriteLine();
        }
        void exitApp()
        {
            Console.WriteLine("Goodbye");
            Environment.Exit(0);
        }
        void getFile(string filename)
        {

        }
        void createFile(string filename, string content)
        {

        }
        void deleteFile(string filename)
        {

        }

        printHelp();

        while (true)
        {
            string input = Console.ReadLine();
            string[] commands = input.Split(' ');
            switch (commands[0])
            {
                case "get":
                    getFile(commands[1]);
                    break;
                case "create":
                    createFile(commands[1], commands[2]);
                    break;
                case "delete":
                    deleteFile(commands[1]);
                    break;
                case "help":
                    printHelp();
                    break;
                case "?":
                    printHelp();
                    break;
                case "exit":
                    exitApp();
                    break;
                case "quit":
                    exitApp();
                    break;
                default:
                    Console.WriteLine("Unknown command. Type ? for help.");
                    break;
            }
        }
    }
}