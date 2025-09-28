using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RailBook.Core.Application.Dtos.Service;
using RailBook.Core.Application.Services;
using RailBook.Core.Domain.Entities;

namespace RailBook.External.Api.Endpoints
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainServicesController : ControllerBase
    {
        private readonly TrainServiceService _trainServiceService;
        private readonly IMapper _mapper;

        public TrainServicesController(TrainServiceService service, IMapper mapper)
        {
            _trainServiceService = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrainServiceDto>>> GetAll()
        {
            var services = await _trainServiceService.GetAllTrainsAsync();
            return Ok(_mapper.Map<List<TrainServiceDto>>(services));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainServiceDto>> GetById(int id)
        {
            var serviceEntity = await _trainServiceService.GetTrainByIdAsync(id);
            if (serviceEntity == null) return NotFound();
            return Ok(_mapper.Map<TrainServiceDto>(serviceEntity));
        }

        [HttpPost]
        public async Task<ActionResult<CreateTrainServiceDto>> Create([FromBody] CreateTrainServiceDto dto)
        {
            var serviceEntity = _mapper.Map<TrainService>(dto);
            await _trainServiceService.AddTrainAsync(serviceEntity);
            return CreatedAtAction(nameof(GetById), new { id = serviceEntity.ServiceId }, _mapper.Map<TrainServiceDto>(serviceEntity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTrainServiceDto dto)
        {
            var serviceEntity = await _trainServiceService.GetTrainByIdAsync(id);
            if (serviceEntity == null) return NotFound();

            _mapper.Map(dto, serviceEntity);
            await _trainServiceService.UpdateTrainAsync(serviceEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainServiceService.DeleteTrainAsync(id);
            return NoContent();
        }
    }
}
