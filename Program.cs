
// Vars
string[] allowedPositions = { "GK", "DF", "MF", "FW" }; // who needs an enum anyway?
Dictionary<string, (string Position, int Rating)> players = new()
{
    {"Luka Modrić",("MF",88)},
    {"Marcelo Brozović",("DF",86)},
    {"Mateo Kovačić",("MF",84)},
    {"Ivan Perišić",("MF",84)},
    {"Andrej Kramarić",("FW", 82)},
    {"Ivan Rakitić",("MF", 82)},
    {"Joško Gvardiol",("DF", 81)},
    {"Mario Pašalić",("MF", 81)},
    {"Lovro Majer",("MF", 80)},
    {"Dominik Livaković",("GK", 80)},
    {"Ante Rebić",("FW", 80)},
    {"Josip Brekalo",("MF", 79)},
    {"Borna Sosa",("DF", 78)},
    {"Nikola Vlašić",("MF", 78)},
    {"Duje Ćaleta-Car",("DF", 78)},
    {"Dejan Lovren",("DF", 78)},
    {"Mislav Oršić",("FW", 77)},
    {"Marko Livaja",("FW", 77)},
    {"Domagoj Vida",("DF", 76)},
    {"Ante Budimir",("FW", 76)},
};

// Data validation
void ValidatePlayerName(string name)
{
    if (name == null)
        throw new Exception("Name cannot be null!");
    if (name.Trim() == "")
        throw new Exception("Name cannot be empty!");
    if (players.ContainsKey(name))
        throw new Exception("Name already exists!");
}
void ValidatePlayerPosition(string position)
{
    if (position == null)
        throw new Exception("Position cannot be null!");
    if (position.Trim() == "")
        throw new Exception("Position cannot be empty!");
    if (!(allowedPositions.Contains(position)))
        throw new Exception("Position is not valid!");
}
void ValidatePlayerRating(int rating)
{
    if (rating > 100)
        throw new Exception("Rating cannot exceed 100!");
    if (rating < 0)
        throw new Exception("Rating subceed 0!");
}

// Input & Output utilities
void WaitForUser()
{
    Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
    Console.ReadKey();
}
void BadUserInputWarning()
{
    Console.WriteLine("Unesena opcija nije validna!");
    WaitForUser();
}

int Menu(string[] options)
{
    while (true)
    {
        Console.Clear();
        for (int i = 0; i<options.Length; i++)
            Console.WriteLine($"{i} - {options[i]}");

        Console.Write("Unesite odabranu opciju: ");
        int userInput;
        if (!int.TryParse(Console.ReadLine(), out userInput))
        {
            BadUserInputWarning();
            continue;
        }
        if (userInput < 0 || userInput > options.Length - 1)
        {
            BadUserInputWarning();
            continue;
        }
        return userInput;
    }
}

// Menus
void MainMenu()
{
    string[] mainMenuOptions =
    {
        "Izlaz iz aplikacije",
        "Odradi trening",
        "Odigraj utakmicu",
        "Statistika",
        "Kontrola igraca",
    };

    while(true)
    {
        int userSelection = Menu(mainMenuOptions);

        switch (userSelection)
        {
            case 0:
                return;
            case 1:
                // Odradi trening
                Training();
                break;
            case 2:
                // Odigraj utakmicu
                break;
            case 3:
                //Statistika
                break;
            case 4:
                // Kontrola igraca
                break;
            default:
                break;
        }
    }
}

// Functionality
void Training()
{
    Console.Clear();
    Console.WriteLine($"| {"Ime i prezime".PadRight(21)}| {"Prethodni rating".PadRight(17)}| {"Novi rating".PadRight(13)}| {"Razlika".PadRight(9)}|");
    for (int i= 0; i < players.Count; i++)
    {
        var name = players.Keys.ElementAt(i);
        int oldRating = players[name].Rating;

        Random r = new Random();
        int diff = (int)(r.Next(-5, 6) * 0.01 * oldRating);
        players[name] = (players[name].Position,diff + oldRating);

        Console.WriteLine(@$"| {name.PadRight(21)}| {oldRating.ToString().PadRight(17)}| {players[name].Rating.ToString().PadRight(13)}| {diff.ToString().PadRight(9)}|");
    }
    WaitForUser();
}

// App
MainMenu();
