using EmployeeCafeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCafeManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
                
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<EmployeeCafe> EmployeesCafe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeCafe>()
                .HasOne(ec => ec.Employee)
                .WithOne(e => e.EmployeeCafe)
                .HasForeignKey<EmployeeCafe>(ec => ec.EmployeeId);

            modelBuilder.Entity<EmployeeCafe>()
                .HasOne(ec => ec.Cafe)
                .WithMany(c => c.EmployeeCafes)
                .HasForeignKey(ec => ec.CafeId);


            modelBuilder.Entity<Employee>().HasData(
                new Employee() { Id = "ECMABC1111", Name = "Ramesh", EmailAddress = "ramesh@gmail.com", PhoneNumber = "12345678", Gender = "Male" },
                new Employee() { Id = "ECMABC2222", Name = "Suresh", EmailAddress = "sures@gmail.com", PhoneNumber = "12345678", Gender = "Male" },
                new Employee() { Id = "ECMABC3333", Name = "Bavesh", EmailAddress = "bavesh@gmail.com", PhoneNumber = "12345678", Gender = "Male" }
            );

            modelBuilder.Entity<Cafe>().HasData(
                new Cafe() { Id = new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), Name = "Limca", Description = "A Lush Creamy Lussy drink with Chilled Flavour.", Location = "Mumbai", Logo = "https://example.com/logos/sunset-brew.png" },
                new Cafe() { Id = new Guid("4ab1234f-9d86-401f-a21b-16421fffc058"), Name = "Maaza", Description = "A Lush Creamy Lussy drink with Chilled Flavour.", Location = "Bangalore", Logo = "https://example.com/logos/sunset-brew.png" }
            );

            modelBuilder.Entity<EmployeeCafe>().HasData(
                new EmployeeCafe() { EmployeeId = "ECMABC1111", CafeId = new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), StartDate=Convert.ToDateTime("2025-12-14 17:09:53.4560000") },
                new EmployeeCafe() { EmployeeId = "ECMABC2222", CafeId = new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), StartDate = Convert.ToDateTime("2025-12-14 17:09:53.4560000") },
                new EmployeeCafe() { EmployeeId = "ECMABC3333", CafeId = new Guid("0ab5321e-9d86-401f-a21b-16421fffc058"), StartDate = Convert.ToDateTime("2025-12-14 17:09:53.4560000") }
            );

        }
    }
}
