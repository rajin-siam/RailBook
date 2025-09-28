using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RailBook.Core.Application.Dtos.Booking;
using RailBook.Core.Application.Services;
using RailBook.Core.Domain.Entities;


namespace RailBook.External.Api.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _service;
        private readonly IMapper _mapper;

        public BookingsController(BookingService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetAll()
        {
            var bookings = await _service.GetAllBookingsAsync();
            return Ok(_mapper.Map<List<BookingDto>>(bookings));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var booking = await _service.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();
            return Ok(_mapper.Map<BookingDto>(booking));
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);
            await _service.AddBookingAsync(booking);
            return CreatedAtAction(nameof(GetById), new { id = booking.BookingId }, _mapper.Map<BookingDto>(booking));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto)
        {
            var booking = await _service.GetBookingByIdAsync(id);
            if (booking == null) return NotFound();

            _mapper.Map(dto, booking);
            await _service.UpdateBookingAsync(booking);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}
