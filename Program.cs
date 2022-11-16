
// Vars
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
int currentMatch = -1;
(string Team1, string Team2, int Score1, int Score2, bool isOver)[] matchesGroupF = 
{
    ("Morocco","Croatia",0,0,false), // Match 1
    ("Belgium","Canada",0,0,false),
    ("Belgium","Morocco",0,0,false),
    ("Croatia","Canada",0,0,false), // Match 2
    ("Croatia","Belgium",0,0,false), // Match 3
    ("Canada","Morocco",0,0,false),
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
    if (!((new string[] { "GK", "DF", "MF", "FW" }).Contains(position)))
        throw new Exception("Position is not valid!");
}
void ValidatePlayerRating(int rating)
{
    if (rating > 100)
        throw new Exception("Rating cannot exceed 100!");
    if (rating < 0)
        throw new Exception("Rating subceed 0!");
}

// Data sanitization
int SanitizePlayerRating(int rating)
{
    if(rating > 100)
        return 100;
    if (rating < 0)
        return 0;
    return rating;
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
        if (!int.TryParse(Console.ReadLine(), out int userInput))
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

// Data manipulation
(string Name, string Position, int Rating)[] PlayersToArray()
{
    List<(string Name, string Position, int Rating)> export = new();

    foreach (var player in players)
    {
        export.Add((player.Key, player.Value.Position, player.Value.Rating));
    }
    return export.ToArray();
}
(string Name, string Position, int Rating)[] SelectBest11()
{
    List<(string Name, string Position, int Rating)> players = new(PlayersToArray());
    players.Sort((e1, e2) => e2.Rating.CompareTo(e1.Rating));
    return players.GetRange(0, 11).ToArray();
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
                Match();
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
    Console.WriteLine($"| {"Ime i prezime",-21}| {"Prethodni rating",-17}| {"Novi rating",-13}| {"Razlika",-9}|");
    for (int i= 0; i < players.Count; i++)
    {
        var name = players.Keys.ElementAt(i);
        int oldRating = players[name].Rating;

        Random r = new();
        int diff = (int)(r.Next(-5, 6) * 0.01 * oldRating);
        players[name] = (players[name].Position,SanitizePlayerRating(diff + oldRating));

        Console.WriteLine(@$"| {name,-21}| {oldRating,-17}| {players[name].Rating,-13}| {diff,-9}|");
    }
    WaitForUser();
}

void Match()
{
    Console.Clear();
    
    WaitForUser();
}

// App
MainMenu();
