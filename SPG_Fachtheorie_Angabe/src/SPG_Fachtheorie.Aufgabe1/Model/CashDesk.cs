using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Numerics;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class CashDesk
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        public CashDesk(int number)
        {
            Number = number;
        }
    }
}