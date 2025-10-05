using AutoMapper;
using RailBook.Domain.Dtos.Booking;
using RailBook.Domain.Dtos.Invoice;
using RailBook.Domain.Dtos.InvoiceDetails;
using RailBook.Domain.Dtos.Passenger;
using RailBook.Domain.Dtos.Service;
using RailBook.Domain.Dtos.User;
using RailBook.Domain.Entities;

namespace RailBook.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();

            // Passenger
            CreateMap<Passenger, PassengerDto>().ReverseMap();
            CreateMap<Passenger, CreatePassengerDto>().ReverseMap();
            CreateMap<Passenger, UpdatePassengerDto>().ReverseMap();

            // Service
            CreateMap<TrainService, TrainServiceDto>().ReverseMap();
            CreateMap<TrainService, CreateTrainServiceDto>().ReverseMap();
            CreateMap<TrainService, UpdateTrainServiceDto>().ReverseMap();

            // Booking
            CreateMap<Booking, BookingDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();
            CreateMap<Booking, UpdateBookingDto>().ReverseMap();

            // Invoice
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Invoice, CreateInvoiceDto>().ReverseMap();
            CreateMap<Invoice, UpdateInvoiceDto>().ReverseMap();

            // InvoiceDetails
            CreateMap<InvoiceDetails, InvoiceDetailsDto>().ReverseMap();
            CreateMap<InvoiceDetails, CreateInvoiceDetailsDto>().ReverseMap();
            CreateMap<InvoiceDetails, UpdateInvoiceDetailsDto>().ReverseMap();
        }
    }

}
