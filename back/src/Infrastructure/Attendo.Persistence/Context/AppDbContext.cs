using Attendo.Application.Interfaces;
using Attendo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Attendo.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
    }
}