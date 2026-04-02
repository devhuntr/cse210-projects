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
            decimal ratePerGb = AccessFrequency == "Frequent" ? 0.023m : 0.01m;
            return CapacityGb * ratePerGb;
        }

        public override string GetSummary()
        {
            return base.GetSummary() + $" | {CapacityGb}GB | {AccessFrequency} access";
        }

        public override bool Validate()
        {
            return base.Validate() && CapacityGb > 0 &&
                   (AccessFrequency == "Frequent" || AccessFrequency == "Infrequent");
        }
    }
}
