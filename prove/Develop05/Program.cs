// Eternal Quest Program - Unit 05 Polymorphism Prove Assignment
//
// Exceeds core requirements by adding a NegativeGoal class.
// A NegativeGoal represents a bad habit the user wants to break
// (e.g., "ate junk food", "skipped the gym"). Every time the
// user records that event, they LOSE points instead of gaining
// them. This encourages accountability and gives the program a
// second dimension — it's not just about gaining points, it's
// also about not losing them. The NegativeGoal inherits from
// Goal and overrides RecordEvent() to return a negative value,
// which the caller adds to the score just like any other goal.


using System;
using System.Collections.Generic;
using System.IO;

// PROGRAM Entry Point
// Main driver class. Handles the menu loop, score, and file I/O.
class Program
{
    private static int        _score = 0;
    private static List<Goal> _goals = new List<Goal>();

    static void Main(string[] args)
    {
        // I put it in here fancy-like to make it feel more like a game opening screen, 
        //but you could just as easily do this with a Console.WriteLine at the top of the menu loop if you wanted
        Console.WriteLine("╔══════════════════════════════════╗");
        Console.WriteLine("║       Welcome to Eternal Quest   ║");
        Console.WriteLine("╚══════════════════════════════════╝");

        bool running = true;
        while (running)
        {
            DisplayMenu();
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1": DisplayScore();          break;
                case "2": ListGoals();             break;
                case "3": CreateGoal();            break;
                case "4": RecordEvent();           break;
                case "5": SaveGoals("goals.txt");  break;
                case "6": LoadGoals("goals.txt");  break;
                case "7": running = false;         break;
                default:
                    Console.WriteLine("  Hmm, that's not a valid option. Try again.");
                    break;
            }
        }

        Console.WriteLine("     Keep up the quest! See you next time.");
    }

    // just print the menu options
    static void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("─── Main Menu ───────────────────────");
        Console.WriteLine("  1. Show Score");
        Console.WriteLine("  2. List Goals");
        Console.WriteLine("  3. Create New Goal");
        Console.WriteLine("  4. Record an Event");
        Console.WriteLine("  5. Save Goals");
        Console.WriteLine("  6. Load Goals");
        Console.WriteLine("  7. Quit");
        Console.WriteLine("─────────────────────────────────────");
        Console.Write("  Your choice: ");
    }

    // show the user their current total score
    static void DisplayScore()
    {
        Console.WriteLine($"\n  ★ Current Score: {_score} points");
    }

    // list all goals with their current status
    static void ListGoals()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("\n  No goals yet — create some!");
            return;
        }

        Console.WriteLine("\n  Your Goals:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.Write($"  {i + 1}. ");
            // polymorphism here because each subclass handles its own display
            _goals[i].PrintDetails();
        }
    }

    // walk the user through creating any type of goal
    static void CreateGoal()
    {
        Console.WriteLine("\n  What kind of goal?");
        Console.WriteLine("    1. Simple Goal     (one-time completion)");
        Console.WriteLine("    2. Eternal Goal    (repeating forever)");
        Console.WriteLine("    3. Checklist Goal  (done X times total)");
        Console.WriteLine("    4. Negative Goal   (bad habit — loses points!)");
        Console.Write("  Type: ");

        string type = Console.ReadLine()?.Trim() ?? "";

        Console.Write("  Goal name: ");
        string name = Console.ReadLine() ?? "Unnamed";

        Console.Write("  Short description: ");
        string desc = Console.ReadLine() ?? "";

        Console.Write("  Points (reward per event): ");
        int.TryParse(Console.ReadLine(), out int pts);

        switch (type)
        {
            case "1":
                _goals.Add(new SimpleGoal(name, desc, pts));
                break;

            case "2":
                _goals.Add(new EternalGoal(name, desc, pts));
                break;

            case "3":
                Console.Write("  How many times must it be completed? ");
                int.TryParse(Console.ReadLine(), out int target);
                Console.Write("  Completion bonus points: ");
                int.TryParse(Console.ReadLine(), out int bonus);
                _goals.Add(new ChecklistGoal(name, desc, pts, target, bonus));
                break;

            case "4":
                // for negative goals, pts is the penalty amount
                _goals.Add(new NegativeGoal(name, desc, pts));
                break;

            default:
                Console.WriteLine("  Didn't recognize that type. No goal added.");
                return;
        }

        Console.WriteLine($"  Goal \"{name}\" created!");
    }

    // let the user pick a goal they've worked on and log it
    static void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("\n  No goals to record yet!");
            return;
        }

        ListGoals();
        Console.Write("\n  Which goal did you work on? (number): ");
        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > _goals.Count)
        {
            Console.WriteLine("  That's not a valid goal number.");
            return;
        }

        Goal chosen = _goals[idx - 1];

        //RecordEvent() does the right thing for each goal type
        int earned = chosen.RecordEvent();
        _score += earned;

        DisplayScore();
    }

    // write everything to a plain text file, one goal per line
    static void SaveGoals(string filename)
    {
        using StreamWriter writer = new StreamWriter(filename);
        writer.WriteLine(_score);
        foreach (Goal g in _goals)
            //Each class builds its own save string
            writer.WriteLine(g.GetStringRepresentation());

        Console.WriteLine($"    Goals saved to {filename}.");
    }

    // read the file back and reconstruct the goal list
    static void LoadGoals(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("  No save file found. Start fresh!");
            return;
        }

        _goals.Clear();
        string[] lines = File.ReadAllLines(filename);

        // first line is always the score
        int.TryParse(lines[0], out _score);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] parts = lines[i].Split('|');
            if (parts.Length < 1) continue;

            // rebuild the right object based on the type tag at the start
            switch (parts[0])
            {
                case "SimpleGoal":
                    // SimpleGoal|name|desc|pts|isCompleted
                    bool.TryParse(parts[4], out bool done);
                    _goals.Add(new SimpleGoal(parts[1], parts[2],
                        int.Parse(parts[3]), done));
                    break;

                case "EternalGoal":
                    // EternalGoal|name|desc|pts|timesCompleted
                    int.TryParse(parts[4], out int tc);
                    _goals.Add(new EternalGoal(parts[1], parts[2],
                        int.Parse(parts[3]), tc));
                    break;

                case "ChecklistGoal":
                    // ChecklistGoal|name|desc|pts|targetCount|bonus|timesCompleted
                    _goals.Add(new ChecklistGoal(parts[1], parts[2],
                        int.Parse(parts[3]),
                        int.Parse(parts[4]),
                        int.Parse(parts[5]),
                        int.Parse(parts[6])));
                    break;

                case "NegativeGoal":
                    // NegativeGoal|name|desc|pts|timesTriggered
                    int.TryParse(parts[4], out int triggers);
                    _goals.Add(new NegativeGoal(parts[1], parts[2],
                        int.Parse(parts[3]), triggers));
                    break;
            }
        }

        Console.WriteLine($"    Goals loaded from {filename}. Welcome back!");
        DisplayScore();
    }
}