using Ticketing.GraphQL.Web.DataLoaders;
using Ticketing.GraphQL.Web.Models;

namespace Ticketing.GraphQL.Web.Schema.Queries;

public class CourseType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Subject Subject { get; set; }
    
    // [GraphQLIgnore]
    [IsProjected(true)]
    public Guid InstructorId { get; set; }

    [GraphQLNonNullType]
    public async Task<InstructorType> Instructor([Service] InstructorDataLoader instructorDataLoader) 
    {
        var instructorDTO = await instructorDataLoader.LoadAsync(InstructorId, CancellationToken.None);
        return new InstructorType()
        {
            Id = instructorDTO.Id,
            FirstName = instructorDTO.FirstName,
            LastName = instructorDTO.LastName,
            Salary = instructorDTO.Salary
        };
    }
    
    public IEnumerable<StudentType>? Students { get; set; }
}