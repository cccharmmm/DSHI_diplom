using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/collectionOfThMlWithThMController")]
    [ApiController]
    public class CollectionOfThMlWithThMController : ControllerBase
    {
        private readonly ICollectionOfThMlWithThService _service;

        public CollectionOfThMlWithThMController(ICollectionOfThMlWithThService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<CollectionOfThMlWithThM>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionOfThMlWithThM>> GetById(int id)
        {
            var collectionOfThMlWithThM = await _service.GetByIdAsync(id);
            if (collectionOfThMlWithThM == null) return NotFound();
            return Ok(collectionOfThMlWithThM);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CollectionOfThMlWithThM collectionOfThMlWithThM)
        {
            await _service.AddAsync(collectionOfThMlWithThM);
            return CreatedAtAction(nameof(GetById), new { id = collectionOfThMlWithThM.Id }, collectionOfThMlWithThM);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CollectionOfThMlWithThM collectionOfThMlWithThM)
        {
            if (id != collectionOfThMlWithThM.Id) return BadRequest();
            await _service.UpdateAsync(collectionOfThMlWithThM);
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
