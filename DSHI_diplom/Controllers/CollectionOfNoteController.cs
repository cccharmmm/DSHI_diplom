using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/collectionOfNote")]
    [ApiController]
    public class CollectionOfNoteController : ControllerBase
    {
        private readonly ICollectionOfNoteService _service;

        public CollectionOfNoteController(ICollectionOfNoteService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<CollectionOfNote>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionOfNote>> GetById(int id)
        {
            var collectionOfNote = await _service.GetByIdAsync(id);
            if (collectionOfNote == null) return NotFound();
            return Ok(collectionOfNote);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CollectionOfNote collectionOfNote)
        {
            await _service.AddAsync(collectionOfNote);
            return CreatedAtAction(nameof(GetById), new { id = collectionOfNote.Id }, collectionOfNote);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CollectionOfNote collectionOfNote)
        {
            if (id != collectionOfNote.Id) return BadRequest();
            await _service.UpdateAsync(collectionOfNote);
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
