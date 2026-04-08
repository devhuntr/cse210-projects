using System.Collections.Generic;

namespace CloudSimulator
{
    // Analyzes an Architecture and produces a list of cost-saving recommendations.
    public class RecommendationEngine
    {
        private readonly Architecture _architecture;

        public RecommendationEngine(Architecture architecture)
        {
            _architecture = architecture;
        }

        public List<Recommendation> Analyze()
        {
            var recs = new List<Recommendation>();

            foreach (var r in _architecture.Resources)
            {
                // Large VM with very little RAM — downsize to Medium
                if (r is VirtualMachine vm && vm.CpuTier == "Large" && vm.RamGb <= 8)
                    recs.Add(new Recommendation(
                        $"Downsize '{vm.Name}' from Large to Medium — RAM usage is low",
                        50m, vm.ResourceId));

                // Big bucket on Frequent tier — switch to Infrequent
                if (r is StorageBucket bucket && bucket.AccessFrequency == "Frequent" && bucket.CapacityGb > 200)
                    recs.Add(new Recommendation(
                        $"Switch '{bucket.Name}' to Infrequent access to cut storage costs",
                        bucket.CapacityGb * 0.013m, bucket.ResourceId));

                // Small DB with Multi-AZ enabled — probably not worth the cost
                if (r is DatabaseInstance db && db.MultiAz && db.StorageGb < 50)
                    recs.Add(new Recommendation(
                        $"Disable Multi-AZ on '{db.Name}' — small DB may not need redundancy",
                        80m, db.ResourceId));

                // Windows VM — Linux would be cheaper
                if (r is VirtualMachine winVm && winVm.OperatingSystem == "Windows")
                    recs.Add(new Recommendation(
                        $"Consider switching '{winVm.Name}' to Linux to save $15/mo on licensing",
                        15m, winVm.ResourceId));
            }

            return recs;
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
