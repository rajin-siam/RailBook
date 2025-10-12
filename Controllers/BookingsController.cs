using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using RailBook.Dtos.Booking;
using RailBook.Manager.Interfaces;
using System.Security.Claims;

namespace RailBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔒 Requires authentication for ALL endpoints in this controller
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingsController(IBookingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all bookings (protected)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _service.GetAllBookingsAsync();
            return Ok(bookings.Adapt<List<BookingDto>>());
        }

        /// <summary>
        /// Get booking by ID (protected)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apiResponse = await _service.GetBookingByIdAsync(id);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        /// <summary>
        /// Create new booking (protected)
        /// Uses authenticated user's ID from JWT token
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            // Get current user ID from JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { Message = "User ID not found in token" });
            }

            // You can now use this userId for CreatedBy field
            // Pass it to your service method if needed

            var apiResponse = await _service.AddBookingAsync(dto);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        /// <summary>
        /// Update booking (protected)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookingDto dto)
        {
            var apiResponse = await _service.UpdateBookingAsync(id, dto);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

        /// <summary>
        /// Public endpoint - anyone can access (no authentication required)
        /// </summary>
        [HttpGet("public/routes")]
        [AllowAnonymous] // 🔓 Override controller-level [Authorize]
        public IActionResult GetPublicRoutes()
        {
            return Ok(new { Message = "This is a public endpoint" });
        }

        // Cancel booking (protected)
        [HttpPatch("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var apiResponse = await _service.CancelBookingAsync(id);
            return StatusCode(apiResponse.StatusCode, apiResponse);
        }
    }
}