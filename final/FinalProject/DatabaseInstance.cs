namespace CloudSimulator
{
    // A managed cloud database.
    // SQL has a higher base rate than NoSQL.
    // Multi-AZ doubles the base rate for high availability.
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

        public override decimal CalculateCost()
        {
            decimal typeRate    = DbType == "SQL" ? 80m : 60m;
            decimal storageCost = StorageGb * 0.1m;
            decimal multiAzCost = MultiAz ? typeRate : 0m;
            return typeRate + storageCost + multiAzCost;
        }

        public override string GetSummary() =>
            base.GetSummary() + $" | {DbType} | {StorageGb}GB | MultiAZ={MultiAz}";

        public override bool Validate() =>
            base.Validate() && StorageGb > 0 &&
            (DbType == "SQL" || DbType == "NoSQL");

        public override string Serialize() =>
            $"DB|{Name}|{Region}|{DbType}|{StorageGb}|{MultiAz}";
    }
}
