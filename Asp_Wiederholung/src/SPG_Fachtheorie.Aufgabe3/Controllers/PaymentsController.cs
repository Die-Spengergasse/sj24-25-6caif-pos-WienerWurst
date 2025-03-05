using Microsoft.AspNetCore.Mvc;
using SPG_Fachtheorie.Aufgabe1.Infrastructure;
using SPG_Fachtheorie.Aufgabe1.Model;
using SPG_Fachtheorie.Aufgabe3.Dtos;
using System.Collections.Generic;

namespace SPG_Fachtheorie.Aufgabe3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class PaymentsController : ControllerBase
    {
        private readonly AppointmentContext _db;

        public PaymentsController(AppointmentContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<List<PaymentDto>> GetAllPayments([FromQuery] int? cashDesk, DateTime? dateFrom)
        {
            var paymentsQuery = _db.Payments.AsQueryable();
            if (cashDesk.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(e => e.CashDesk.Number == cashDesk.Value);
            }

            // Filter nach Datum (dateFrom), falls angegeben
            if (dateFrom.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(e => e.PaymentDateTime >= dateFrom.Value);
            }

            var payments = _db.Payments
                .Select(e => new PaymentDto(
                    e.Id, e.Employee.FirstName, e.Employee.LastName,
                    e.CashDesk.Number, e.PaymentType.ToString(), e.PaymentItems.Sum(p => p.Price)))
                .ToList();    //  // [{...}, {...}, ... ]


            return Ok(payments);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PaymentDetailDto> GetPayment(int id)
        {
            var payment = _db.Payments
                .Where(e => e.Id == id)
                .Select(e => new PaymentDetailDto(
                    e.Id, e.Employee.FirstName, e.Employee.LastName, e.CashDesk.Number, e.PaymentType.ToString(),
                    e.PaymentItems.Select(p => new PaymentItemDto(p.ArticleName, p.Amount, p.Price)).ToList()
                    )).FirstOrDefault();



            if (payment is null) { return NotFound(); }
            return Ok(payment);
        }
    }
}
