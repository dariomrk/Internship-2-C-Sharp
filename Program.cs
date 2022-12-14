
#region Vars
// Predefined players.
Dictionary<string, (string Position, int Rating)> players = new()
{
    {"Luka Modric",("MF",88)},
    {"Marcelo Brozovic",("DF",86)},
    {"Mateo Kovacic",("MF",84)},
    {"Ivan Perisic",("MF",84)},
    {"Andrej Kramaric",("FW", 82)},
    {"Ivan Rakitic",("MF", 82)},
    {"Josko Gvardiol",("DF", 81)},
    {"Mario Pasalic",("MF", 81)},
    {"Lovro Majer",("MF", 80)},
    {"Dominik Livakovic",("GK", 80)},
    {"Ante Rebic",("FW", 80)},
    {"Josip Brekalo",("MF", 79)},
    {"Borna Sosa",("DF", 78)},
    {"Nikola Vlasic",("MF", 78)},
    {"Duje Caleta-Car",("DF", 78)},
    {"Dejan Lovren",("DF", 78)},
    {"Mislav Orsic",("FW", 77)},
    {"Marko Livaja",("FW", 77)},
    {"Domagoj Vida",("DF", 76)},
    {"Ante Budimir",("FW", 76)},
};

// Used to track goals per player.
Dictionary<string, int> playersGoals = new();

// Used to track stats for each team.
Dictionary<string, (int Points, int Goals, int GoalDiff)> groupTeams = new()
{
    {"Croatia",(0,0,0) },
    {"Morocco",(0,0,0) },
    {"Belgium",(0,0,0) },
    {"Canada",(0,0,0) },
};

// Predefined valid player positions.
string[] validPositions = new string[] { "GK", "DF", "MF", "FW" };

// Predefined matches. Used to track match stats.
(string Team1, string Team2, int Score1, int Score2, bool isOver)[] groupMatches =
{
    ("Morocco","Croatia",0,0,false),
    ("Belgium","Canada",0,0,false),
    ("Belgium","Morocco",0,0,false),
    ("Croatia","Canada",0,0,false),
    ("Croatia","Belgium",0,0,false),
    ("Canada","Morocco",0,0,false),
};

#endregion

#region Data validation & sanitization
void ValidatePlayerName(string name)
{
    if (name == null)
        throw new("Name cannot be null!");
    if (name.Trim() == "")
        throw new("Name cannot be empty!");
}

void ValidatePlayerPosition(string position)
{
    if (position == null)
        throw new("Position cannot be null!");
    if (position.Trim() == "")
        throw new("Position cannot be empty!");
    if (!(validPositions.Contains(position)))
        throw new("Position is not valid!");
}

void ValidatePlayerRating(int rating)
{
    if (rating > 100)
        throw new("Rating cannot exceed 100!");
    if (rating < 0)
        throw new("Rating cannot subceed 0!");
}

int SanitizePlayerRating(int rating)
{
    if (rating > 100)
        return 100;
    if (rating < 0)
        return 0;
    return rating;
}

#endregion

#region Input & Output utilities
void WaitForUser()
{
    Console.WriteLine("Pritisnite bilo koju tipku za nastavak...");
    Console.ReadKey();
}

void WarnBadUserInput()
{
    Console.WriteLine("Unesena opcija nije validna!");
    WaitForUser();
}

void WarnReturnToMainMenu(string message)
{
    Console.WriteLine($"{message} Povratak na glavni meni...");
    WaitForUser();
}

int OutputMenu(string[] options)
{
    while (true)
    {
        Console.Clear();
        for (int i = 0; i<options.Length; i++)
        {
            Console.WriteLine($"{i,-2} -  {options[i]}");
        }

        Console.Write("Unesite odabranu opciju: ");
        if (!int.TryParse(Console.ReadLine(), out int userInput))
        {
            WarnBadUserInput();
            continue;
        }
        if (userInput < 0 || userInput > options.Length - 1)
        {
            WarnBadUserInput();
            continue;
        }
        return userInput;
    }
}

