using Mapster;
using RailBook.ApiModels;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Booking;
using RailBook.Manager.Interfaces;
using System.Security.Claims;

namespace RailBook.Manager.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IInvoiceService _invoiceService;
        private readonly IPassengerService _passengerService;
        private readonly ITrainServiceService _trainServiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookingService(
            IBookingRepository bookingRepository,
            IInvoiceService invoiceService,
            IPassengerService passengerService,
            ITrainServiceService trainServiceService,
            IHttpContextAccessor httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _invoiceService = invoiceService;
            _passengerService = passengerService;
            _trainServiceService = trainServiceService;
            _httpContextAccessor = httpContextAccessor;
        }

        

        public async Task<ApiResponse<List<BookingDto>>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return new ApiResponse<List<BookingDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Bookings retrieved successfully",
                Data = bookings.Adapt<List<BookingDto>>(),
                Errors = null,
                Timestamp = DateTime.UtcNow
            };
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
                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "Booking not found",
                    Data = null,
                    Errors = new List<string> { $"No booking with ID {id}" },
                    Timestamp = DateTime.UtcNow
                };
            }

            var dto = booking.Adapt<BookingDto>();
            return new ApiResponse<BookingDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Booking retrieved successfully",
                Data = dto,
                Errors = null,
                Timestamp = DateTime.UtcNow
            };
        }


        public int? GetCurrentUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity.IsAuthenticated)
                return null;

            // If you added "userId" or "sub" as claim in your token
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? user.FindFirst("userId")?.Value;

            return userIdClaim != null ? int.Parse(userIdClaim) : null;
        }


        public async Task<ApiResponse<BookingDto>> AddBookingAsync(CreateBookingDto dto)
        {
            try
            {
                // Step 1: Convert DTO to entity
                var booking = dto.Adapt<Booking>();

                // Step 2: Set booking metadata
                booking.Status = "Confirmed";
                booking.BookingDate = DateTime.UtcNow;
                booking.CreatedBy = GetCurrentUserId() ?? 1; // Default to 1 if user ID not found
                booking.CreatedAt = DateTime.UtcNow;

                // Step 3: Set passenger metadata
                if (booking.Passengers != null && booking.Passengers.Any())
                {
                    foreach (var passenger in booking.Passengers)
                    {
                        passenger.CreatedBy = 1;
                        passenger.CreatedAt = DateTime.UtcNow;

                        // Set train service metadata for each passenger
                        if (passenger.TrainServices != null && passenger.TrainServices.Any())
                        {
                            foreach (var service in passenger.TrainServices)
                            {
                                service.CreatedBy = 1;
                                service.CreatedAt = DateTime.UtcNow;
                            }
                        }
                    }
                }

                // Step 4: Generate invoice (calculates total automatically)
                booking.Invoice = await _invoiceService.GenerateInvoiceAsync(booking);

                // Step 5: Save everything to database (EF Core handles the cascade)
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
            try
            {
                // Step 1: Get existing booking with all related data (lazy loading will load them)
                var existingBooking = await GetBookingById(id);

                if (existingBooking == null)
                {
                    return new ApiResponse<BookingDto>
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Booking not found",
                        Data = null,
                        Errors = new List<string> { $"No booking with ID {id}" },
                        Timestamp = DateTime.UtcNow
                    };
                }

                // Step 2: Update basic booking fields
                existingBooking.Source = updatedBookingDto.Source;
                existingBooking.Destination = updatedBookingDto.Destination;
                existingBooking.PerTicketPrice = updatedBookingDto.PerTicketPrice;
                existingBooking.Status = updatedBookingDto.Status;
                existingBooking.ModifiedById = GetCurrentUserId() ?? 1; // Default to 1 if user ID not found
                existingBooking.ModifiedAt = DateTime.UtcNow;

                // Step 3: Update passengers (this handles add/update/delete of passengers AND their services)
                await _passengerService.UpdatePassengersWithServicesAsync(existingBooking, updatedBookingDto);

                // Step 4: Recalculate and update invoice
                existingBooking.Invoice = await _invoiceService.GenerateInvoiceAsync(existingBooking);

                // Step 5: Save all changes (EF Core tracks everything)
                await _bookingRepository.UpdateAsync(existingBooking);

                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Booking updated successfully",
                    Data = existingBooking.Adapt<BookingDto>(),
                    Errors = null,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "An error occurred while updating the booking",
                    Data = null,
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        public async Task DeleteBookingAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

        public async Task<ApiResponse<BookingDto>> CancelBookingAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                return new ApiResponse<BookingDto>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Success = false,
                    Message = "Booking not found",
                    Data = null,
                    Errors = new List<string> { $"No booking with ID {id}" },
                    Timestamp = DateTime.UtcNow
                };
            }

            booking.Status = "Cancelled";
            booking.ModifiedById = GetCurrentUserId() ?? 1; // Default to 1 if user ID not found
            booking.ModifiedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking);
            return new ApiResponse<BookingDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = "Booking cancelled successfully",
                Data = booking.Adapt<BookingDto>(),
                Errors = null,
                Timestamp = DateTime.UtcNow
            };

        }
    }
}