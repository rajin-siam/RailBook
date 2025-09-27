using RailBook.Core.Domain.Entities;
using RailBook.Core.Domain.Repositories;


namespace RailBook.Core.Application.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _bookingRepository.GetByIdAsync(id);
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _bookingRepository.AddAsync(booking);
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
