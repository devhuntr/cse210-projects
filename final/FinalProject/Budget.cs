using System;

namespace CloudSimulator
{
    // Tracks a monthly spending limit and raises alerts when the cost is close to or over budget.
    public class Budget
    {
        public decimal MonthlyLimit { get; private set; }

        public Budget(decimal monthlyLimit)
        {
            if (monthlyLimit <= 0)
                throw new ArgumentException("Budget limit must be greater than zero.");
            MonthlyLimit = monthlyLimit;
        }

        public decimal Remaining(decimal actualCost)    => MonthlyLimit - actualCost;
        public decimal UsagePercent(decimal actualCost) => (actualCost / MonthlyLimit) * 100m;

        public void PrintStatus(decimal actualCost)
        {
            decimal remaining = Remaining(actualCost);
            decimal percent   = UsagePercent(actualCost);

            Console.WriteLine($"  Budget Limit : ${MonthlyLimit:F2}/mo");
            Console.WriteLine($"  Current Cost : ${actualCost:F2}/mo  ({percent:F1}% used)");

            if (remaining < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  *** OVER BUDGET: ${Math.Abs(remaining):F2} over your limit! ***");
            }
            else if (percent >= 80)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  WARNING: Approaching limit - only ${remaining:F2} remaining.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  On Track: ${remaining:F2} remaining.");
            }

            Console.ResetColor();
        }
    }
}
