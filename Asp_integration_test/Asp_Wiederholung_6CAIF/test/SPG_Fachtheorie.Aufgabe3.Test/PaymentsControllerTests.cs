using Spg.Fachtheorie.Aufgabe3.API.Test;
using SPG_Fachtheorie.Aufgabe1.Commands;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe1.Services;
using SPG_Fachtheorie.Aufgabe3.Controllers;
using SPG_Fachtheorie.Aufgabe3.Dtos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe3.Test
{
    public class PaymentsControllerTests
    {
        [Theory]
        [InlineData(1, null, 2)]
        [InlineData(null, "2025-03-15", 2)]
        [InlineData(1, "2025-03-15", 1)]


        public async Task GetAllPaymentsSuccessTests(
            int? cashDesk, string? dateFromString, int count)
        {
            // ARRANGE
            DateTime? dateFrom = dateFromString is null ?
                null : DateTime.Parse(dateFromString);
            var factory = new TestWebApplicationFactory();
            factory.InitializeDatabase(db =>
            {

                var cashDesk1 = new CashDesk(1);
                var cashDesk2 = new CashDesk(2);
                var cashier = new Cashier(
                    2, "FN", "LN", new DateOnly(2004, 2, 1),
                    3000M, null, "Feinkost");
                var payment1 = new Payment(cashDesk1, new DateTime(2025, 05, 05),
                    cashier, PaymentType.Cash);
                var payment2 = new Payment(cashDesk1, new DateTime(2025, 06, 05),
                    cashier, PaymentType.Cash);
                var payment3 = new Payment(cashDesk2, new DateTime(2025, 07, 05),
                    cashier, PaymentType.Cash);
                var payment4 = new Payment(cashDesk2, new DateTime(2025, 08, 05),
                    cashier, PaymentType.Cash);

                db.AddRange(payment1, payment2, payment3, payment4);
                db.SaveChanges();
            });

            // ACT & ASSERT
            if (cashDesk is not null && dateFrom is not null)
            {
                var (statusCode, payments) = await factory.GetHttpContent<List<PaymentDto>>($"/api/payments?dateFrom=2025-05-13&cashDesk=1");
                Assert.True(statusCode == System.Net.HttpStatusCode.OK);
                Assert.NotNull(payments);
                Assert.True(payments.Count == count);
                Assert.True(payments.All(p => p.PaymentDateTime >= dateFrom && p.CashDeskNumber == cashDesk));
            }

            else if (cashDesk is not null && dateFrom is null)
            {
                var (statusCode, payments) = await factory.GetHttpContent<List<PaymentDto>>($"/api/payments?cashDesk=1");
                Assert.True(statusCode == System.Net.HttpStatusCode.OK);
                Assert.NotNull(payments);
                Assert.True(payments.Count == count);
                Assert.True(payments.All(p => p.CashDeskNumber == cashDesk));
            }

            else if (cashDesk is null && dateFrom is not null)
            {
                var (statusCode, payments) = await factory.GetHttpContent<List<PaymentDto>>($"/api/payments?dateFrom=2025-05-13");
                Assert.True(statusCode == System.Net.HttpStatusCode.OK);
                Assert.NotNull(payments);
                Assert.True(payments.Count == count);
                Assert.True(payments.All(p => p.PaymentDateTime >= dateFrom));
            }
        }
    }
}