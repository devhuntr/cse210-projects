using System;
using System.Collections.Generic;

// Cloud Infrastructure Cost Simulator — Final Project
// Combines all foundations: class hierarchy, cost calculation, file persistence,
// budget tracking, and an interactive console menu.
namespace CloudSimulator
{
    class Program
    {
        private static Architecture _architecture = new Architecture("My Cloud Stack");
        private static Budget       _budget       = new Budget(500m);
        private const  string       SaveFile      = "architecture.txt";

        static void Main(string[] args)
        {
            PrintBanner();

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": AddResource();         break;
                    case "2": RemoveResource();      break;
                    case "3": ListResources();       break;
                    case "4": ViewCostReport();      break;
                    case "5": ViewBudgetStatus();    break;
                    case "6": ViewRecommendations(); break;
                    case "7": ExportReport();        break;
                    case "8": SaveArchitecture();    break;
                    case "9": LoadArchitecture();    break;
                    case "B": SetBudget();           break;
                    case "0": running = false;       break;
                    default:
                        Console.WriteLine("  Invalid choice — please try again.");
                        break;
                }
            }

            Console.WriteLine("Thank you for using the Cloud Cost Simulator. Goodbye!");
        }

        // ── UI Helpers ──────────────────────────────────────────────────────────

        static void PrintBanner()
        {
            Console.WriteLine();
            Console.WriteLine("  ╔══════════════════════════════════════════════╗");
            Console.WriteLine("  ║   Cloud Infrastructure Cost Simulator v1.0  ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        static void PrintMenu()
        {
            decimal cost = _architecture.TotalMonthlyCost();
            Console.WriteLine(new string('─', 55));
            Console.WriteLine($"  Architecture : {_architecture.Name}");
            Console.WriteLine($"  Resources    : {_architecture.ResourceCount}");
            Console.Write($"  Monthly Cost : ${cost:F2}  ");
            PrintBudgetBadge(cost);
            Console.WriteLine(new string('─', 55));
            Console.WriteLine("  1) Add resource          2) Remove resource");
            Console.WriteLine("  3) List resources        4) View cost report");
            Console.WriteLine("  5) Budget status         6) Recommendations");
            Console.WriteLine("  7) Export report to file");
            Console.WriteLine("  8) Save architecture     9) Load architecture");
            Console.WriteLine("  B) Set budget limit      0) Exit");
            Console.WriteLine(new string('─', 55));
            Console.Write("  Choice: ");
        }

        static void PrintBudgetBadge(decimal cost)
        {
            decimal pct = _budget.UsagePercent(cost);
            if (pct > 100)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[OVER BUDGET]");
            }
            else if (pct >= 80)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[{pct:F0}% of budget]");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[{pct:F0}% of budget]");
            }
            Console.ResetColor();
        }

        // ── Menu Actions ────────────────────────────────────────────────────────

        static void AddResource()
        {
            Console.WriteLine("  Resource type:");
            Console.WriteLine("    1) Virtual Machine");
            Console.WriteLine("    2) Database Instance");
            Console.WriteLine("    3) Storage Bucket");
            Console.WriteLine("    4) Load Balancer");
            Console.Write("  Choice: ");
            string type = Console.ReadLine()?.Trim();

            Console.Write("  Name: ");
            string name = Console.ReadLine()?.Trim();
            Console.Write("  Region (e.g. us-east-1): ");
            string region = Console.ReadLine()?.Trim();

            CloudResource resource = null;

            switch (type)
            {
                case "1":
                    Console.Write("  CPU Tier (Small/Medium/Large): ");
                    string tier = Console.ReadLine()?.Trim();
                    Console.Write("  RAM GB: ");
                    int ram = ReadInt(4);
                    Console.Write("  OS (Linux/Windows): ");
                    string os = Console.ReadLine()?.Trim();
                    resource = new VirtualMachine(name, region, tier, ram, os);
                    break;

                case "2":
                    Console.Write("  DB Type (SQL/NoSQL): ");
                    string dbType = Console.ReadLine()?.Trim();
                    Console.Write("  Storage GB: ");
                    int storage = ReadInt(20);
                    Console.Write("  Multi-AZ (true/false): ");
                    bool multiAz = ReadBool(false);
                    resource = new DatabaseInstance(name, region, dbType, storage, multiAz);
                    break;

                case "3":
                    Console.Write("  Capacity GB: ");
                    int cap = ReadInt(100);
                    Console.Write("  Access Frequency (Frequent/Infrequent/Archive): ");
                    string freq = Console.ReadLine()?.Trim();
                    resource = new StorageBucket(name, region, cap, freq);
                    break;

                case "4":
                    Console.Write("  Max Connections: ");
                    int conns = ReadInt(1000);
                    Console.Write("  Rule Count: ");
                    int rules = ReadInt(1);
                    Console.Write("  Protocol (HTTP/HTTPS): ");
                    string proto = Console.ReadLine()?.Trim();
                    resource = new LoadBalancer(name, region, conns, rules, proto);
                    break;

                default:
                    Console.WriteLine("  Unknown type — nothing added.");
                    return;
            }

            if (resource != null && resource.Validate())
            {
                _architecture.AddResource(resource);
                Console.WriteLine($"\n  Added: {resource.GetSummary()}");
            }
            else
            {
                Console.WriteLine("  Resource failed validation — check your inputs.");
            }
        }

        static void RemoveResource()
        {
            if (_architecture.ResourceCount == 0)
            {
                Console.WriteLine("  No resources to remove.");
                return;
            }

            Console.WriteLine("  Current resources:");
            int i = 1;
            foreach (var r in _architecture.Resources)
                Console.WriteLine($"    {i++}) {r.Name} ({r.GetType().Name}) — ID: {r.ResourceId}");

            Console.Write("  Enter Resource ID to remove: ");
            string id = Console.ReadLine()?.Trim();

            if (_architecture.RemoveResource(id))
                Console.WriteLine("  Resource removed.");
            else
                Console.WriteLine("  Resource not found.");
        }

        static void ListResources()
        {
            if (_architecture.ResourceCount == 0)
            {
                Console.WriteLine("  No resources in this architecture yet.");
                return;
            }
            Console.WriteLine($"  Resources in '{_architecture.Name}':");
            foreach (var r in _architecture.Resources)
                Console.WriteLine($"    {r.GetSummary()}");
        }

        static void ViewCostReport()
        {
            new CostReport(_architecture).PrintToConsole();
        }

        static void ViewBudgetStatus()
        {
            Console.WriteLine("  Budget Status:");
            _budget.PrintStatus(_architecture.TotalMonthlyCost());
        }

        static void ViewRecommendations()
        {
            var engine = new RecommendationEngine(_architecture);
            List<Recommendation> recs = engine.Analyze();

            if (recs.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  No recommendations — your architecture looks efficient!");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"  Cost-Saving Recommendations ({recs.Count} found):");
            foreach (var rec in recs)
                Console.WriteLine(rec);
            Console.WriteLine();
            Console.WriteLine($"  Total potential savings: ${engine.TotalPotentialSavings():F2}/mo");
        }

        static void ExportReport()
        {
            string path = "cost_report.txt";
            new CostReport(_architecture).ExportToFile(path);
            Console.WriteLine($"  Report exported to: {System.IO.Path.GetFullPath(path)}");
        }

        static void SaveArchitecture()
        {
            ArchitectureSerializer.Save(_architecture, SaveFile);
            Console.WriteLine($"  Architecture saved to: {System.IO.Path.GetFullPath(SaveFile)}");
        }

        static void LoadArchitecture()
        {
            try
            {
                _architecture = ArchitectureSerializer.Load(SaveFile);
                Console.WriteLine($"  Loaded '{_architecture.Name}' — {_architecture.ResourceCount} resource(s).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error: {ex.Message}");
            }
        }

        static void SetBudget()
        {
            Console.Write("  New monthly budget limit ($): ");
            if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal limit) && limit > 0)
            {
                _budget = new Budget(limit);
                Console.WriteLine($"  Budget updated to ${limit:F2}/mo");
            }
            else
            {
                Console.WriteLine("  Invalid amount — budget unchanged.");
            }
        }

        // ── Input Helpers ───────────────────────────────────────────────────────

        static int ReadInt(int defaultValue)
        {
            string input = Console.ReadLine()?.Trim();
            return int.TryParse(input, out int value) ? value : defaultValue;
        }

        static bool ReadBool(bool defaultValue)
        {
            string input = Console.ReadLine()?.Trim();
            return bool.TryParse(input, out bool value) ? value : defaultValue;
        }
    }
}
