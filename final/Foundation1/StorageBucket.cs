// Represents a cloud object storage bucket resource.
/// Used to store files, media, backups, and other data in the cloud.
/// Pricing is calculated per gigabyte and varies by access frequency tier (Frequent, Infrequent, Archive).
/// Frequent access costs the most, Archive costs the least because usually they are not accessed often and they 
/// can easily be reserved).
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

        public override decimal CalculateCost() => 0m;
    }
}
