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
        public DbSet<User> Users => Set<User>();
        public DbSet<ClassAttendance> ClassAttendances => Set<ClassAttendance>();
        public DbSet<ClassAttendanceStudent> ClassAttendanceStudents => Set<ClassAttendanceStudent>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(u => u.Login)
                      .IsRequired()
                      .HasMaxLength(64);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(u => u.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(512);

                entity.Property(u => u.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(u => u.Login)
                      .IsUnique();

                entity.HasIndex(u => u.Email)
                      .IsUnique();
            });

            modelBuilder.Entity<Group>().ToTable("groups");
            modelBuilder.Entity<Student>().ToTable("students");
            modelBuilder.Entity<Class>().ToTable("classes");

            modelBuilder.Entity<ClassAttendance>(e =>
            {
                e.ToTable("class_attendance");
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).ValueGeneratedOnAdd();

                e.HasOne(a => a.Class)
                 .WithMany(c => c.Attendance)
                 .HasForeignKey(a => a.ClassId);

                e.Property(a => a.Date).HasColumnType("date");
                e.HasIndex(a => new { a.ClassId, a.Date }).IsUnique();
            });

            modelBuilder.Entity<ClassAttendanceStudent>(e =>
            {
                e.ToTable("class_attendance_students");
                e.HasKey(x => new { x.ClassAttendanceId, x.StudentId });

                e.HasOne(x => x.ClassAttendance)
                 .WithMany(a => a.Students)
                 .HasForeignKey(x => x.ClassAttendanceId);

                e.HasOne(x => x.Student)
                 .WithMany()
                 .HasForeignKey(x => x.StudentId);
            });
        }
    }
}
