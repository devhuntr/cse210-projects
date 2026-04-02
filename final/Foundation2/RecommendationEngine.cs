using System.Collections.Generic;
/// This is the hardest, but the coolest one.
/// IT Analyzes an Architecture and produces a list of cost-saving recommendations.
/// Applies a set of rules to identify inefficiencies, such as oversized VMs,
/// missing load balancers, unnecessary Multi-AZ databases, and inefficient storage tiers.
/// Each recommendation includes a description and an estimated monthly savings amount.
/// This is the smart part of the program that is actually going to help
/// me and other users save!
namespace CloudSimulator
{
    public class RecommendationEngine
    {
        private readonly Architecture _architecture;

        public RecommendationEngine(Architecture architecture)
        {
            _architecture = architecture;
        }

        public List<Recommendation> Analyze()
        {
            var recommendations = new List<Recommendation>();

            foreach (var r in _architecture.Resources)
            {
                if (r is VirtualMachine vm && vm.CpuTier == "Large" && vm.RamGb <= 8)
                    recommendations.Add(new Recommendation(
                        $"Downsize '{vm.Name}' from Large to Medium — RAM usage is low",
                        50m, vm.ResourceId));

                if (r is StorageBucket bucket && bucket.AccessFrequency == "Frequent" && bucket.CapacityGb > 200)
                    recommendations.Add(new Recommendation(
                        $"Switch '{bucket.Name}' to Infrequent access to reduce storage costs",
                        bucket.CapacityGb * 0.013m, bucket.ResourceId));

                if (r is DatabaseInstance db && db.MultiAz && db.StorageGb < 50)
                    recommendations.Add(new Recommendation(
                        $"Disable Multi-AZ on '{db.Name}' — small DB may not need it",
                        80m, db.ResourceId));
            }

            return recommendations;
        }

        public decimal TotalPotentialSavings()
        {
            decimal total = 0m;
            foreach (var rec in Analyze())
                total += rec.EstimatedMonthlySavings;
            return total;
        }
    }
}
