using Microsoft.EntityFrameworkCore;

namespace WorkTimeTable.DataBase
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Worker> Worker { get; set; } = null!;
        public DbSet<Department> Department { get; set; } = null!;
        public DbSet<Contract> Contract { get; set; } = null!;
        public DbSet<Timetable> Timetable { get; set; } = null!;
        public DbSet<DepartmentHierarchy> DepartmentHierarchy { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Worker>()
                .HasOne(w => w.Department)
                .WithMany(d => d.Workers)
                .HasForeignKey(w => w.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Department>()
                .HasOne(d => d.Leader)
                .WithOne(w => w.DepartmentUnderControl)
                .HasForeignKey<Department>(d => d.LeaderId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
