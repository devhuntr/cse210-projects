using System;

namespace CloudSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cloud Infrastructure Cost Simulator");
            Console.WriteLine("====================================\n");

            var architecture = new Architecture("My Cloud Stack");

            architecture.AddResource(new VirtualMachine("Web Server", "us-east-1", "Medium", 8, "Linux"));
            architecture.AddResource(new DatabaseInstance("Primary DB", "us-east-1", "SQL", 100, false));
            architecture.AddResource(new StorageBucket("Media Bucket", "us-east-1", 500, "Frequent"));
            architecture.AddResource(new LoadBalancer("Main LB", "us-east-1", 5000, 4, "HTTPS"));

            var report = new CostReport(architecture);
            report.PrintToConsole();

            var engine = new RecommendationEngine(architecture);
            var recommendations = engine.Analyze();

            Console.WriteLine($"Recommendations found: {recommendations.Count}");
            Console.WriteLine($"Potential savings: ${engine.TotalPotentialSavings():F2}/mo");
            foreach (var rec in recommendations)
                Console.WriteLine($"  {rec}");
        }
    }
}
