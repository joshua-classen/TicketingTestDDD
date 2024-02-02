using Microsoft.EntityFrameworkCore;
using Ticketing.GraphQL.Web.DTOs;

namespace Ticketing.GraphQL.Web.Services;

public class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
    {
    }
    
    public DbSet<CourseDTO> Courses { get; set; }
    public DbSet<InstructorDTO?> Instructors { get; set; }
    public DbSet<StudentDTO> Students { get; set; }
}