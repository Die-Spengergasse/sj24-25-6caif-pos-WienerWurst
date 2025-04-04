using System;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public CashDesk CashDesk { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public Employee Employee { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime? Confirmed { get; set; }
        public List<PaymentItem> PaymentItems { get; }

        // Add a public constructor to make the Payment class accessible
        public Payment(CashDesk cashDesk, Employee employee)
        {
            CashDesk = cashDesk ?? throw new ArgumentNullException(nameof(cashDesk));
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
            PaymentItems = new List<PaymentItem>();
        }
    }
}

