using Mapster;
using RailBook.ApiModels;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Booking;
using RailBook.Manager.Interfaces;


namespace RailBook.Manager.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IInvoiceService _invoiceService;

        public BookingService(IBookingRepository bookingRepository, IInvoiceService invoiceService)
        {
            _bookingRepository = bookingRepository;
            _invoiceService = invoiceService;
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Adapt<List<BookingDto>>();
        }

        public async Task<ApiResponse<BookingDto>> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                // Create ApiResponse with 404
                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status404NotFound,  // ⬅️ Service decides
                    Success = false,
                    Message = "Booking not found",
                    Data = null,
                    Errors = new List<string> { $"No booking with ID {id}" },
                    Timestamp = DateTime.UtcNow
                };
            }
            var dto = booking.Adapt<BookingDto>();

            // Create ApiResponse with 200
            return new ApiResponse<BookingDto>
            {
                StatusCode = StatusCodes.Status200OK,  // ⬅️ Service decides
                Success = true,
                Message = "Booking retrieved successfully",
                Data = dto,
                Errors = null,
                Timestamp = DateTime.UtcNow
            };
        }


        public async Task<ApiResponse<BookingDto>> AddBookingAsync(CreateBookingDto dto)
        {
            try
            {
                var booking = dto.Adapt<Booking>();

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
                
                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Booking created successfully",
                    Data = booking.Adapt<BookingDto>(),
                    Errors = null,
                    Timestamp = DateTime.UtcNow
                };

            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "An error occurred while creating the booking",
                    Data = null,
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                };
            }

        }

        public async Task UpdateBookingAsync(BookingDto bookingDto)
        {   
            var booking = bookingDto.Adapt<Booking>();
            await _bookingRepository.UpdateAsync(booking);
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

    }
}
