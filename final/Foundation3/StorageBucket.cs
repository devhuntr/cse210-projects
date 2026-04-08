namespace CloudSimulator
{
    public class StorageBucket : CloudResource
    {
        public int    CapacityGb      { get; private set; }
        public string AccessFrequency { get; private set; }

        public StorageBucket(string name, string region,
                             int capacityGb, string accessFrequency)
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

        public override string GetSummary()
        {
            return base.GetSummary() + $" | {CapacityGb}GB | {AccessFrequency} access";
        }

        public override bool Validate()
        {
            return base.Validate() && CapacityGb > 0 &&
                   (AccessFrequency == "Frequent" || AccessFrequency == "Infrequent" || AccessFrequency == "Archive");
        }

        // Format: BUCKET|name|region|capacityGb|accessFrequency
        public override string Serialize()
        {
            return $"BUCKET|{Name}|{Region}|{CapacityGb}|{AccessFrequency}";
        }
    }
}
