using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RailBook.Domain.Dtos.Passenger;
using RailBook.Manager.Implementations;


namespace RailBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly PassengerService _service;
        private readonly IMapper _mapper;

        public PassengersController(PassengerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PassengerDto>>> GetAll()
        {
            var passengers = await _service.GetAllPassengersAsync();
            return Ok(_mapper.Map<List<PassengerDto>>(passengers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PassengerDto>> GetById(int id)
        {
            var passenger = await _service.GetPassengerByIdAsync(id);
            if (passenger == null) return NotFound();
            return Ok(_mapper.Map<PassengerDto>(passenger));
        }

        [HttpPost]
        public async Task<ActionResult<PassengerDto>> Create([FromBody] CreatePassengerDto dto)
        {
            var passenger = _mapper.Map<Passenger>(dto);
            await _service.AddPassengerAsync(passenger);
            return CreatedAtAction(nameof(GetById), new { id = passenger.Id }, _mapper.Map<PassengerDto>(passenger));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePassengerDto dto)
        {
            var passenger = await _service.GetPassengerByIdAsync(id);
            if (passenger == null) return NotFound();

            _mapper.Map(dto, passenger);
            await _service.UpdatePassengerAsync(passenger);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeletePassengerAsync(id);
            return NoContent();
        }
    }
}
