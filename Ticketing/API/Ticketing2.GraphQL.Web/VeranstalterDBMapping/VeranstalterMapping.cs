using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.VeranstalterDBMapping;

public class VeranstalterMapping : IEntityTypeConfiguration<Veranstalter>
{
    public void Configure(EntityTypeBuilder<Veranstalter> builder)
    {
        builder.ToTable(nameof(Veranstalter));
        builder.HasKey(veranstalter => veranstalter.Id);
        builder.Property(veranstalter => veranstalter.Id).ValueGeneratedOnAdd();

        builder.Property(veranstalter => veranstalter.Name);
        builder.Property(veranstalter => veranstalter.Email);
        builder.Property(veranstalter => veranstalter.hashPasswort);
    }
}