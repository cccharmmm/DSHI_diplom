using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/musicalForm")]
    [ApiController]
    public class MusicalFormController : ControllerBase
    {
        private readonly IMusicalFormService _service;

        public MusicalFormController(IMusicalFormService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<MusicalForm>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicalForm>> GetById(int id)
        {
            var musicalForm = await _service.GetByIdAsync(id);
            if (musicalForm == null) return NotFound();
            return Ok(musicalForm);
        }

        [HttpPost]
        public async Task<ActionResult> Create(MusicalForm musicalForm)
        {
            await _service.AddAsync(musicalForm);
            return CreatedAtAction(nameof(GetById), new { id = musicalForm.Id }, musicalForm);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MusicalForm musicalForm)
        {
            if (id != musicalForm.Id) return BadRequest();
            await _service.UpdateAsync(musicalForm);
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
