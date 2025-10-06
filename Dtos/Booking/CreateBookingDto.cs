using FluentValidation;
using RailBook.Dtos.Passenger;
using RailBook.Dtos.User;

namespace RailBook.Dtos.Booking
{
    public class CreateBookingDto
    {
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int PerTicketPrice { get; set; }
        public List<CreatePassengerDto> Passengers { get; set; } = [];
    }


    public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.Source)
                .NotEmpty().WithMessage("Source station is required.")
                .Length(3, 50).WithMessage("Source name must be between 3 and 50 characters.");

            // ✅ Validate Destination
            RuleFor(x => x.Destination)
                .NotEmpty().WithMessage("Destination station is required.")
                .Length(3, 50).WithMessage("Destination name must be between 3 and 50 characters.")
                .NotEqual(x => x.Source).WithMessage("Source and destination cannot be the same.");

            // ✅ Validate PerTicketPrice
            RuleFor(x => x.PerTicketPrice)
                .GreaterThan(0).WithMessage("Ticket price must be greater than 0.");

            // ✅ Validate passengers
            RuleFor(x => x.Passengers)
                .NotEmpty().WithMessage("At least one passenger is required.");
        }
    }
}