void OutputPlayedMatches()
{
    Console.Clear();
    Console.WriteLine("Odigrane utakmice:");
    foreach (var match in groupMatches)
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
        Console.WriteLine($"| {player.Name,-21}| {player.Position,-9}| {player.Rating,-7}|");
    }
}

bool UserConfirmation(string message)
{
    Console.Clear();
    while (true)
    {
        Console.WriteLine($"{message}");
        Console.Write($"Unesite Y za da ili N za ne: ");
        string userInput = Console.ReadLine();

        if (userInput.ToUpper() == "Y")
            return true;
        else if (userInput.ToUpper() == "N")
            return false;
        else
        {
            Console.Clear();
            Console.WriteLine("Neispravan unos! Pokusajte ponovo...");
        }
    }
}

#endregion

#region Data manipulation & generation
void SyncPlayerGoals()
{
    foreach (var player in players)
        if (player.Value.Position == "FW")
        {
            if (!playersGoals.ContainsKey(player.Key))
            {
                playersGoals.Add(player.Key, 0);
            }
        }
}

(string Name, string Position, int Rating)[]
    PlayersToArray()
{
    List<(string Name, string Position, int Rating)> export = new();

    foreach (var player in players)
    {
        export.Add((player.Key, player.Value.Position, player.Value.Rating));
    }
    return export.ToArray();
}

(string Name, string Position, int Rating)[]
    PlayersToSortedArray()
{
    List<(string Name, string Position, int Rating)> players = new(PlayersToArray());
    players.Sort((e1, e2) => e2.Rating.CompareTo(e1.Rating));
    return players.ToArray();
}

(string Name, string Position, int Rating)[]
    PlayersByPosition(
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

(string Name, string Position, int Rating)[]
    PlayersLineup()
{
    List<(string Name, string Position, int Rating)> selected = new();

    selected.AddRange(PlayersByPosition(PlayersToSortedArray(), "GK", 1));
    selected.AddRange(PlayersByPosition(PlayersToSortedArray(), "DF", 4));
    selected.AddRange(PlayersByPosition(PlayersToSortedArray(), "MF", 3));
    selected.AddRange(PlayersByPosition(PlayersToSortedArray(), "FW", 3));

    return selected.ToArray();
}

int RandomScore()
{
    // Statistics info:
    // https://docs.bvsalud.org/biblioref/2018/12/965586/goal-scoring-frequency-in-soccer-in-different-age-groups.pdf
    // Box-Muller transform:
    // https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
    // Score is generated using the statistics information from the
    // referenced document and the Box-Muller transform.

    double mean = 2.43;
    double stdDeviation = 1.41;
    Random rand = new();
    double u1 = 1.0 - rand.NextDouble();
    double u2 = 1.0 - rand.NextDouble();
    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
    double randNormal = mean + stdDeviation * randStdNormal;
    return (int)Math.Round(Math.Abs(randNormal));
}

(string Team1, string Team2, int Score1, int Score2, bool isOver)
    GenerateAndUpdateMatchData(
    string team1,
    string team2)
{
    (string Team1, string Team2, int Score1, int Score2, bool isOver) output = (team1, team2, 0, 0, true);
    output.Score1 = RandomScore();
    output.Score2 = RandomScore();

    groupTeams[team1] = (groupTeams[team1].Points, groupTeams[team1].Goals + output.Score1, groupTeams[team1].Goals + output.Score1 - output.Score2);
    groupTeams[team2] = (groupTeams[team2].Points, groupTeams[team2].Goals + output.Score2, groupTeams[team2].Goals + output.Score2 - output.Score1);

    if (output.Score1 > output.Score2)
    {
        groupTeams[team1]=(groupTeams[team1].Points+3, groupTeams[team1].Goals, groupTeams[team1].GoalDiff);
    }
    else if (output.Score1 < output.Score2)
    {
        groupTeams[team2]=(groupTeams[team2].Points+3, groupTeams[team2].Goals, groupTeams[team2].GoalDiff);
    }
    else
    {
        groupTeams[team1]=(groupTeams[team1].Points+1, groupTeams[team1].Goals, groupTeams[team1].GoalDiff);
        groupTeams[team2]=(groupTeams[team2].Points+1, groupTeams[team2].Goals, groupTeams[team2].GoalDiff);
    }

    return output;
}

(string Name, string Position, int Rating)[]
    AdjustRating(
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
                    playersGoals[lineup[i].Name]++;
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

void LineupToDict((string Name, string Position, int Rating)[] lineup)
{
    foreach (var player in lineup)
    {
        players[player.Name] = (player.Position, player.Rating);
    }
}

#endregion

#region Functionality
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
                PlayerControlMenu();
                break;

            default:
                break;
        }
    }
}

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

        Console.WriteLine(@$"| {name,-21}| {oldRating,-17}| {players[name].Rating,-12}| {players[name].Rating - oldRating,-8}|");
    }
    WaitForUser();
}

