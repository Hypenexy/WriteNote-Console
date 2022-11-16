
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
        var response = await client.PostAsync(Server + "app/getnotes.php", content);
        var responseString = await response.Content.ReadAsStringAsync();

        responseString = JsonConvert.SerializeObject(responseString);
        Console.WriteLine(responseString);
    }
}