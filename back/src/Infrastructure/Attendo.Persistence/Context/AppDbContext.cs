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
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<User> Users => Set<User>();
    }
}