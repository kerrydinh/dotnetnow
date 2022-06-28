using System.Collections.Generic;

namespace DotNetNow.Domain.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    }
}
