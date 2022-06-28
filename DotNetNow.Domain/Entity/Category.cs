using System.Collections.Generic;

namespace DotNetNow.Domain.Entity
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public virtual Category Parent { get; set; }
        public ICollection<Category> Children { get; set; } = new HashSet<Category>();
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
