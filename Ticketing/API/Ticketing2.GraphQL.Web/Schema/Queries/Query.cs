using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Queries;

public class Query
{
    // private readonly VeranstalterRepository _veranstalterRepository;
    //
    // public QueryTicketing(VeranstalterRepository veranstalterRepository) // macht man das hier später über Dependency Injection?
    // {
    //     _veranstalterRepository = veranstalterRepository;
    // }
    
    
    
    // das darf z.B. eig nur der admin aufrufen.
    
    // das hier vll umstellen auf IQueryable
    // später im domain driven design darf dass dann nur ein IEnumerable sein
    // weil der Datenbankzugriff nur in der Infrastruktursschicht stattfinden darf
     public IQueryable<Veranstalter> GetVeranstalter2([FromServices] TicketingDbContext context)
     {
         var abc = context.Veranstalter.AsQueryable();
         // var veranstalter = await context.Veranstalter.ToListAsync();
         return abc;
     }
}