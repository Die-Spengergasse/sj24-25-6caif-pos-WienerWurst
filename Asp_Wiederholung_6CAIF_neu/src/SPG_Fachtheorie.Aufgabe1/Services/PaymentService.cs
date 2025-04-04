using System;
using System.Linq;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Commands;
using SPG_Fachtheorie.Aufgabe1.Commands.SPG_Fachtheorie.Aufgabe1.Model;

namespace SPG_Fachtheorie.Aufgabe1.Services
{
    public class PaymentService
    {
        private readonly AppointmentContext _db;

        public PaymentService(AppointmentContext db)
        {
            _db = db;
        }

        // Stellt einen read-only Zugriff auf die Datenbank als Property bereit.
        public IQueryable<Payment> Payments => _db.Payments.AsQueryable();

        // 1. CreatePayment
        public Payment CreatePayment(NewPaymentCommand cmd)
        {
            var cashDesk = _db.CashDesks.Find(cmd.CashDeskNumber);
            var employee = _db.Employees.Find(cmd.EmployeeRegistrationNumber);

            if (cashDesk == null || employee == null)
                throw new PaymentServiceException("Invalid cash desk or employee.");

            if (_db.Payments.Any(p => p.CashDesk.Number == cmd.CashDeskNumber && p.Confirmed == null))
                throw new PaymentServiceException("Open payment for cashdesk.");

            if (string.IsNullOrWhiteSpace(cmd.PaymentType))
                throw new PaymentServiceException("Payment type is required.");

            var payment = new Payment(cashDesk, employee)
            {
                PaymentDateTime = cmd.PaymentDateTime,
                PaymentType = Enum.Parse<PaymentType>(cmd.PaymentType)
            };

            _db.Payments.Add(payment);
            _db.SaveChanges();

            return payment;
        }

        // 2. ConfirmPayment
        public void ConfirmPayment(int paymentId)
        {
            var payment = _db.Payments.Find(paymentId);

            if (payment == null)
                throw new PaymentServiceException("Payment not found.");

            if (payment.Confirmed != null)
                throw new PaymentServiceException("Payment already confirmed.");

            payment.Confirmed = DateTime.UtcNow;
            _db.SaveChanges();
        }

        // 3. AddPaymentItem
        public void AddPaymentItem(NewPaymentItemCommand cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd.ArticleName))
                throw new PaymentServiceException("Article name is required.");

            if (cmd.Amount <= 0 || cmd.Price < 0)
                throw new PaymentServiceException("Invalid amount or price.");

            var payment = _db.Payments.Find(cmd.PaymentId);

            if (payment == null)
                throw new PaymentServiceException("Payment not found.");

            if (payment.Confirmed != null)
                throw new PaymentServiceException("Payment already confirmed.");


            var item = new PaymentItem(
                cmd.ArticleName, cmd.Amount, cmd.Price, payment);



            _db.PaymentItems.Add(item);
            _db.SaveChanges();
        }

        // 4. DeletePayment
        public void DeletePayment(int paymentId, bool deleteItems)
        {
            var payment = _db.Payments
                .Where(p => p.Id == paymentId)
                .FirstOrDefault();

            if (payment == null)
                throw new PaymentServiceException("Payment not found.");

            if (deleteItems)
            {
                var items = _db.PaymentItems.Where(i => i.Payment.Id == paymentId);
                _db.PaymentItems.RemoveRange(items);
            }

            _db.Payments.Remove(payment);
            _db.SaveChanges();
        }
    }
}
