using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.GraphQL.Web.DomainObjects;

namespace Ticketing.GraphQL.Web.DBMapping;

public class VeranstaltungMapping : IEntityTypeConfiguration<Veranstaltung>
{
    public void Configure(EntityTypeBuilder<Veranstaltung> builder)
    {
        builder.ToTable(nameof(Veranstaltung));
        builder.HasKey(veranstaltung => veranstaltung.Id);
        builder.Property(veranstaltung => veranstaltung.Id).ValueGeneratedOnAdd();

        builder.Property(veranstaltung => veranstaltung.Name);
        builder.Property(veranstaltung => veranstaltung.MaxAmountTickets);
        builder.HasMany(veranstaltung => veranstaltung.PurchasedTickets);
    }
}