using System.Collections.Generic;

namespace CloudSimulator
{
    // Container for a cloud infrastructure design.
    // Validates resources before accepting them and computes total monthly cost.
    public class Architecture
    {
        public string Name { get; private set; }

        private readonly List<CloudResource> _resources = new List<CloudResource>();
        public IReadOnlyList<CloudResource> Resources => _resources.AsReadOnly();

        public int ResourceCount => _resources.Count;

        public Architecture(string name)
        {
            Name = name;
        }

        public void AddResource(CloudResource resource)
        {
            if (resource != null && resource.Validate())
                _resources.Add(resource);
        }

        public bool RemoveResource(string resourceId)
        {
            var resource = _resources.Find(r => r.ResourceId == resourceId);
            if (resource == null) return false;
            _resources.Remove(resource);
            return true;
        }

        public decimal TotalMonthlyCost()
        {
            decimal total = 0m;
            foreach (var r in _resources)
                total += r.CalculateCost();
            return total;
        }

        public List<T> GetResourcesByType<T>() where T : CloudResource
        {
            var result = new List<T>();
            foreach (var r in _resources)
                if (r is T typed) result.Add(typed);
            return result;
        }
    }
}
