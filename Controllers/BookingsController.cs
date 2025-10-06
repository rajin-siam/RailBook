using AutoMapper;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using RailBook.Dtos.Booking;
using RailBook.Manager.Implementations;
using RailBook.Manager.Interfaces;


namespace RailBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetAll()
        {
            var bookings = await _service.GetAllBookingsAsync();
            return Ok(bookings.Adapt<List<BookingDto>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apiResponse = await _service.GetBookingByIdAsync(id);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var apiResponse = await _service.AddBookingAsync(dto);

            // Pass to StatusCode() method
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto)
        //{
        //    var booking = await _service.GetBookingByIdAsync(id);
        //    if (booking == null) return NotFound();

        //    _mapper.Map(dto, booking);
        //    await _service.UpdateBookingAsync(booking);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _service.DeleteBookingAsync(id);
        //    return NoContent();
        //}
    }
}
