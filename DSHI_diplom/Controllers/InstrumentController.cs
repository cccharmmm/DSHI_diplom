using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/instrument")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly IInstrumentService _service;

        public InstrumentController(IInstrumentService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Instrument>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instrument>> GetById(int id)
        {
            var instrument = await _service.GetByIdAsync(id);
            if (instrument == null) return NotFound();
            return Ok(instrument);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Instrument instrument)
        {
            await _service.AddAsync(instrument);
            return CreatedAtAction(nameof(GetById), new { id = instrument.Id }, instrument);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Instrument instrument)
        {
            if (id != instrument.Id) return BadRequest();
            await _service.UpdateAsync(instrument);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
