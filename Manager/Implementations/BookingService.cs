using AutoMapper;
using RailBook.DataAccess.Interfaces;
using RailBook.Domain.Dtos.Booking;


namespace RailBook.Manager.Implementations
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly InvoiceService _invoiceService;

        public BookingService(IBookingRepository bookingRepository, InvoiceService invoiceService, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _invoiceService = invoiceService;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task<Booking?> AddBookingAsync(CreateBookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);
            booking.Status = "Confirmed";
            booking.BookingDate = DateTime.UtcNow;
            booking.CreatedBy = 1;
            booking.CreatedAt = DateTime.UtcNow;

            if (booking.Passengers != null && booking.Passengers.Any())
            {
                foreach (var passenger in booking.Passengers)
                {
                    passenger.CreatedBy = booking.CreatedBy; // or current user
                    passenger.CreatedAt = DateTime.UtcNow;
                }
            }
            booking.Invoice = await _invoiceService.GenerateInvoiceAsync(booking);
            await _bookingRepository.AddAsync(booking);
            return booking;

        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }
    }
}
