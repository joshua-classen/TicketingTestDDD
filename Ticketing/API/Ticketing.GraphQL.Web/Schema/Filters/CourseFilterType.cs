using HotChocolate.Data.Filters;
using Ticketing.GraphQL.Web.Schema.Queries;

namespace Ticketing.GraphQL.Web.Schema.Filters;

public class CourseFilterType : FilterInputType<CourseType>
{
    protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
    {
        descriptor.Ignore(c => c.Students);
        
        base.Configure(descriptor);
    }
}