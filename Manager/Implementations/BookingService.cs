using Mapster;
using Microsoft.EntityFrameworkCore;
using RailBook.ApiModels;
using RailBook.Controllers;
using RailBook.DataAccess.Implementations;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Booking;
using RailBook.Manager.Interfaces;


namespace RailBook.Manager.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IInvoiceService _invoiceService;
        private readonly IPassengerService _passengerService;

        public BookingService(IBookingRepository bookingRepository, IInvoiceService invoiceService, IPassengerService passengerService)
        {
            _bookingRepository = bookingRepository;
            _invoiceService = invoiceService;
            _passengerService = passengerService;
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Adapt<List<BookingDto>>();
        }

        public async Task<Booking?> GetBookingById(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
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

       public async Task<ApiResponse<BookingDto>> UpdateBookingAsync(int id, UpdateBookingDto updatedBookingDto)
{
    // Start transaction
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    try
    {
        var existingBooking = await GetBookingById(id);

        if (existingBooking == null)
        {
            return new ApiResponse<BookingDto>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Success = false,
                Message = "Booking not found"
            };
        }

        // 1️⃣ Update parent fields
        existingBooking.Source = updatedBookingDto.Source;
        existingBooking.Destination = updatedBookingDto.Destination;
        existingBooking.PerTicketPrice = updatedBookingDto.PerTicketPrice;
        existingBooking.ModifiedById = 1;
        existingBooking.ModifiedAt = DateTime.UtcNow;

        // 2️⃣ Update passengers (without saving)
        await _passengerService.UpdatePassengerAsync(existingBooking, updatedBookingDto);

        // 3️⃣ Recalculate invoice
        existingBooking.Invoice = await _invoiceService.GenerateInvoiceAsync(existingBooking);

        // 4️⃣ Save ALL changes in ONE transaction
        await _context.SaveChangesAsync();
        
        // 5️⃣ Commit transaction
        await transaction.CommitAsync();

        return new ApiResponse<BookingDto>
        {
            StatusCode = StatusCodes.Status200OK,
            Success = true,
            Message = "Booking updated successfully",
            Data = existingBooking.Adapt<BookingDto>(),
            Timestamp = DateTime.UtcNow
        };
    }
    catch (Exception ex)
    {
        // Rollback on any error
        await transaction.RollbackAsync();
        
        return new ApiResponse<BookingDto>
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Success = false,
            Message = "An error occurred while updating the booking",
            Errors = new List<string> { ex.Message },
            Timestamp = DateTime.UtcNow
        };
    }
}
        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

    }
}
