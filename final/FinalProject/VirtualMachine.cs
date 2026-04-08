namespace CloudSimulator
{
    // A cloud virtual machine.
    // Cost = tier base rate + RAM cost + optional Windows license surcharge.
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

        public override decimal CalculateCost()
        {
            // bigger tier = more $$$: Small=$20, Medium=$50, Large=$100 (defaults to Medium if something weird gets passed in)
            decimal tierRate = CpuTier switch { "Small" => 20m, "Medium" => 50m, "Large" => 100m, _ => 50m };
            // 50 cents per GB of RAM, pretty straightforward
            decimal ramCost  = RamGb * 0.5m;
            // Windows costs extra, Linux is free
            decimal osCost   = OperatingSystem == "Windows" ? 15m : 0m;
            return tierRate + ramCost + osCost;
        }

        // tack on the VM-specific stuff after the base info (name + region)
        public override string GetSummary() =>
            base.GetSummary() + $" | {CpuTier} | {RamGb}GB RAM | {OperatingSystem}";

        // make sure nothing dumb got passed in,  RAM has to be positive, tier and OS have to be ones we actually support
        public override bool Validate() =>
            base.Validate() && RamGb > 0 &&
            (CpuTier == "Small" || CpuTier == "Medium" || CpuTier == "Large") &&
            (OperatingSystem == "Linux" || OperatingSystem == "Windows");

        // turns the VM into a string we can save to a file, pipe-separated so we can parse it back later
        public override string Serialize() =>
            $"VM|{Name}|{Region}|{CpuTier}|{RamGb}|{OperatingSystem}";
    }
}
