using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/composer")]
    [ApiController]
    public class ComposerController : ControllerBase
    {
        private readonly IComposerService _service;

        public ComposerController(IComposerService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Composer>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Composer>> GetById(int id)
        {
            var сomposer = await _service.GetByIdAsync(id);
            if (сomposer == null) return NotFound();
            return Ok(сomposer);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Composer сomposer)
        {
            await _service.AddAsync(сomposer);
            return CreatedAtAction(nameof(GetById), new { id = сomposer.Id }, сomposer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Composer сomposer)
        {
            if (id != сomposer.Id) return BadRequest();
            await _service.UpdateAsync(сomposer);
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
