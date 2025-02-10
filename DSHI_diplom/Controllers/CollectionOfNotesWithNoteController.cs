using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/collectionOfNotesWithNote")]
    [ApiController]
    public class CollectionOfNotesWithNoteController : ControllerBase
    {
        private readonly ICollectionOfNotesWithNoteService _service;

        public CollectionOfNotesWithNoteController(ICollectionOfNotesWithNoteService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<CollectionOfNotesWithNote>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CollectionOfNotesWithNote>> GetById(int id)
        {
            var collectionOfNotesWithNote = await _service.GetByIdAsync(id);
            if (collectionOfNotesWithNote == null) return NotFound();
            return Ok(collectionOfNotesWithNote);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CollectionOfNotesWithNote collectionOfNotesWithNote)
        {
            await _service.AddAsync(collectionOfNotesWithNote);
            return CreatedAtAction(nameof(GetById), new { id = collectionOfNotesWithNote.Id }, collectionOfNotesWithNote);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, CollectionOfNotesWithNote collectionOfNotesWithNote)
        {
            if (id != collectionOfNotesWithNote.Id) return BadRequest();
            await _service.UpdateAsync(collectionOfNotesWithNote);
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
