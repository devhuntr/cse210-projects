namespace CloudSimulator
{
    // Abstract base class for all cloud resources.
    // Every resource has a name, region, unique ID, and can calculate its monthly cost.
    public abstract class CloudResource
    {
        public string Name       { get; private set; }
        public string Region     { get; private set; }
        public string ResourceId { get; private set; }
        protected decimal BaseMonthlyRate { get; set; }

        protected CloudResource(string name, string region)
        {
            Name       = name;
            Region     = region;
            ResourceId = System.Guid.NewGuid().ToString();
        }

        public abstract decimal CalculateCost();

        public virtual string GetSummary()
        {
            return $"[{GetType().Name,-18}] {Name,-20} ({Region}) | ${CalculateCost():F2}/mo";
        }

        public virtual bool Validate()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Region);
        }

        public abstract string Serialize();
    }
}
