using RailBook.ApiModels;
using RailBook.Dtos.Booking;

namespace RailBook.Manager.Interfaces
{
    public interface IBookingService
    {
        Task<ApiResponse<List<BookingDto>>> GetAllBookingsAsync();
        Task<ApiResponse<BookingDto>> GetBookingByIdAsync(int id);
        Task<ApiResponse<BookingDto>> AddBookingAsync(CreateBookingDto dto);

        // ✅ Updated signature to match implementation
        Task<ApiResponse<BookingDto>> UpdateBookingAsync(int id, UpdateBookingDto updatedBookingDto);

        Task<ApiResponse<BookingDto>> CancelBookingAsync(int id);
    }
}