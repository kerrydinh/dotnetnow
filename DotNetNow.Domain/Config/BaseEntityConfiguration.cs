using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DotNetNow.Domain.Base;

namespace DotNetNow.Domain.Config
{
    public class BaseEntityConfiguration<TEntityType> : IEntityTypeConfiguration<TEntityType> where TEntityType : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntityType> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();
            builder.Property(b => b.CreatedTime)
                .HasDefaultValueSql("getutcdate()")
                .ValueGeneratedOnAdd();
        }
    }
}
