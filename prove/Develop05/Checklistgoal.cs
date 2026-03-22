// CHECKLIST GOAL 
// Has to be done X times total. Each time earns points, and
// completing it earns a bonus. Example: temple 10× = 50pts each
// + 500pt bonus on the last one.
class ChecklistGoal : Goal
{
    private int _timesCompleted;
    private int _targetCount;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points,
                         int targetCount, int bonus)
        : base(name, description, points)
    {
        _timesCompleted = 0;
        _targetCount    = targetCount;
        _bonus          = bonus;
    }

    // loading constructor
    public ChecklistGoal(string name, string description, int points,
                         int targetCount, int bonus, int timesCompleted)
        : base(name, description, points)
    {
        _targetCount    = targetCount;
        _bonus          = bonus;
        _timesCompleted = timesCompleted;
    }

    public bool IsComplete() => _timesCompleted >= _targetCount;

    public override int RecordEvent()
    {
        if (IsComplete())
        {
            Console.WriteLine("  (This checklist goal is already complete — no points awarded.)");
            return 0;
        }

        _timesCompleted++;
        int earned = Points;

        if (IsComplete())
        {
            earned += _bonus;
            Console.WriteLine($"   GOAL COMPLETE! You earned {Points} + {_bonus} bonus = {earned} points!");
        }
        else
        {
            Console.WriteLine($"  Progress! You earned {Points} points. ({_timesCompleted}/{_targetCount} done)");
        }

        return earned;
    }

    public override void PrintDetails()
    {
        string checkbox = IsComplete() ? "[X]" : "[ ]";
        ConsoleColor color = IsComplete() ? ConsoleColor.Green : ConsoleColor.Yellow;
        Console.ForegroundColor = color;
        Console.WriteLine($"  {checkbox} {Name} ({Description}) — Completed {_timesCompleted}/{_targetCount} times");
        Console.ResetColor();
    }

    public override string GetStringRepresentation()
        => $"ChecklistGoal|{Name}|{Description}|{Points}|{_targetCount}|{_bonus}|{_timesCompleted}";
}