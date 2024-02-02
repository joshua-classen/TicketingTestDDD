using Ticketing.GraphQL.Web.DTOs;
using Ticketing.GraphQL.Web.Services.Instructors;

namespace Ticketing.GraphQL.Web.DataLoaders;

public class InstructorDataLoader : BatchDataLoader<Guid, InstructorDTO>
{
    private readonly InstructorsRepository _instructorsRepository;
    
    public InstructorDataLoader(InstructorsRepository instructorsRepository,IBatchScheduler batchScheduler, DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        _instructorsRepository = instructorsRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, InstructorDTO>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var instructors = await _instructorsRepository.GetManyByIds(keys);
        
        return instructors.ToDictionary(i => i.Id);
    }
}