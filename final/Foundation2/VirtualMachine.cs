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

        public override decimal CalculateCost()
        {
            decimal tierRate = CpuTier switch { "Small" => 20m, "Medium" => 50m, "Large" => 100m, _ => 50m };
            decimal ramCost  = RamGb * 0.5m;
            decimal osCost   = OperatingSystem == "Windows" ? 15m : 0m;
            return tierRate + ramCost + osCost;
        }

        public override string GetSummary()
        {
            return base.GetSummary() + $" | {CpuTier} | {RamGb}GB RAM | {OperatingSystem}";
        }

        public override bool Validate()
        {
            return base.Validate() && RamGb > 0 &&
                   (CpuTier == "Small" || CpuTier == "Medium" || CpuTier == "Large");
        }
    }
}
