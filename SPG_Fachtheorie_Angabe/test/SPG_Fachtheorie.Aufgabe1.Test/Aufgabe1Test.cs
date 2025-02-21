using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Model;
using System;
using System.Linq;
using Xunit;

namespace SPG_Fachtheorie.Aufgabe1.Test
{
    [Collection("Sequential")]
    public class Aufgabe1Test
    {
        private AppointmentContext GetEmptyDbContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(@"Data Source=cash.db")
                .Options;

            var db = new AppointmentContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        // Creates an empty DB in Debug\net8.0\cash.db
        [Fact]
        public void CreateDatabaseTest()
        {
            using var db = GetEmptyDbContext();
        }

        [Fact]
        public void AddCashierSuccessTest()
        {
            using var db = GetEmptyDbContext();

            // Arrange: Erstellen eines Cashiers
            var cashier = new Cashier(1001, "John", "Doe", new Address("Main Street", "City", "12345"),"Cashier" );

            // Act: Cashier zur Datenbank hinzufügen und speichern
            db.Cashiers.Add(cashier);
            db.SaveChanges();

            // Datenbank-Tracking leeren
            db.ChangeTracker.Clear();

            // Assert: Prüfen, ob der Cashier korrekt gespeichert wurde
            var cashierFromDb = db.Cashiers.First();
            Assert.NotNull(cashierFromDb);
            Assert.Equal(1001, cashierFromDb.RegistrationNumber);
        }

        [Fact]
        public void AddPaymentSuccessTest()
        {
            using var db = GetEmptyDbContext();
            // Arrange: Erstellen der benötigten Objekte
            var cashDesk = new CashDesk(1);
            var employee = new Cashier(3001, "Lisa", "Müller", new Address("Hauptstraße", "Berlin", "10115"), "Cashier");
            var payment = new Payment(cashDesk, DateTime.UtcNow, employee);

            // Act: Payment zur Datenbank hinzufügen und speichern
            db.CashDesks.Add(cashDesk);
            db.Employees.Add(employee);
            db.Payments.Add(payment);
            db.SaveChanges();

            // Datenbank-Tracking leeren
            db.ChangeTracker.Clear();

            // Assert: Prüfen, ob die Zahlung korrekt gespeichert wurde
            var paymentFromDb = db.Payments.Include(p => p.CashDesk)
                                           .Include(p => p.Employee)
                                           .FirstOrDefault();

            Assert.NotNull(paymentFromDb);
            Assert.Equal(employee.RegistrationNumber, paymentFromDb.Employee.RegistrationNumber);
            Assert.Equal(cashDesk.Number, paymentFromDb.CashDesk.Number);
        }

        [Fact]
        public void EmployeeDiscriminatorSuccessTest()
        {
            using var db = GetEmptyDbContext();

            // Arrange: Erstellen eines Cashiers
            var cashier = new Cashier(2001, "Alice", "Smith", new Address("Street 1", "City", "67890"), "Cashier");

            // Act: Cashier zur Datenbank hinzufügen und speichern
            db.Employees.Add(cashier);
            db.SaveChanges();

            // Datenbank-Tracking leeren
            db.ChangeTracker.Clear();

            // Assert: Prüfen, ob der Discriminator (Type) korrekt gesetzt wurde
            var employeeFromDb = db.Employees.First();

            Assert.NotNull(employeeFromDb);
            Assert.Equal("Cashier", employeeFromDb.Type);
        }
    }
}