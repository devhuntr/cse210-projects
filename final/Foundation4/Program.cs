using System;
using System.Collections.Generic;


namespace CloudSimulator
{
    class Program
    {
        private static Architecture _architecture = new Architecture("My Cloud Stack");
        private static Budget       _budget       = new Budget(500m);
        private const  string       SaveFile      = "architecture.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Cloud Infrastructure Cost Simulator — Foundation 4");
            Console.WriteLine("===================================================");

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": AddResource();       break;
                    case "2": ListResources();     break;
                    case "3": ViewCostReport();    break;
                    case "4": ViewBudgetStatus();  break;
                    case "5": ViewRecommendations(); break;
                    case "6": SaveArchitecture();  break;
                    case "7": LoadArchitecture();  break;
                    case "8": SetBudget();         break;
                    case "9": RemoveResource();    break;
                    case "0": running = false;     break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            Console.WriteLine("Goodbye!");
        }

        static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine($"Architecture: {_architecture.Name} | Resources: {_architecture.ResourceCount} | Cost: ${_architecture.TotalMonthlyCost():F2}/mo");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("1) Add resource");
            Console.WriteLine("2) List resources");
            Console.WriteLine("3) View cost report");
            Console.WriteLine("4) View budget status");
            Console.WriteLine("5) View recommendations");
            Console.WriteLine("6) Save architecture to file");
            Console.WriteLine("7) Load architecture from file");
            Console.WriteLine("8) Set budget limit");
            Console.WriteLine("9) Remove resource");
            Console.WriteLine("0) Exit");
            Console.Write("Choice: ");
        }

        static void AddResource()
        {
            Console.WriteLine("Resource type: 1=VM  2=Database  3=Storage Bucket  4=Load Balancer");
            Console.Write("Choice: ");
            string type = Console.ReadLine()?.Trim();

            Console.Write("Name: ");
            string name = Console.ReadLine()?.Trim();

            Console.Write("Region (e.g. us-east-1): ");
            string region = Console.ReadLine()?.Trim();

            CloudResource resource = null;

            switch (type)
            {
                case "1":
                    Console.Write("CPU Tier (Small/Medium/Large): ");
                    string tier = Console.ReadLine()?.Trim();
                    Console.Write("RAM GB: ");
                    int ram = int.Parse(Console.ReadLine()?.Trim() ?? "4");
                    Console.Write("OS (Linux/Windows): ");
                    string os = Console.ReadLine()?.Trim();
                    resource = new VirtualMachine(name, region, tier, ram, os);
                    break;

                case "2":
                    Console.Write("DB Type (SQL/NoSQL): ");
                    string dbType = Console.ReadLine()?.Trim();
                    Console.Write("Storage GB: ");
                    int storage = int.Parse(Console.ReadLine()?.Trim() ?? "20");
                    Console.Write("Multi-AZ? (true/false): ");
                    bool multiAz = bool.Parse(Console.ReadLine()?.Trim() ?? "false");
                    resource = new DatabaseInstance(name, region, dbType, storage, multiAz);
                    break;

                case "3":
                    Console.Write("Capacity GB: ");
                    int cap = int.Parse(Console.ReadLine()?.Trim() ?? "100");
                    Console.Write("Access Frequency (Frequent/Infrequent/Archive): ");
                    string freq = Console.ReadLine()?.Trim();
                    resource = new StorageBucket(name, region, cap, freq);
                    break;

                case "4":
                    Console.Write("Max Connections: ");
                    int conns = int.Parse(Console.ReadLine()?.Trim() ?? "1000");
                    Console.Write("Rule Count: ");
                    int rules = int.Parse(Console.ReadLine()?.Trim() ?? "1");
                    Console.Write("Protocol (HTTP/HTTPS): ");
                    string proto = Console.ReadLine()?.Trim();
                    resource = new LoadBalancer(name, region, conns, rules, proto);
                    break;

                default:
                    Console.WriteLine("Unknown type — nothing added.");
                    return;
            }

            if (resource != null && resource.Validate())
            {
                _architecture.AddResource(resource);
                Console.WriteLine($"Added: {resource.GetSummary()}");
            }
            else
            {
                Console.WriteLine("Resource failed validation — not added.");
            }
        }

        static void ListResources()
        {
            if (_architecture.ResourceCount == 0)
            {
                Console.WriteLine("No resources yet.");
                return;
            }
            Console.WriteLine($"Resources in '{_architecture.Name}':");
            foreach (var r in _architecture.Resources)
                Console.WriteLine($"  {r.GetSummary()}");
        }

        static void ViewCostReport()
        {
            new CostReport(_architecture).PrintToConsole();
        }

        static void ViewBudgetStatus()
        {
            _budget.PrintStatus(_architecture.TotalMonthlyCost());
        }

        static void ViewRecommendations()
        {
            var engine = new RecommendationEngine(_architecture);
            List<Recommendation> recs = engine.Analyze();

            if (recs.Count == 0)
            {
                Console.WriteLine("No recommendations — architecture looks efficient!");
                return;
            }

            Console.WriteLine($"Recommendations ({recs.Count}):");
            foreach (var rec in recs)
                Console.WriteLine($"  {rec}");
            Console.WriteLine($"Total potential savings: ${engine.TotalPotentialSavings():F2}/mo");
        }

        static void SaveArchitecture()
        {
            ArchitectureSerializer.Save(_architecture, SaveFile);
            Console.WriteLine($"Saved to {SaveFile}");
        }

        static void LoadArchitecture()
        {
            try
            {
                _architecture = ArchitectureSerializer.Load(SaveFile);
                Console.WriteLine($"Loaded '{_architecture.Name}' with {_architecture.ResourceCount} resource(s).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading: {ex.Message}");
            }
        }

        static void RemoveResource()
        {
            if (_architecture.ResourceCount == 0)
            {
                Console.WriteLine("No resources to remove.");
                return;
            }

            var resources = _architecture.Resources;
            Console.WriteLine("Select resource to remove:");
            for (int i = 0; i < resources.Count; i++)
                Console.WriteLine($"  {i + 1}) {resources[i].GetSummary()}");
            Console.Write("Choice (0 to cancel): ");

            if (!int.TryParse(Console.ReadLine()?.Trim(), out int choice) || choice == 0)
            {
                Console.WriteLine("Cancelled.");
                return;
            }

            if (choice < 1 || choice > resources.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            var target = resources[choice - 1];
            _architecture.RemoveResource(target.ResourceId);
            Console.WriteLine($"Removed: {target.Name}");
        }

        static void SetBudget()
        {
            Console.Write("Enter new monthly budget limit ($): ");
            if (decimal.TryParse(Console.ReadLine()?.Trim(), out decimal limit) && limit > 0)
            {
                _budget = new Budget(limit);
                Console.WriteLine($"Budget set to ${limit:F2}/mo");
            }
            else
            {
                Console.WriteLine("Invalid amount — budget unchanged.");
            }
        }
    }
}
