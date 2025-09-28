using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RailBook.Core.Application.Dtos.Invoice;
using RailBook.Core.Application.Services;
using RailBook.Core.Domain.Entities;

namespace RailBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceService _service;
        private readonly IMapper _mapper;

        public InvoicesController(InvoiceService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDto>>> GetAll()
        {
            var invoices = await _service.GetAllInvoicesAsync();
            return Ok(_mapper.Map<List<InvoiceDto>>(invoices));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetById(int id)
        {
            var invoice = await _service.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound();
            return Ok(_mapper.Map<InvoiceDto>(invoice));
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> Create([FromBody] CreateInvoiceDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);
            await _service.AddInvoiceAsync(invoice);
            return CreatedAtAction(nameof(GetById), new { id = invoice.InvoiceId }, _mapper.Map<InvoiceDto>(invoice));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInvoiceDto dto)
        {
            var invoice = await _service.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound();

            _mapper.Map(dto, invoice);
            await _service.UpdateInvoiceAsync(invoice);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteInvoiceAsync(id);
            return NoContent();
        }
    }
}
