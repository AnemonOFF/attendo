using Attendo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Group> Groups { get; }
    DbSet<Student> Students { get; }
    DbSet<Class> Classes { get; }
    DbSet<User> Users { get; }
    DbSet<ClassAttendance> ClassAttendances { get; }
    DbSet<ClassAttendanceStudent> ClassAttendanceStudents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