void Match()
{
    Console.Clear();
    var lineup = PlayersLineup();
    if (lineup.Length < 11)
    {
        Console.WriteLine("Nedovoljan broj igraca! Nije moguce odigrati utakmicu!");
        WaitForUser();
        return;
    }

    (string Team1, string Team2, int Score1, int Score2, bool isOver) match = ("", "", 0, 0, false);

    int i;
    for (i = 0; i < groupMatches.Length; i++)
    {
        if ((groupMatches[i].Team1 == "Croatia" || groupMatches[i].Team2 == "Croatia") && !groupMatches[i].isOver)
        {
            match = GenerateAndUpdateMatchData(groupMatches[i].Team1, groupMatches[i].Team2);
            break;
        }
        else if (!groupMatches[i].isOver)
        {
            groupMatches[i] = GenerateAndUpdateMatchData(groupMatches[i].Team1, groupMatches[i].Team2);
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
        LineupToDict(lineup);
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
        LineupToDict(lineup);
    }

    if (i < groupMatches.Length)
    {
        groupMatches[i] = match;
    }
    else
    {
        Console.WriteLine("Sve utakmice su odigrane!");
        WaitForUser();
    }

    OutputPlayedMatches();
    WaitForUser();
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
        "Ispis igraca po poziciji",
        "Ispis postave",
        "Ispis strijelaca",
        "Rezultati Hrvatske",
        "Rezultati svih ekipa",
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
            OutputPlayersArray(PlayersToSortedArray());
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
            {
                Console.Clear();
                var playersSorted = PlayersToSortedArray();
                int groupRating = 100;
                foreach (var player in playersSorted)
                {
                    if (player.Rating < groupRating)
                    {
                        groupRating = player.Rating;
                        Console.WriteLine($"{groupRating}:");
                    }
                    Console.WriteLine($" >>>{player.Name}");
                }
                WaitForUser();
            }
            break;

        case 6:
            {
                Console.Clear();
                var playersArray = PlayersToArray();
                foreach (string position in validPositions)
                {
                    Console.WriteLine($"{position}:");
                    foreach (var player in playersArray)
                    {
                        if (player.Position == position)
                            Console.WriteLine($" >>>{player.Name}");
                    }
                }
                WaitForUser();
            }
            break;

        case 7:
            {
                OutputPlayersArray(PlayersLineup());
                WaitForUser();
            }
            break;

        case 8:
            {
                Console.Clear();
                Console.WriteLine($"| {"Ime i prezime",-21}| {"Broj golova",-12}|");
                foreach (var fw in playersGoals)
                    Console.WriteLine($"| {fw.Key,-21}| {fw.Value,-12}|");
                WaitForUser();
            }
            break;
        case 9:
            {
                Console.Clear();
                Console.WriteLine("Ispis rezultata Hrvatske:");
                foreach (var match in groupMatches)
                    if ((match.Team1 == "Croatia" || match.Team2 == "Croatia") && match.isOver)
                        Console.WriteLine($"{match.Team1} {match.Score1}:{match.Score2} {match.Team2}");
                WaitForUser();
            }
            break;

        case 10:
            {
                Console.Clear();
                Console.WriteLine($"| {"Ekipa",-10}| {"Bodovi",-7}|");
                foreach (var team in groupTeams)
                    Console.WriteLine($"| {team.Key,-10}| {team.Value.Points,-7}|");
                WaitForUser();
            }
            break;

        case 11:
            {
                List<(string Name, int Points, int Goals, int GoalDiff)> teamScoresList = new();
                foreach (var team in groupTeams)
                {
                    teamScoresList.Add((team.Key, team.Value.Points, team.Value.Goals, team.Value.GoalDiff));
                }
                teamScoresList.Sort((e1, e2) =>
                {
                    if (e1.Points != e2.Points)
                        return e2.Points.CompareTo(e1.Points);
                    return e2.Name.CompareTo(e1.Name);
                }
                );

                Console.Clear();
                Console.WriteLine($"| {"Rank",-5}| {"Ekipa",-10}| {"Bodovi",-7}| {"Gol razlika",-12}|");
                for (int i = 0; i < teamScoresList.Count; i++)
                {
                    (string Name, int Points, int Goals, int GoalDiff) team = teamScoresList[i];
                    Console.WriteLine($"| {$"{i+1}.",-5}| {team.Name,-10}| {team.Points,-7}| {team.GoalDiff,-12}|");
                }
                WaitForUser();
            }
            break;

        default:
            Console.WriteLine("Neispravan unos! Povratak na glavni meni...");
            WaitForUser();
            break;
    }
}

