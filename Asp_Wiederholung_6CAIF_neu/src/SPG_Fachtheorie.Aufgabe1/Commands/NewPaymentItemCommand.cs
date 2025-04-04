using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe1.Commands
{
    namespace SPG_Fachtheorie.Aufgabe1.Model
    {
        public class NewPaymentItemCommand
        {
            public required string ArticleName { get; set; }
            public int Amount { get; set; }
            public decimal Price { get; set; }
            public int PaymentId { get; set; }
        }
    }

}
