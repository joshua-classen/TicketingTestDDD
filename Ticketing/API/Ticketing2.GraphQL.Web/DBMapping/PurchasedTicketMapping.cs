using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.DBMapping;

public class PurchasedTicketMapping : IEntityTypeConfiguration<PurchasedTicket>
{
    public void Configure(EntityTypeBuilder<PurchasedTicket> builder)
    {
        builder.ToTable(nameof(PurchasedTicket));
        builder.HasKey(purchasedTicket => purchasedTicket.Id);
        builder.Property(purchasedTicket => purchasedTicket.Id).ValueGeneratedOnAdd();
        
        builder.Property(purchasedTicket => purchasedTicket.TicketNumber);
        builder.Property(purchasedTicket => purchasedTicket.PurchaseDate);
        builder.Property(purchasedTicket => purchasedTicket.TicketPriceEuroCent);
        
        builder.HasOne(purchasedTicket => purchasedTicket.VeranstaltungId); // ef core setzt hier automatisch foreign key 
    }
    
}