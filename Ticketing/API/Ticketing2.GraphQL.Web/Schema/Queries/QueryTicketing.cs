using Ticketing2.GraphQL.Web.Services.Veranstalter;

namespace Ticketing2.GraphQL.Web.Schema.Queries;

public class QueryTicketing
{
    private readonly VeranstalterRepository _veranstalterRepository;
    
    public QueryTicketing(VeranstalterRepository veranstalterRepository) // macht man das hier später über Dependency Injection?
    {
        _veranstalterRepository = veranstalterRepository;
    }
    
    
    //todo: Grammatikprüfung in jetbrains rider ausstellen?
    public async Task<IEnumerable<VeranstalterType>> GetVeranstalter()
    {
        // sind veranstalterDTOs hier meine domain objekte sozusagen?
        var veranstalterDTOs = await _veranstalterRepository.GetAll();
        
        var veranstalterTypes = veranstalterDTOs.Select(v => new VeranstalterType(v.Id, v.Name, v.Email, v.HashedPassword ));
        return veranstalterTypes;
    }
    
}