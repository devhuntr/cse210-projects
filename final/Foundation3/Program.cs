using System;
using System.IO;

// Foundation 3: Adds file persistence save and reload an Architecture from disk.
namespace CloudSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cloud Infrastructure Cost Simulator — Foundation 3");
            Console.WriteLine("===================================================\n");

            // --- Build an architecture ---
            var architecture = new Architecture("My Cloud Stack");
            architecture.AddResource(new VirtualMachine("Web Server",  "us-east-1", "Medium", 8,   "Linux"));
            architecture.AddResource(new DatabaseInstance("Primary DB", "us-east-1", "SQL",    100, false));
            architecture.AddResource(new StorageBucket("Media Bucket", "us-east-1", 500,       "Frequent"));
            architecture.AddResource(new LoadBalancer("Main LB",       "us-east-1", 5000, 4,   "HTTPS"));

            // --- Print original report ---
            Console.WriteLine("=== Original Architecture ===");
            new CostReport(architecture).PrintToConsole();

            // --- Save to file ---
            string saveFile = "architecture.txt";
            ArchitectureSerializer.Save(architecture, saveFile);
            Console.WriteLine($"Architecture saved to: {Path.GetFullPath(saveFile)}\n");

            // --- Load from file ---
            Architecture loaded = ArchitectureSerializer.Load(saveFile);
            Console.WriteLine("=== Loaded from File ===");
            new CostReport(loaded).PrintToConsole();

            // --- Recommendations ---
            var engine = new RecommendationEngine(loaded);
            var recs   = engine.Analyze();
            Console.WriteLine($"Recommendations ({recs.Count}):");
            if (recs.Count == 0)
                Console.WriteLine("  None — architecture looks efficient!");
            else
                foreach (var rec in recs)
                    Console.WriteLine($"  {rec}");

            Console.WriteLine($"\nPotential savings: ${engine.TotalPotentialSavings():F2}/mo");
        }
    }
}
