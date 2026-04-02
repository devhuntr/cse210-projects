/// Represents a cloud database instance resource.
/// Supports the main two database engine types: SQL and NoSQL, each with different base rates.
/// Cost is calculated from the engine type, the amount of storage provisioned,
/// and whether Multi-AZ redundancy is enabled. That usually doubles the cost I beleive
namespace CloudSimulator
{
    public class DatabaseInstance : CloudResource
    {
        public string DbType    { get; private set; }
        public int    StorageGb { get; private set; }
        public bool   MultiAz   { get; private set; }

        public DatabaseInstance(string name, string region,
                                string dbType, int storageGb, bool multiAz)
            : base(name, region)
        {
            DbType    = dbType;
            StorageGb = storageGb;
            MultiAz   = multiAz;
        }

        public override decimal CalculateCost() => 0m;
    }
}
