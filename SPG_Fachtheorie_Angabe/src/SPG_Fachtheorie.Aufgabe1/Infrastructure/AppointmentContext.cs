using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Model;
using static System.Net.Mime.MediaTypeNames;

namespace SPG_Fachtheorie.Aufgabe1.Infrastructure
{
    public class AppointmentContext : DbContext
    {
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentItem> PaymentItems => Set<PaymentItem>();
        public DbSet<CashDesk> CashDesks => Set<CashDesk>();
        public DbSet<Cashier> Cashiers => Set<Cashier>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<Employee> Employees => Set<Employee>();


        // TODO: Add your DbSets here
        public AppointmentContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO: Add your configuration here
            modelBuilder.Entity<Employee>().OwnsOne(a => a.Address);

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentType).HasConversion<string>();
            modelBuilder.Entity<Employee>()
                    .HasDiscriminator<string>("Type")  // Definiert das Discriminator-Feld
                    .HasValue<Manager>("Manager")      // Wert für die Manager-Klasse
                    .HasValue<Cashier>("Cashier");

        }
    }
}