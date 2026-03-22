// BASE CLASS 
// Every goal type inherits from here. Holds the shared stuff
// like name, description, and how many points it's worth.
abstract class Goal
{
    // keeping these private so subclasses have to use the
    // constructor or properties
    private string _name;
    private string _description;
    private int _points;

    public Goal(string name, string description, int points)
    {
        _name        = name;
        _description = description;
        _points      = points;
    }

    // read-only properties so subclasses can see but not freely stomp on these
    public string Name        => _name;
    public string Description => _description;
    public int    Points      => _points;

    // virtual so each subclass can override with its own logic
    public virtual int    RecordEvent()              { return _points; }
    public virtual void   PrintDetails()             { Console.WriteLine($"  {_name}: {_description}"); }
    public virtual string GetStringRepresentation()  => $"{GetType().Name}|{_name}|{_description}|{_points}";
}