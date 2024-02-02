using Ticketing.GraphQL.Web.Models;
using Ticketing.GraphQL.Web.Schema.Queries;

namespace Ticketing.GraphQL.Web.Schema.Mutations;

public class CourseInputType
{
    public string Name { get; set; }
    public Subject Subject { get; set; }
    public Guid InstructorId { get; set; }
}