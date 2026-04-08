namespace CloudSimulator
{
    // A cloud load balancer that distributes traffic across multiple servers.
    // Cost is driven by protocol, connection volume, and number of routing rules.
    public class LoadBalancer : CloudResource
    {
        public int    MaxConnections { get; private set; }
        public int    RuleCount      { get; private set; }
        public string Protocol       { get; private set; }

        public LoadBalancer(string name, string region,
                            int maxConnections, int ruleCount, string protocol)
            : base(name, region)
        {
            MaxConnections = maxConnections;
            RuleCount      = ruleCount;
            Protocol       = protocol;
        }

        public override decimal CalculateCost()
        {
            decimal baseCost = Protocol == "HTTPS" ? 25m : 18m;
            decimal connCost = (MaxConnections / 1000m) * 0.5m;
            decimal ruleCost = RuleCount * 0.25m;
            return baseCost + connCost + ruleCost;
        }

        public override string GetSummary() =>
            base.GetSummary() + $" | {Protocol} | {MaxConnections} conns | {RuleCount} rules";

        public override bool Validate() =>
            base.Validate() && MaxConnections > 0 && RuleCount >= 0 &&
            (Protocol == "HTTP" || Protocol == "HTTPS");

        public override string Serialize() =>
            $"LB|{Name}|{Region}|{MaxConnections}|{RuleCount}|{Protocol}";
    }
}
