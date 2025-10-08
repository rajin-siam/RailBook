using Mapster;
using Microsoft.AspNetCore.Mvc;
using RailBook.Dtos.Booking;
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

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetAll()
        {
            var bookings = await _service.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/bookings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apiResponse = await _service.GetBookingByIdAsync(id);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var apiResponse = await _service.AddBookingAsync(dto);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        // PUT: api/bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto)
        {
            var apiResponse = await _service.UpdateBookingAsync(id, dto);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        // DELETE: api/bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBookingAsync(id);
            return NoContent();
        }
    }
}