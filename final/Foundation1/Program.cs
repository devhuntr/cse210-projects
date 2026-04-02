using System;
//This is the class that is going to be running the whole project.
namespace CloudSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cloud Infrastructure Cost Simulator");
            Console.WriteLine("===================================\n");

            var vm = new VirtualMachine("Web Server", "us-east-1", "Medium", 8, "Linux");
            var db = new DatabaseInstance("Primary DB", "us-east-1", "SQL", 100, false);
            var bucket = new StorageBucket("Media Bucket", "us-east-1", 500, "Frequent");
            var lb = new LoadBalancer("Main LB", "us-east-1", 5000, 4, "HTTPS");
            var arch = new Architecture("My Cloud Stack");

            Console.WriteLine($"VM:      {vm.Name} | {vm.Region} | {vm.CpuTier} | {vm.RamGb}GB | {vm.OperatingSystem}");
            Console.WriteLine($"DB:      {db.Name} | {db.Region} | {db.DbType} | {db.StorageGb}GB | MultiAZ={db.MultiAz}");
            Console.WriteLine($"Bucket:  {bucket.Name} | {bucket.Region} | {bucket.CapacityGb}GB | {bucket.AccessFrequency}");
            Console.WriteLine($"LB:      {lb.Name} | {lb.Region} | {lb.MaxConnections} conns | {lb.Protocol}");
            Console.WriteLine($"Arch:    {arch.Name} | {arch.ResourceCount} resources");
        }
    }
}
