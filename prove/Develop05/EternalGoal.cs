// ETERNAL GOAL
// Never truly "done", you just keep doing it and racking up
// points. Example: "Read scriptures" for 100 points each time.
class EternalGoal : Goal
{
    private int _timesCompleted;

    public EternalGoal(string name, string description, int points)
        : base(name, description, points)
    {
        _timesCompleted = 0;
    }

    // loading constructor 
    public EternalGoal(string name, string description, int points, int timesCompleted)
        : base(name, description, points)
    {
        _timesCompleted = timesCompleted;
    }

    // every time you record it, you just get the points — it has no finish line
    public override int RecordEvent()
    {
        _timesCompleted++;
        Console.WriteLine($"  You earned {Points} points!");
        return Points;
    }

    public override void PrintDetails()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  [∞] {Name} ({Description}) — Completed {_timesCompleted} time(s)");
        Console.ResetColor();
    }

    public override string GetStringRepresentation()
        => $"EternalGoal|{Name}|{Description}|{Points}|{_timesCompleted}";
}