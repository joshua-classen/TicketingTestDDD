using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.DBMapping;

public class TicketMapping : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable(nameof(Ticket));
        builder.HasKey(ticket => ticket.Id);
        builder.Property(ticket => ticket.Id).ValueGeneratedOnAdd();

        builder.Property(ticket => ticket.Name);
    }
}