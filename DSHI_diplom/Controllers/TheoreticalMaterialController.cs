using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/theoreticalMaterial")]
    [ApiController]
    public class TheoreticalMaterialController : ControllerBase
    {
        private readonly ITheoreticalMaterialService _service;

        public TheoreticalMaterialController(ITheoreticalMaterialService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<TheoreticalMaterial>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TheoreticalMaterial>> GetById(int id)
        {
            var theoreticalMaterial = await _service.GetByIdAsync(id);
            if (theoreticalMaterial == null) return NotFound();
            return Ok(theoreticalMaterial);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TheoreticalMaterial theoreticalMaterial)
        {
            await _service.AddAsync(theoreticalMaterial);
            return CreatedAtAction(nameof(GetById), new { id = theoreticalMaterial.Id }, theoreticalMaterial);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TheoreticalMaterial theoreticalMaterial)
        {
            if (id != theoreticalMaterial.Id) return BadRequest();
            await _service.UpdateAsync(theoreticalMaterial);
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
