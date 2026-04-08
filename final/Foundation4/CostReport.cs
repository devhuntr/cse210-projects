using System;
using System.IO;
using System.Text;

namespace CloudSimulator
{
    public class CostReport
    {
        public Architecture Architecture { get; private set; }
        public DateTime     GeneratedAt  { get; private set; }

        public CostReport(Architecture architecture)
        {
            Architecture = architecture;
            GeneratedAt  = DateTime.Now;
        }

        public string GenerateReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Cost Report: {Architecture.Name}");
            sb.AppendLine($"Generated:   {GeneratedAt:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine(new string('-', 60));
            foreach (var r in Architecture.Resources)
                sb.AppendLine(r.GetSummary());
            sb.AppendLine(new string('-', 60));
            sb.AppendLine($"Total Monthly Cost: ${Architecture.TotalMonthlyCost():F2}");
            return sb.ToString();
        }

        public void PrintToConsole() => Console.WriteLine(GenerateReport());

        public void ExportToFile(string filePath) => File.WriteAllText(filePath, GenerateReport());
    }
}
