
using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Crayon.Output;

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
        string responseString = "WNCError1";
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
                    Console.WriteLine(Red("Username can't be empty!"));
                }
                else
                {
                    if (Username.Length > 20)
                    {
                        Console.WriteLine(Red("Username can't be that long!"));
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
            var cts = new CancellationTokenSource();
            bool networksuccess = true ;

            try
            {
                var response = await client.PostAsync(Server + "app/startup.php", content);
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
                networksuccess = false;
                Console.WriteLine(Red("Could not connect to server: " + Bold(Server)));
                Console.WriteLine(Red("Check your internet connection or the server's status..."));
                Console.WriteLine();
            }

            if (networksuccess == true)
            {
                //Console.WriteLine(responseString); debug only
                break;
            }
        }

        string responsedataObject = JsonConvert.DeserializeObject(responseString).ToString();
        dynamic responsedata = JObject.Parse(responsedataObject);
        string verifiedusername = responsedata.user.username.ToString();
        string pfp = responsedata.user.pfp.ToString();
        string temp = responsedata.weather.temp.ToString();
        string type = responsedata.weather.desc.ToString();
        string city = responsedata.weather.city.ToString();
        void printPfp(string pfp){
            string[] lines = pfp.Split('n');
            for (int i = 0; i < lines.Length; i++)
            {
                Console.WriteLine();
                string[] pixels = lines[i].Split(';');
                for (int l = 0; l < pixels.Length-1; l++)
                {
                    string[] rgb = pixels[l].Split(',');
                    Console.Write(Rgb(byte.Parse(rgb[0]), byte.Parse(rgb[1]), byte.Parse(rgb[2])).Text("█"));
                }
            }
        }
        printPfp(pfp);
        Console.WriteLine();
        Console.WriteLine("Welcome, " + Bold(verifiedusername));
        Console.WriteLine($"The weather in {city} is {type} at {temp} Degrees Celsius");
        Console.WriteLine();

        void printHelp()
        {
            void print(string command, string[] parameters, string description)
            {
                Console.WriteLine(" " + command + " {" + String.Join("} {", parameters) + "} - " + Rgb(120, 120, 120).Text(description));
            }
            Console.WriteLine("List of commands:");
            print("get", new string[]{"file_name"}, "Downloads and displays the specified file from your account.");
            print("list", new string[]{}, "Displays a list of your existing files.");
            print("create", new string[]{"file_name", "content"}, "Creates a new file with the specified name containing the content if specified.");
            print("delete", new string[]{"file_name"}, "Deletes permamently the specified file from your account.");
            print("help", new string[]{}, "Displays this list.");
            print("exit", new string[]{}, "Exits the application.");
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