
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
void BadUserInputWarning()
{
    Console.WriteLine("Unesena opcija nije validna!\nPritisnite bilo koju tipku za ponovni pokusaj.");
    Console.ReadKey();
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

// App
MainMenu();
