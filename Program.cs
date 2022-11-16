
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
string[] validPositions = new string[] { "GK", "DF", "MF", "FW" };
int currentMatch = 0;
(string Team1, string Team2, int Score1, int Score2, bool isOver)[] matchesGroupF =
{
    ("Morocco","Croatia",0,0,false),
    ("Belgium","Canada",0,0,false),
    ("Belgium","Morocco",0,0,false),
    ("Croatia","Canada",0,0,false),
    ("Croatia","Belgium",0,0,false),
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
    if (!(validPositions.Contains(position)))
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
    if (rating > 100)
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
int OutputMenu(string[] options)
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
void OutputPlayedMatches()
{
    Console.Clear();
    Console.WriteLine("Odigrane utakmice:");
    foreach (var match in matchesGroupF)
        if (match.isOver)
        {
            Console.WriteLine($"{match.Team1} {match.Score1} : {match.Score2} {match.Team2}");
        }
}
void OutputPlayersArray((string Name, string Position, int Rating)[] players)
{
    Console.Clear();
    Console.WriteLine($"| {"Ime i prezime",-21}| {"Pozicija",-9}| {"Rating",-7}|");
    foreach (var player in players)
    {
        Console.WriteLine(@$"| {player.Name,-21}| {player.Position,-9}| {player.Rating,-7}|");
    }
}

// Data manipulation & generation
(string Name, string Position, int Rating)[] PlayersToArray()
{
    List<(string Name, string Position, int Rating)> export = new();

    foreach (var player in players)
    {
        export.Add((player.Key, player.Value.Position, player.Value.Rating));
    }
    return export.ToArray();
}
(string Name, string Position, int Rating)[] PlayersSorted()
{
    List<(string Name, string Position, int Rating)> players = new(PlayersToArray());
    players.Sort((e1, e2) => e2.Rating.CompareTo(e1.Rating));
    return players.ToArray();
}
(string Name, string Position, int Rating)[] SelectPlayersByPosition(
    (string Name, string Position, int Rating)[] players,
    string position,
    int count = 0)
{
    if (count == 0)
        count = players.Length;

    int currentCount = 0;

    List<(string Name, string Position, int Rating)> selected = new();

    for (int i = 0; i < players.Length; i++)
    {
        if (currentCount == count)
            break;
        if (players[i].Position == position)
        {
            selected.Add(players[i]);
            currentCount++;
        }
    }
    return selected.ToArray();
}
(string Name, string Position, int Rating)[] SelectLineup()
{
    List<(string Name, string Position, int Rating)> selected = new();

    selected.AddRange(SelectPlayersByPosition(PlayersSorted(), "GK", 1));
    selected.AddRange(SelectPlayersByPosition(PlayersSorted(), "DF", 4));
    selected.AddRange(SelectPlayersByPosition(PlayersSorted(), "MF", 3));
    selected.AddRange(SelectPlayersByPosition(PlayersSorted(), "FW", 3));

    return selected.ToArray();
}
int RandomScore()
{
    // Statistic information: https://docs.bvsalud.org/biblioref/2018/12/965586/goal-scoring-frequency-in-soccer-in-different-age-groups.pdf
    // Box-Muller transform: https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
    // Score is generated using the statistics information from the referenced document and the Box-Muller transform.
    double mean = 2.43;
    double stdDeviation = 1.41;
    Random rand = new();
    double u1 = 1.0 - rand.NextDouble();
    double u2 = 1.0 - rand.NextDouble();
    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
    double randNormal = mean + stdDeviation * randStdNormal;
    return (int)Math.Round(Math.Abs(randNormal));
}
(string Team1, string Team2, int Score1, int Score2, bool isOver) GenerateMatchData(
    string team1,
    string team2)
{
    (string Team1, string Team2, int Score1, int Score2, bool isOver) output = (team1, team2, 0, 0, true);
    output.Score1 = RandomScore();
    output.Score2 = RandomScore();
    return output;
}
(string Name, string Position, int Rating)[] AdjustRating(
    int hasWon,
    (string Name, string Position, int Rating)[] lineup,
    int numOfGoals)
{
    int assignedGoals = 0;

    while (assignedGoals < numOfGoals)
    {
        for (int i = 0; i < lineup.Length; i++)
        {
            if (assignedGoals == numOfGoals)
                break;

            if (lineup[i].Position == "FW")
            {
                Random r = new();
                if (r.NextDouble() > 0.5)
                {
                    lineup[i].Rating = SanitizePlayerRating(lineup[i].Rating + (int)(lineup[i].Rating * 0.05));
                    assignedGoals++;
                }
            }
        }
    }

    for (int i = 0; i < lineup.Length; i++)
    {
        if (hasWon == 1)
        {
            lineup[i].Rating = SanitizePlayerRating(lineup[i].Rating + (int)(lineup[i].Rating * 0.02));
        }
        else if (hasWon == -1)
        {
            lineup[i].Rating = SanitizePlayerRating(lineup[i].Rating - (int)(lineup[i].Rating * 0.02));
        }
    }

    return lineup;
}
Dictionary<string, (string Position, int Rating)> LineupToDict(
    (string Name, string Position, int Rating)[] lineup,
    Dictionary<string, (string Position, int Rating)> players)
{
    Dictionary<string, (string Position, int Rating)> modifiedPlayers = new(players);
    foreach (var player in lineup)
    {
        modifiedPlayers[player.Name] = (player.Position,player.Rating);
    }
    return modifiedPlayers;
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

    while (true)
    {
        int userSelection = OutputMenu(mainMenuOptions);

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
                StatisticsMenu();
                break;

            case 4:
                // Kontrola igraca
                break;

            default:
                break;
        }
    }
}
void StatisticsMenu()
{
    string[] statisticsMenuOptions =
    {
        "Povratak na glavni meni",
        "Ispis igraca",
        "Ispis po ratingu uzlazno",
        "Ispis po ratingu silazno",
        "Ispis igraca po imenu i prezimenu uzlazno",
        "Ispis igraca po ratingu",
        "Ispos igraca po poziciji",
        "Ispis postave",
        "Ispis strijelaca",
        "Rezultati po ekipi",
        "Ispis tablice grupe",
    };

    int userSelection = OutputMenu(statisticsMenuOptions);

    switch (userSelection)
    {
        case 0:
            return;

        case 1:
            OutputPlayersArray(PlayersToArray());
            WaitForUser();
            break;

        case 2:
            OutputPlayersArray(PlayersSorted());
            WaitForUser();
            break;

        case 3:
            OutputPlayersArray(PlayersToArray().Reverse().ToArray());
            WaitForUser();
            break;

        case 4:
            List<(string Name, string Position, int Rating)> playersList = new(PlayersToArray());
            playersList.Sort((e1, e2) => e1.Name.CompareTo(e2.Name));
            OutputPlayersArray(playersList.ToArray());
            WaitForUser();
            break;

        case 5:
            Console.Clear();
            var playersSorted = PlayersSorted();
            int groupRating = 100;
            foreach(var player in playersSorted)
            {
                if(player.Rating < groupRating)
                {
                    groupRating = player.Rating;
                    Console.WriteLine($"{groupRating}:");
                }
                Console.WriteLine($" >>>{player.Name}");
            }
            WaitForUser();
            break;

        case 6:
            Console.Clear();
            var playersArray = PlayersToArray();
            foreach(string position in validPositions)
            {
                Console.WriteLine($"{position}:");
                foreach (var player in playersArray)
                {
                    if(player.Position == position)
                        Console.WriteLine($" >>>{player.Name}");
                }
            }
            WaitForUser();
            break;

        case 7:

            break;

        default:
            break;
    }
}

