using RailBook.ApiModels;
using RailBook.Domain.Entities;
using RailBook.Dtos.Booking;

namespace RailBook.Manager.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllBookingsAsync();
        Task<ApiResponse<BookingDto>> GetBookingByIdAsync(int id);
        Task<ApiResponse<BookingDto>> AddBookingAsync(CreateBookingDto dto);
        Task UpdateBookingAsync(BookingDto booking);
        Task DeleteBookingAsync(int id);
    }

}