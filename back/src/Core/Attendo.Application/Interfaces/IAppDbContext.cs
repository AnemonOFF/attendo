using Attendo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Group> Groups { get; }
    DbSet<Student> Students { get; }
    DbSet<Class> Classes { get; }
    DbSet<Attendance> Attendances { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