// Functionality
void Training()
{
    Console.Clear();
    Console.WriteLine($"| {"Ime i prezime",-21}| {"Prethodni rating",-17}| {"Novi rating",-12}| {"Razlika",-8}|");
    for (int i = 0; i < players.Count; i++)
    {
        var name = players.Keys.ElementAt(i);
        int oldRating = players[name].Rating;

        Random r = new();
        int diff = (int)(r.Next(-5, 6) * 0.01 * oldRating);
        players[name] = (players[name].Position, SanitizePlayerRating(diff + oldRating));

        Console.WriteLine(@$"| {name,-21}| {oldRating,-17}| {players[name].Rating,-12}| {diff,-8}|");
    }
    WaitForUser();
}
void Match()
{
    Console.Clear();
    var lineup = SelectLineup();
    if (lineup.Length < 11)
    {
        Console.WriteLine("Nedovoljan broj igraca! Nije moguce odigrati utakmicu!");
        WaitForUser();
        return;
    }

    (string Team1, string Team2, int Score1, int Score2, bool isOver) match = ("", "", 0, 0, false);

    int i;
    for (i = 0; i < matchesGroupF.Length; i++)
    {
        if ((matchesGroupF[i].Team1 == "Croatia" || matchesGroupF[i].Team2 == "Croatia") && !matchesGroupF[i].isOver)
        {
            match = GenerateMatchData(matchesGroupF[i].Team1, matchesGroupF[i].Team2);
            break;
        }
        else if (!matchesGroupF[i].isOver)
        {
            matchesGroupF[i] = GenerateMatchData(matchesGroupF[i].Team1, matchesGroupF[i].Team2);
            OutputPlayedMatches();
            WaitForUser();
            return;
        }
    }

    if (match.Team1 == "Croatia")
    {
        int hasWon = 0;
        if (match.Score1 > match.Score2)
            hasWon = 1;
        else if (match.Score1 == match.Score2)
            hasWon = 0;
        else
            hasWon = -1;
        lineup = AdjustRating(hasWon, lineup, match.Score1);
        players = LineupToDict(lineup, players);
    }
    else
    {
        int hasWon = 0;
        if (match.Score1 < match.Score2)
            hasWon = 1;
        else if (match.Score1 == match.Score2)
            hasWon = 0;
        else
            hasWon = -1;
        lineup = AdjustRating(hasWon, lineup, match.Score2);
        players = LineupToDict(lineup,players);
    }

    if(i < matchesGroupF.Length)
    {
        matchesGroupF[i] = match;
    }
    else
    {
        Console.WriteLine("Sve utakmice su odigrane!");
        WaitForUser();
    }

    OutputPlayedMatches();
    WaitForUser();
}

// App
MainMenu();
