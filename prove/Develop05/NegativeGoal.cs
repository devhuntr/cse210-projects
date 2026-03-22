// NEGATIVE GOAL (EXCEEDS for better grade:))
// For breaking bad habits. Recording this one COSTS you points.
// Example: "Ate junk food" = -100 pts
class NegativeGoal : Goal
{
    private int _timesTriggered;

    public NegativeGoal(string name, string description, int penalty)
        : base(name, description, penalty) // penalty stored as positive; we negate on record
    {
        _timesTriggered = 0;
    }

    public NegativeGoal(string name, string description, int penalty, int timesTriggered)
        : base(name, description, penalty)
    {
        _timesTriggered = timesTriggered;
    }

    public override int RecordEvent()
    {
        _timesTriggered++;
        int lost = Points; // Points stores the positive penalty value
        Console.WriteLine($"  Oof! You lost {lost} points. Try to do better! (Triggered {_timesTriggered}×)");
        return -lost; // negative so the caller subtracts correctly by just adding this
    }

    public override void PrintDetails()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  [!] {Name} ({Description}) — Triggered {_timesTriggered} time(s) [−{Points} pts each]");
        Console.ResetColor();
    }

    public override string GetStringRepresentation()
        => $"NegativeGoal|{Name}|{Description}|{Points}|{_timesTriggered}";
}