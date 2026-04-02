using System.Collections.Generic;
/// Acts as a container for a collection of cloud resources, representing
/// a complete cloud infrastructure design.
/// The resources are validated before being added, and the class can calculate
/// the total monthly cost across all resources it holds.
/// This is like a shopping cart for cloud resources, but instead of buying them, 
/// the program are simulating the cost of them.
namespace CloudSimulator
{
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

        public void AddResource(CloudResource resource) { }
        public bool RemoveResource(string resourceId) => false;
        public decimal TotalMonthlyCost() => 0m;
        public List<T> GetResourcesByType<T>() where T : CloudResource => new List<T>();
    }
}
