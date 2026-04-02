/// Represents a cloud load balancer resource.
/// So, the load balancer distributes incoming traffic across multiple servers
/// to improve availability and performance. I haven't used this a lot, but it is expensive.
/// Cost is based on the maximum connection tier, the number of routing rules,
/// and the protocol used.
namespace CloudSimulator
{
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

        public override decimal CalculateCost() => 0m;
    }
}
