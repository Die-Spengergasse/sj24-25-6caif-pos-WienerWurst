using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe3.Commands
{
    public class UpdateConfirmedCommand : IValidatableObject
    {
        public DateTime Confirmed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Maximal 1 Minute in der Zukunft erlaubt
            if (Confirmed > DateTime.UtcNow.AddMinutes(1))
            {
                yield return new ValidationResult(
                    "Confirmed time cannot be more than 1 minute in the future.",
                    new[] { nameof(Confirmed) });
            }
        }
    }
}
