using Atendo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Atendo.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Group> Groups { get; }
    DbSet<Student> Students { get; }
    DbSet<Event> Events { get; }
    DbSet<Attendance> Attendances { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
