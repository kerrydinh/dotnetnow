using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DotNetNow.Domain.Entity;

namespace DotNetNow.Domain.Config
{
    public class CategoryConfiguration : BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder
                .HasOne(o => o.Parent)
                .WithMany(o => o.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
                .HasMany(p => p.Products)
                .WithMany(p => p.Categories)
                .UsingEntity(j => j.ToTable("CategoryProduct"));
        }
    }
}
