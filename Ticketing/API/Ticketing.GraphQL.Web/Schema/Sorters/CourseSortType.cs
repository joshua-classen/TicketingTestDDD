using HotChocolate.Data.Sorting;
using Ticketing.GraphQL.Web.Schema.Queries;

namespace Ticketing.GraphQL.Web.Schema.Sorters;

public class CourseSortType : SortInputType<CourseType>
{
    protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
    {
        descriptor.Ignore(c => c.Id);
        descriptor.Ignore(c => c.InstructorId);
        
        base.Configure(descriptor);
    }
}