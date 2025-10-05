using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RailBook.Domain.Dtos.InvoiceDetails;
using RailBook.Manager.Implementations;

namespace RailBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDetailsController : ControllerBase
    {
        private readonly InvoiceDetailsService _service;
        private readonly IMapper _mapper;

        public InvoiceDetailsController(InvoiceDetailsService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDetailsDto>>> GetAll()
        {
            var details = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<InvoiceDetailsDto>>(details));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDetailsDto>> GetById(int id)
        {
            var detail = await _service.GetByIdAsync(id);
            if (detail == null) return NotFound();
            return Ok(_mapper.Map<InvoiceDetailsDto>(detail));
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDetailsDto>> Create([FromBody] CreateInvoiceDetailsDto dto)
        {
            var detail = _mapper.Map<InvoiceDetails>(dto);
            await _service.AddAsync(detail);
            return CreatedAtAction(nameof(GetById), new { id = detail.Id }, _mapper.Map<InvoiceDetailsDto>(detail));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceDetailsDto dto)
        {
            var detail = await _service.GetByIdAsync(id);
            if (detail == null) return NotFound();

            _mapper.Map(dto, detail);
            await _service.UpdateAsync(detail);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
