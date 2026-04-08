using System;
using System.IO;

namespace CloudSimulator
{
    public class ArchitectureSerializer
    {
        public static void Save(Architecture architecture, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            writer.WriteLine($"ARCHITECTURE|{architecture.Name}");
            foreach (var resource in architecture.Resources)
                writer.WriteLine(resource.Serialize());
        }

        public static Architecture Load(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Architecture file not found: {filePath}");

            Architecture architecture = null;

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split('|');
                string   type  = parts[0];

                if (type == "ARCHITECTURE")
                {
                    architecture = new Architecture(parts[1]);
                    continue;
                }

                if (architecture == null)
                    throw new InvalidOperationException("File must begin with an ARCHITECTURE line.");

                CloudResource resource = type switch
                {
                    "VM"     => new VirtualMachine(parts[1], parts[2], parts[3],
                                                   int.Parse(parts[4]), parts[5]),
                    "DB"     => new DatabaseInstance(parts[1], parts[2], parts[3],
                                                     int.Parse(parts[4]), bool.Parse(parts[5])),
                    "BUCKET" => new StorageBucket(parts[1], parts[2],
                                                  int.Parse(parts[3]), parts[4]),
                    "LB"     => new LoadBalancer(parts[1], parts[2],
                                                 int.Parse(parts[3]), int.Parse(parts[4]), parts[5]),
                    _        => throw new InvalidOperationException($"Unknown resource type: {type}")
                };

                architecture.AddResource(resource);
            }

            return architecture;
        }
    }
}
