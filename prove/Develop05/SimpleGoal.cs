// SIMPLE GOAL
// A one-and-done goal. Mark it complete, get the points, done.
// Example: "Run a marathon" for 1000 points.
class SimpleGoal : Goal
{
    private bool _isCompleted;

    public SimpleGoal(string name, string description, int points)
        : base(name, description, points)
    {
        _isCompleted = false;
    }

    // full constructor used when loading from file
    public SimpleGoal(string name, string description, int points, bool isCompleted)
        : base(name, description, points)
    {
        _isCompleted = isCompleted;
    }

    public bool IsCompleted => _isCompleted;

    // override: only award points if it hasn't been done yet
    public override int RecordEvent()
    {
        if (_isCompleted)
        {
            Console.WriteLine("  (This goal is already complete — no points awarded.)");
            return 0;
        }
        _isCompleted = true;
        Console.WriteLine($"  Goal completed! You earned {Points} points!");
        return Points;
    }

    public override void PrintDetails()
    {
        string checkbox = _isCompleted ? "[X]" : "[ ]";
        ConsoleColor color = _isCompleted ? ConsoleColor.Green : ConsoleColor.White;
        Console.ForegroundColor = color;
        Console.WriteLine($"  {checkbox} {Name} ({Description})");
        Console.ResetColor();
    }

    public override string GetStringRepresentation()
        => $"SimpleGoal|{Name}|{Description}|{Points}|{_isCompleted}";
}