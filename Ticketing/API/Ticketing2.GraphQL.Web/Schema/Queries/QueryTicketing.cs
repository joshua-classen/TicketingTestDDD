using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketing2.GraphQL.Web.DomainObjects;
using Ticketing2.GraphQL.Web.Services;

namespace Ticketing2.GraphQL.Web.Schema.Queries;

public class QueryTicketing
{
    // private readonly VeranstalterRepository _veranstalterRepository;
    //
    // public QueryTicketing(VeranstalterRepository veranstalterRepository) // macht man das hier später über Dependency Injection?
    // {
    //     _veranstalterRepository = veranstalterRepository;
    // }
    
     public async Task<IEnumerable<Veranstalter>> GetVeranstalter2([FromServices] TicketingDbContext context)
     {
         var veranstalter = await context.Veranstalter.ToListAsync();
         return veranstalter;
     }
}