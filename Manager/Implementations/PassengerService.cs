using Mapster;
using RailBook.DataAccess.Interfaces;
using RailBook.Dtos.Booking;
using RailBook.Dtos.Passenger;
using RailBook.Manager.Interfaces;

namespace RailBook.Manager.Implementations
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly ITrainServiceRepository _trainServiceRepository;

        public PassengerService(
            IPassengerRepository passengerRepository,
            ITrainServiceRepository trainServiceRepository)
        {
            _passengerRepository = passengerRepository;
            _trainServiceRepository = trainServiceRepository;
        }

        public async Task<List<PassengerDto>> GetAllPassengersAsync()
        {
            var passengerList = await _passengerRepository.GetAllAsync();
            return passengerList.Adapt<List<PassengerDto>>();
        }

        public async Task<PassengerDto?> GetPassengerByIdAsync(int id)
        {
            var passenger = await _passengerRepository.GetByIdAsync(id);
            return passenger?.Adapt<PassengerDto>();
        }

        public async Task AddPassengerAsync(PassengerDto passengerDto)
        {
            var passenger = passengerDto.Adapt<Passenger>();
            await _passengerRepository.AddAsync(passenger);
        }

        // ✅ NEW METHOD: Handles passengers AND their train services
        public async Task UpdatePassengersWithServicesAsync(Booking existingBooking, UpdateBookingDto updatedBookingDto)
        {
            // Get IDs from existing and updated data
            var existingPassengerIds = existingBooking.Passengers.Select(p => p.Id).ToList();
            var updatedPassengerIds = updatedBookingDto.Passengers.Select(p => p.Id).ToList();

            // ==================== STEP 1: ADD NEW PASSENGERS ====================
            var newPassengerDtos = updatedBookingDto.Passengers
                .Where(p => p.Id == 0 || !existingPassengerIds.Contains(p.Id))
                .ToList();

            foreach (var passengerDto in newPassengerDtos)
            {
                var newPassenger = new Passenger
                {
                    Name = passengerDto.Name,
                    Age = passengerDto.Age,
                    Gender = passengerDto.Gender,
                    BookingId = existingBooking.Id,
                    CreatedBy = 1,
                    CreatedAt = DateTime.UtcNow,
                    TrainServices = new List<TrainService>()
                };

                // Add train services for this new passenger
                if (passengerDto.TrainServices != null && passengerDto.TrainServices.Any())
                {
                    foreach (var serviceDto in passengerDto.TrainServices)
                    {
                        newPassenger.TrainServices.Add(new TrainService
                        {
                            ServiceName = serviceDto.ServiceName ?? string.Empty,
                            Price = serviceDto.Price ?? 0,
                            CreatedBy = 1,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                existingBooking.Passengers.Add(newPassenger);
            }

            // ==================== STEP 2: UPDATE EXISTING PASSENGERS ====================
            var passengersToUpdate = updatedBookingDto.Passengers
                .Where(p => p.Id > 0 && existingPassengerIds.Contains(p.Id))
                .ToList();

            foreach (var passengerDto in passengersToUpdate)
            {
                var existingPassenger = existingBooking.Passengers.First(p => p.Id == passengerDto.Id);

                // Update passenger basic info
                existingPassenger.Name = passengerDto.Name;
                existingPassenger.Age = passengerDto.Age;
                existingPassenger.Gender = passengerDto.Gender;
                existingPassenger.ModifiedById = 1;
                existingPassenger.ModifiedAt = DateTime.UtcNow;

                // ==================== UPDATE TRAIN SERVICES FOR THIS PASSENGER ====================
                if (passengerDto.TrainServices != null)
                {
                    var existingServiceIds = existingPassenger.TrainServices.Select(s => s.Id).ToList();
                    var updatedServiceIds = passengerDto.TrainServices
                        .Where(s => s.Id > 0)
                        .Select(s => s.Id)
                        .ToList();

                    // ADD new services
                    var newServices = passengerDto.TrainServices
                        .Where(s => s.Id == 0 || !existingServiceIds.Contains(s.Id))
                        .ToList();

                    foreach (var serviceDto in newServices)
                    {
                        existingPassenger.TrainServices.Add(new TrainService
                        {
                            ServiceName = serviceDto.ServiceName ?? string.Empty,
                            Price = serviceDto.Price ?? 0,
                            PassengerId = existingPassenger.Id,
                            CreatedBy = 1,
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    // UPDATE existing services
                    var servicesToUpdate = passengerDto.TrainServices
                        .Where(s => s.Id > 0 && existingServiceIds.Contains(s.Id))
                        .ToList();

                    foreach (var serviceDto in servicesToUpdate)
                    {
                        var existingService = existingPassenger.TrainServices.First(s => s.Id == serviceDto.Id);

                        if (!string.IsNullOrEmpty(serviceDto.ServiceName))
                            existingService.ServiceName = serviceDto.ServiceName;

                        if (serviceDto.Price.HasValue)
                            existingService.Price = serviceDto.Price.Value;

                        existingService.ModifiedById = 1;
                        existingService.ModifiedAt = DateTime.UtcNow;
                    }

                    // DELETE removed services
                    var servicesToRemove = existingPassenger.TrainServices
                        .Where(s => !updatedServiceIds.Contains(s.Id))
                        .ToList();

                    foreach (var serviceToRemove in servicesToRemove)
                    {
                        await _trainServiceRepository.DeleteAsync(serviceToRemove.Id);
                    }
                }
            }

            // ==================== STEP 3: DELETE REMOVED PASSENGERS ====================
            var passengersToRemove = existingBooking.Passengers
                .Where(p => !updatedPassengerIds.Contains(p.Id))
                .ToList();

            foreach (var passengerToRemove in passengersToRemove)
            {
                // Delete all services first (if not cascade)
                foreach (var service in passengerToRemove.TrainServices.ToList())
                {
                    await _trainServiceRepository.DeleteAsync(service.Id);
                }

                // Then delete passenger
                await _passengerRepository.DeleteAsync(passengerToRemove.Id);
            }
        }

        public async Task DeletePassengerAsync(int id)
        {
            await _passengerRepository.DeleteAsync(id);
        }
    }
}