namespace CloudSimulator
{
    // Cloud object storage.
    // Three access tiers: Frequent (hot), Infrequent (cool), Archive (cold).
    // Lower access frequency = cheaper per-GB rate.
    public class StorageBucket : CloudResource
    {
        public int    CapacityGb      { get; private set; }
        public string AccessFrequency { get; private set; }

        public StorageBucket(string name, string region, int capacityGb, string accessFrequency)
            : base(name, region)
        {
            CapacityGb      = capacityGb;
            AccessFrequency = accessFrequency;
        }

        public override decimal CalculateCost()
        {
            decimal ratePerGb = AccessFrequency switch
            {
                "Frequent"   => 0.023m,
                "Infrequent" => 0.010m,
                "Archive"    => 0.004m,
                _            => 0.023m
            };
            return CapacityGb * ratePerGb;
        }

        public override string GetSummary() =>
            base.GetSummary() + $" | {CapacityGb}GB | {AccessFrequency} access";

        public override bool Validate() =>
            base.Validate() && CapacityGb > 0 &&
            (AccessFrequency == "Frequent" || AccessFrequency == "Infrequent" || AccessFrequency == "Archive");

        public override string Serialize() =>
            $"BUCKET|{Name}|{Region}|{CapacityGb}|{AccessFrequency}";
    }
}
