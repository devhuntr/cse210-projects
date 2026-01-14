using System;

class Program
{
    static void Main(string[] args)
    {
        // ASk the user for their grade percentage & convert it to a integer
        Console.WriteLine("What is your grade percentage? (Just the digits) ");
        string gradePercentage = Console.ReadLine();
        int percentage = int.Parse(gradePercentage);

        string letter = "";

        if (percentage >= 90)
        {
            letter = "A";
        }
        else if (percentage >= 80)
        {
            letter = "B";
        }
        else if (percentage >= 70)
        {
            letter = "C";
        }
        else if (percentage >= 60)
        {
            letter = "D";
        }
        else
        {
            letter = "F";
        }

        Console.WriteLine($"Your letter grade is {letter}.");

        if (percentage >= 70)
        {
            Console.WriteLine("Congrats! You passed the course!");
        }
        else
        {
            Console.WriteLine("Sorry mate, you did not pass. Better luck next time!");
        }
    }
}