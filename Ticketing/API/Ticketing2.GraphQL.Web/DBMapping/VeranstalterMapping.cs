using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing2.GraphQL.Web.DomainObjects;

namespace Ticketing2.GraphQL.Web.DBMapping;

public class VeranstalterMapping : IEntityTypeConfiguration<VeranstalterUser>
{
    public void Configure(EntityTypeBuilder<VeranstalterUser> builder)
    {
        builder.ToTable(nameof(VeranstalterUser));
        builder.HasKey(veranstalter => veranstalter.Id);
        builder.Property(veranstalter => veranstalter.Id).ValueGeneratedOnAdd();
        
        
        // das muss später ein fremdschlüssel sein
        builder.Property(veranstalter => veranstalter.AspNetUserId);

        builder.HasMany(veranstalter => veranstalter.Veranstaltungen);
    }
}