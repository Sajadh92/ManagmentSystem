using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.HumanResources;

public class EmployeeMap : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable(nameof(Employee), "dbo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Gender).HasMaxLength(7);
        builder.Property(x => x.Phone).HasMaxLength(15);
        builder.Property(x => x.Birthday);
        builder.Property(x => x.JobTitle).HasMaxLength(25);
        builder.Property(x => x.JobRank);

        builder.Property(x => x.CreateDate);
        builder.Property(x => x.CreateUserId);
        builder.Property(x => x.UpdateDate);
        builder.Property(x => x.UpdateUserId);
        builder.Property(x => x.IsRemoved);
        builder.Property(x => x.RemoveDate);
        builder.Property(x => x.RemoveUserId);
        builder.Property(x => x.Version).IsRowVersion();
    }
}