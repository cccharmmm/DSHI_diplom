using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/collectionOfTheoreticalMaterial")]
    [ApiController]
    public class CollectionOfTheoreticalMaterialController : ControllerBase
    {
        private readonly ICollectionOfTheoreticalMaterialService _service;

        public CollectionOfTheoreticalMaterialController(ICollectionOfTheoreticalMaterialService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<CollectionOfTheoreticalMaterial>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionOfTheoreticalMaterial>> GetById(int id)
        {
            var collectionOfTheoreticalMaterial = await _service.GetByIdAsync(id);
            if (collectionOfTheoreticalMaterial == null) return NotFound();
            return Ok(collectionOfTheoreticalMaterial);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CollectionOfTheoreticalMaterial collectionOfTheoreticalMaterial)
        {
            await _service.AddAsync(collectionOfTheoreticalMaterial);
            return CreatedAtAction(nameof(GetById), new { id = collectionOfTheoreticalMaterial.Id }, collectionOfTheoreticalMaterial);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CollectionOfTheoreticalMaterial collectionOfTheoreticalMaterial)
        {
            if (id != collectionOfTheoreticalMaterial.Id) return BadRequest();
            await _service.UpdateAsync(collectionOfTheoreticalMaterial);
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
