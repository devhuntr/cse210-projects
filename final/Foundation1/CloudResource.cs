//This is the abstract class for my cloud cost estimator project.
// It is the base class for the cloud resources we are actually going to be measuring the cost of.


namespace CloudSimulator
{
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
        public virtual string GetSummary() => "";
        public virtual bool Validate() => true;
    }
}
