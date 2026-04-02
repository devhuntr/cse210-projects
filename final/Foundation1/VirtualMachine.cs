/// Represents a cloud virtual machine resource.
/// Pricing is based on the CPU tier (Small, Medium, Large), the amount of RAM, 
/// and whether the OS is Linux or Windows Windows adds an extra charge becasue it isn't open source.
namespace CloudSimulator
{
    public class VirtualMachine : CloudResource
    {
        public string CpuTier         { get; private set; }
        public int    RamGb           { get; private set; }
        public string OperatingSystem { get; private set; }

        public VirtualMachine(string name, string region,
                              string cpuTier, int ramGb, string operatingSystem)
            : base(name, region)
        {
            CpuTier         = cpuTier;
            RamGb           = ramGb;
            OperatingSystem = operatingSystem;
        }

        public override decimal CalculateCost() => 0m;
    }
}
