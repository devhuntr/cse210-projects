namespace CloudSimulator
{
    // A single cost-saving suggestion produced by the RecommendationEngine.
    public class Recommendation
    {
        public string  Description             { get; private set; }
        public decimal EstimatedMonthlySavings { get; private set; }
        public string  AffectedResourceId      { get; private set; }

        public Recommendation(string description, decimal estimatedMonthlySavings, string affectedResourceId)
        {
            Description             = description;
            EstimatedMonthlySavings = estimatedMonthlySavings;
            AffectedResourceId      = affectedResourceId;
        }

        public override string ToString() =>
            $"  [Save ${EstimatedMonthlySavings:F2}/mo] {Description}";
    }
}
