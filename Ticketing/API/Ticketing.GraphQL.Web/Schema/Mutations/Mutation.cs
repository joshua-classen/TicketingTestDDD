using HotChocolate.Subscriptions;
using Ticketing.GraphQL.Web.DTOs;
using Ticketing.GraphQL.Web.Schema.Subscriptions;
using Ticketing.GraphQL.Web.Services.Courses;

namespace Ticketing.GraphQL.Web.Schema.Mutations;

public class Mutation
{
    private readonly CoursesRepository _coursesRepository;
    
    public Mutation(CoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    // wie kann man das umbenennen durch graphQL stoppen?
    [GraphQLName("createCourse")] // funktioniert nicht. Warum?
    public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
    {
        var courseDTO = new CourseDTO()
        {
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };
        
        courseDTO = await _coursesRepository.Create(courseDTO);
        
        var course = new CourseResult()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            InstructorId = courseDTO.InstructorId
        };
        
        await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

        return course;
    }
    
    public async Task<CourseResult> UpdateCourse(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
    {
        var courseDTO = new CourseDTO()
        {
            Id = id,
            Name = courseInput.Name,
            Subject = courseInput.Subject,
            InstructorId = courseInput.InstructorId
        };

        courseDTO = await _coursesRepository.Update(courseDTO);
        
        var course = new CourseResult()
        {
            Id = courseDTO.Id,
            Name = courseDTO.Name,
            Subject = courseDTO.Subject,
            InstructorId = courseDTO.InstructorId
        };
        
        var updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
        await topicEventSender.SendAsync(updateCourseTopic, course);
        
        return course;
    }
    
    public async Task<bool> DeleteCourse(Guid id)
    {
        try
        {
            return await _coursesRepository.Delete(id);
        }
        catch (Exception)
        {
            return false;
        }
    }
}