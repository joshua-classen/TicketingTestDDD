using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.GraphQL.Web.DomainObjects;

namespace Ticketing.GraphQL.Web.DBMapping;

public class VeranstalterUserMapping : IEntityTypeConfiguration<VeranstalterUser>
{
    public void Configure(EntityTypeBuilder<VeranstalterUser> builder)
    {
        builder.ToTable(nameof(VeranstalterUser));
        builder.HasKey(veranstalter => veranstalter.Id);
        builder.Property(veranstalter => veranstalter.Id).ValueGeneratedOnAdd();
        
        
        //todo: das muss später ein fremdschlüssel sein
        builder.Property(veranstalter => veranstalter.AspNetUserId);
        // builder.HasOne(veranstalter => veranstalter.AspNetUserId); // hier bekommt der das nicht hin, das liegt wohl daran das das ein string ist.todo: Der soll hier ne andere Id nutzen.

        builder.HasMany(veranstalter => veranstalter.Veranstaltungen);
    }
}