
using RailBook.Domain.Dtos.Booking;
using RailBook.Domain.Entities;

namespace RailBook.Manager.Interfaces
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(int id);
        Task<Booking?> AddBookingAsync(CreateBookingDto dto);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int id);
    }

}