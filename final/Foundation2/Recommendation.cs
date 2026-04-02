namespace CloudSimulator
{
    /// Represents a single cost-saving recommendation produced by the RecommendationEngine.
    /// Holds a human-readable description of the suggested change, the estimated monthly
    /// savings if applied, and the ID of the specific resource the suggestion targets.
    public class Recommendation
    {
        public string  Description             { get; private set; }
        public decimal EstimatedMonthlySavings { get; private set; }
        public string  AffectedResourceId      { get; private set; }

        public Recommendation(string description,
                              decimal estimatedMonthlySavings,
                              string affectedResourceId)
        {
            Description             = description;
            EstimatedMonthlySavings = estimatedMonthlySavings;
            AffectedResourceId      = affectedResourceId;
        }

        public override string ToString()
        {
            return $"[Save ${EstimatedMonthlySavings:F2}/mo] {Description} (Resource: {AffectedResourceId})";
        }
    }
}
