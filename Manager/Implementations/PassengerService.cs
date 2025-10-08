using Mapster;
using Microsoft.EntityFrameworkCore;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Booking;
using RailBook.Dtos.Passenger;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerService(IPassengerRepository passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<List<PassengerDto>> GetAllPassengersAsync()
        {
            var passengerList =  await _passengerRepository.GetAllAsync();
            return passengerList.Adapt<List<PassengerDto>>();
        }

        public async Task<PassengerDto?> GetPassengerByIdAsync(int id)
        {
            return (await _passengerRepository.GetByIdAsync(id)).Adapt<PassengerDto>();
        }

        public async Task AddPassengerAsync(PassengerDto passengerDto)
        {
            var passenger = passengerDto.Adapt<Passenger>();
            await _passengerRepository.AddAsync(passenger);
        }

        public async Task UpdatePassengerAsync(Booking existingBooking, UpdateBookingDto updatedBookingDto)
        {
            // 2️⃣ Handle passengers
            var existingPassengerIds = existingBooking.Passengers.Select(p => p.Id).ToList();
            var updatedPassengerIds = updatedBookingDto.Passengers.Select(p => p.Id).ToList();

            // ➕ Add new passengers
            var newPassengers = updatedBookingDto.Passengers
                .Where(p => !existingPassengerIds.Contains(p.Id))
                .Select(p => new Passenger
                {
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    BookingId = existingBooking.Id,
                    CreatedBy = 1,
                    CreatedAt = DateTime.UtcNow
                }).ToList();

            foreach (var np in newPassengers)
                existingBooking.Passengers.Add(np);

            // 🔁 Update existing passengers
            foreach (var passengerDto in updatedBookingDto.Passengers.Where(p => existingPassengerIds.Contains(p.Id)))
            {
                var existingPassenger = existingBooking.Passengers.First(x => x.Id == passengerDto.Id);
                existingPassenger.Name = passengerDto.Name;
                existingPassenger.Age = passengerDto.Age;
                existingPassenger.Gender = passengerDto.Gender;
                existingPassenger.ModifiedById = 1;
                existingPassenger.ModifiedAt = DateTime.UtcNow;
            }

            // ❌ Delete removed passengers
            var removedPassengers = existingBooking.Passengers
                .Where(p => !updatedPassengerIds.Contains(p.Id))
                .ToList();

            foreach (var rp in removedPassengers)
                 await _passengerRepository.DeleteAsync(rp.Id);

            
        }

        public async Task DeletePassengerAsync(int id)
        {
            await _passengerRepository.DeleteAsync(id);
        }


    }
}
