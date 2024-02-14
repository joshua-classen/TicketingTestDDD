using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.VeranstalterDBMapping;

public class VeranstaltungMapping : IEntityTypeConfiguration<Veranstaltung>
{
    public void Configure(EntityTypeBuilder<Veranstaltung> builder)
    {
        builder.ToTable(nameof(Veranstaltung));
        builder.HasKey(veranstaltung => veranstaltung.Id);
        builder.Property(veranstaltung => veranstaltung.Id).ValueGeneratedOnAdd();

        builder.Property(veranstaltung => veranstaltung.Name);
    }
}