void PlayerControlMenu()
{
    string[] playerControlMenuOptions =
    {
        "Povratak na glavni meni",
        "Unos novog igraca",
        "Brisanje igraca",
        "Uredi ime i prezime igraca",
        "Uredi poziciju igraca",
        "Uredi rating igraca",
    };

    int userSelection = OutputMenu(playerControlMenuOptions);

    switch (userSelection)
    {
        case 1:
            {
                Console.Clear();

                if (players.Keys.Count == 26)
                {
                    WarnReturnToMainMenu("Ekipa je puna! Nije moguce dodavati nove igrace.");
                    return;
                }

                Console.Write("Unesite ime i prezime novog igraca: ");
                string newPlayerName = Console.ReadLine();

                try
                {
                    ValidatePlayerName(newPlayerName);
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravno ime!");
                    return;
                }

                if (players.ContainsKey(newPlayerName))
                {
                    WarnReturnToMainMenu($"Nije moguce dodati igraca {newPlayerName} kako isti vec postoji!");
                    return;
                }
                (string Position, int Rating) newPlayerInfo = ("", 0);

                Console.Write("Unesite poziciju igraca: ");
                string positionUserInput = Console.ReadLine();

                try
                {
                    ValidatePlayerPosition(positionUserInput);
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu("Pozicija nije validna!");
                    return;
                }

                newPlayerInfo.Position = positionUserInput;

                Console.Write("Unesite rating igraca: ");
                string ratingUserInput = Console.ReadLine();

                try
                {
                    newPlayerInfo.Rating = int.Parse(ratingUserInput);
                    ValidatePlayerRating(newPlayerInfo.Rating);
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu("Rating nije validan!");
                    return;
                }

                players.Add(newPlayerName, newPlayerInfo);
                SyncPlayerGoals();
                WarnReturnToMainMenu("Novi igrac je dodan!");
            }
            break;

        case 2:
            {
                Console.Clear();
                Console.Write("Unesite ime i prezime igraca kojeg zelite obrisati: ");

                string playerName = Console.ReadLine();

                try
                {
                    ValidatePlayerName(playerName);
                    if (!players.ContainsKey(playerName))
                        throw new();
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravno ime!");
                    return;
                }

                if (!UserConfirmation($"Zelite li sigurno obrisati igraca: {playerName}?"))
                {
                    WarnReturnToMainMenu($"Otkazano!");
                    return;
                }
                players.Remove(playerName);
                playersGoals.Remove(playerName);
                WarnReturnToMainMenu($"Igrac obrisan!");
            }
            break;

        case 3:
            {
                Console.Clear();
                Console.Write("Unesite ime i prezime igraca kojem zelite urediti ime i prezime: ");
                string playerName = Console.ReadLine();

                try
                {
                    ValidatePlayerName(playerName);
                    if (!players.ContainsKey(playerName))
                        throw new();
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravno ime!");
                    return;
                }

                Console.Write("Unesite novo ime i prezime igraca: ");
                string newPlayerName = Console.ReadLine();
                try
                {
                    ValidatePlayerName(newPlayerName);
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravno ime!");
                    return;
                }
                if (!UserConfirmation($"Zelite li sigurno izmjeniti ime igraca: {playerName} -> {newPlayerName}?"))
                {
                    WarnReturnToMainMenu($"Otkazano!");
                    return;
                }

                var info = players[playerName];
                players.Remove(playerName);
                players.Add(newPlayerName, info);

                if (info.Position == "FW")
                {
                    var goalsInfo = playersGoals[playerName];
                    playersGoals.Remove(playerName);
                    playersGoals.Add(newPlayerName, goalsInfo);
                }
                WarnReturnToMainMenu($"Igrac izmjenjen!");
            }
            break;
        case 4:
            {
                Console.Clear();
                Console.Write("Unesite ime i prezime igraca kojem zelite urediti poziciju: ");
                string playerName = Console.ReadLine();
                try
                {
                    ValidatePlayerName(playerName);
                    if (!players.ContainsKey(playerName))
                        throw new();
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravno ime!");
                    return;
                }
                Console.Write("Unesite novu poziciju: ");
                string newPlayerPosition = Console.ReadLine();
                try
                {
                    ValidatePlayerPosition(newPlayerPosition);
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravna pozicija!");
                    return;
                }
                if (!UserConfirmation($"Zelite li sigurno izmjeniti poziciju: {players[playerName].Position} -> {newPlayerPosition}?"))
                {
                    WarnReturnToMainMenu($"Otkazano!");
                    return;
                }
                var info = players[playerName];
                info.Position = newPlayerPosition;
                players[playerName] = info;
                WarnReturnToMainMenu($"Pozicija izmjenjena!");
                SyncPlayerGoals();
            }
            break;

        case 5:
            {
                Console.Clear();
                Console.Write("Unesite ime i prezime igraca kojem zelite urediti rating: ");
                string playerName = Console.ReadLine();
                try
                {
                    ValidatePlayerName(playerName);
                    if (!players.ContainsKey(playerName))
                        throw new();
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravno ime!");
                    return;
                }
                Console.Write("Unesite novi rating: ");
                string userInput = Console.ReadLine();
                int newPlayerRating;
                try
                {
                    newPlayerRating = int.Parse(userInput);
                    ValidatePlayerRating(newPlayerRating);
                }
                catch (Exception)
                {
                    WarnReturnToMainMenu($"Neispravan rating!");
                    return;
                }
                if (!UserConfirmation($"Zelite li sigurno izmjeniti rating: {players[playerName].Rating} -> {newPlayerRating}?"))
                {
                    WarnReturnToMainMenu($"Otkazano!");
                    return;
                }
                var info = players[playerName];
                info.Rating = newPlayerRating;
                players[playerName] = info;
                WarnReturnToMainMenu($"Rating izmjenjen!");
            }
            break;

        default:
            WarnReturnToMainMenu("Neispravan unos!");
            break;
    }
}

# endregion

// App
SyncPlayerGoals();
MainMenu();
