using AutoMapper;
using RailBook.Core.Application.Dtos.Booking;
using RailBook.Core.Application.Dtos.Invoice;
using RailBook.Core.Application.Dtos.InvoiceDetails;
using RailBook.Core.Application.Dtos.Passenger;
using RailBook.Core.Application.Dtos.Service;
using RailBook.Core.Application.Dtos.User;
using RailBook.Core.Domain.Entities;

namespace RailBook.Core.Application.Mapping
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
            CreateMap<Service, ServiceDto>().ReverseMap();
            CreateMap<Service, CreateServiceDto>().ReverseMap();
            CreateMap<Service, UpdateServiceDto>().ReverseMap();

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
