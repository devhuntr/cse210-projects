using System;

// Tracks a monthly spending limit for an Architecture.
// Raises alerts when the actual cost is close to or over budget.
namespace CloudSimulator
{
    public class Budget
    {
        public decimal MonthlyLimit { get; private set; }

        public Budget(decimal monthlyLimit)
        {
            if (monthlyLimit <= 0)
                throw new ArgumentException("Budget limit must be greater than zero.");
            MonthlyLimit = monthlyLimit;
        }

        // Returns how much of the budget is still available (negative = over budget).
        public decimal Remaining(decimal actualCost) => MonthlyLimit - actualCost;

        // Returns the cost as a percentage of the budget limit.
        public decimal UsagePercent(decimal actualCost) => (actualCost / MonthlyLimit) * 100m;

        // Prints a coloured status line to the console.
        public void PrintStatus(decimal actualCost)
        {
            decimal remaining = Remaining(actualCost);
            decimal percent   = UsagePercent(actualCost);

            Console.WriteLine($"Budget Limit : ${MonthlyLimit:F2}/mo");
            Console.WriteLine($"Current Cost : ${actualCost:F2}/mo  ({percent:F1}% used)");

            if (remaining < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"OVER BUDGET  : ${Math.Abs(remaining):F2} over limit!");
            }
            else if (percent >= 80)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"WARNING      : Approaching budget limit — ${remaining:F2} remaining.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"On Track     : ${remaining:F2} remaining.");
            }

            Console.ResetColor();
        }
    }
}
