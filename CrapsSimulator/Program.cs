// See https://aka.ms/new-console-template for more information

using CrapsSimulator;
using CrapsSimulator.Strategies;

Random random1 = new();
System.Threading.Thread.Sleep(1000);
Random random2 = new();

int diceRoll = 0;
int point = 0;
RollType rollType = RollType.ComeOutRoll;

HashSet<int> PointRolls = new() { 4, 5, 6, 8, 9, 10 };
HashSet<int> ComeOutRollCraps = new() { 2, 3, 12 };
HashSet<int> ComeOutRollWins = new() { 7, 11 };

IStrategy strategy = new NoPassLayOddsStrategy();
List<string> Log = new();

#region Game loop
int shots = 0;

AddLog($"Start game: {strategy.GetType()}");

while (shots < Constants.SHOTS_MAX)
{
    // Roll dice.
    RollDice();

    await UpdateState();
}

await WriteStats();
#endregion

#region Game functions
void RollDice()
{
    int d1 = random1.Next(Constants.DIE_MIN, Constants.DIE_MAX + 1);
    int d2 = random2.Next(Constants.DIE_MIN, Constants.DIE_MAX + 1);

    diceRoll = d1 + d2;
}

async Task UpdateState()
{
    if (rollType == RollType.ComeOutRoll)
    {
        shots++;

        if (PointRolls.Contains(diceRoll))
        {
            // Point established.
            point = diceRoll;
            rollType = RollType.PointRoll;
        }
        else if (ComeOutRollCraps.Contains(diceRoll))
        {
            // Craps.
            await strategy.ResolveComeOutCraps(diceRoll == 12);
            point = 0;
        }
        else if (ComeOutRollWins.Contains(diceRoll))
        {
            // Natural win.
            await strategy.ResolveComeOutWin();
            point = 0;
        }
    }
    else if (rollType == RollType.PointRoll)
    {
        if (diceRoll == point)
        {
            // Point match.
            await strategy.ResolvePointMatch(point);

            point = 0;
            rollType = RollType.ComeOutRoll;
        }
        else if (diceRoll == 7)
        {
            // Point craps.
            await strategy.ResolvePointCraps(point);

            point = 0;
            rollType = RollType.ComeOutRoll;
        }
    }

    AddLog("");
}


void AddLog(string message)
{
    Log.Add($"{strategy.Bankroll}\t{diceRoll}\t{message}");
}

async Task WriteStats()
{
    using FileStream fileStream = File.Open("stats.txt", FileMode.Create);
    using StreamWriter streamWriter = new(fileStream);

    foreach (string entry in Log)
    {
        await streamWriter.WriteLineAsync(entry);
    }
}
#endregion

enum GameState
{
    Off,
    On
}

static class Constants
{
    public const int SHOTS_MAX = 2000;

    public const int DIE_MIN = 1;
    public const int DIE_MAX = 6;
}