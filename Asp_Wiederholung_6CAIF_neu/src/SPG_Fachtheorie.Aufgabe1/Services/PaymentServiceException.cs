using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe1.Services
{
    public class PaymentServiceException : Exception
    {
        public PaymentServiceException(string message)
            : base(message)
        {
        }
    }
}
