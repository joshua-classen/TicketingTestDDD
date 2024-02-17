using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.DBMapping;

public class KundeUserMapping : IEntityTypeConfiguration<KundeUser>
{
    public void Configure(EntityTypeBuilder<KundeUser> builder)
    {
        builder.ToTable(nameof(KundeUser));
        builder.HasKey(kundeUser => kundeUser.Id);
        builder.Property(kundeUser => kundeUser.Id).ValueGeneratedOnAdd();
        
        //todo: das muss später ein fremdschlüssel sein
        builder.Property(kundeUser => kundeUser.AspNetUserId);
        // builder.HasOne(kundeUser => kundeUser.AspNetUserId); // hier bekommt der das nicht hin
        
        builder.HasMany(kundeUser => kundeUser.PurchasedTickets);
    }
}