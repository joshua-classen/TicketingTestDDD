using Ticketing.GraphQL.Web.Schema.Filters;
using Ticketing.GraphQL.Web.Schema.Sorters;
using Ticketing.GraphQL.Web.Services;
using Ticketing.GraphQL.Web.Services.Courses;

namespace Ticketing.GraphQL.Web.Schema.Queries;

public class Query
{
    private readonly CoursesRepository _coursesRepository;

    public Query(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    // [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)] // not efficient
    // [UseFiltering]
    public async Task<IEnumerable<CourseType>> GetCourses()
    {
        var courseDTOs = await _coursesRepository.GetAll();
        
        //CourseType()  wird aufgerufen. Dann wird nach einiger Zeit auch LoadBatchAsync getriggert. Warum?
        var coursesType = courseDTOs.Select(c => new CourseType()  
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId
        });

        return coursesType;
    }
    
    [UseDbContext(typeof(SchoolDbContext))]
    [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
    [UseProjection]
    [UseFiltering(typeof(CourseFilterType))]
    [UseSorting(typeof(CourseSortType))]
    public IQueryable<CourseType> GetPaginatedCourses([ScopedService] SchoolDbContext context)
    {
        return context.Courses.Select(c => new CourseType()
        {
            Id = c.Id,
            Name = c.Name,
            Subject = c.Subject,
            InstructorId = c.InstructorId
        });
    }
    

    public async Task<CourseType> GetCourseByIdAsync(Guid id)
    {
        var courseDTO = await _coursesRepository.GetById(id);
        
        return new CourseType()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
        };
    }
    
    [GraphQLDeprecated("This query is deprecated.")]
    public string Instructions => "Smash that like button and subscribe to SingletonSean";
}