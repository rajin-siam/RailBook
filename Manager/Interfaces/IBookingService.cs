using RailBook.ApiModels;
using RailBook.Dtos.Booking;

namespace RailBook.Manager.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync();
        Task<ApiResponse<BookingDto>> GetBookingByIdAsync(int id);
        Task<ApiResponse<BookingDto>> AddBookingAsync(CreateBookingDto dto);

        // ✅ Updated signature to match implementation
        Task<ApiResponse<BookingDto>> UpdateBookingAsync(int id, UpdateBookingDto updatedBookingDto);

        Task DeleteBookingAsync(int id);
    }
}