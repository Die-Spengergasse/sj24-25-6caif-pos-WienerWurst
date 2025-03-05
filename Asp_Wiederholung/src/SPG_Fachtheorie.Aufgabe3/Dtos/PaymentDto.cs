﻿using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SPG_Fachtheorie.Aufgabe3.Dtos
{
    public record PaymentDto(
       int Id, string EmployeeFirstName, string EmployeeLastName,
       int CashDeskNumber, string PaymentType,
       decimal TotalAmount);

}
