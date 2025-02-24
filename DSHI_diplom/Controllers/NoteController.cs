using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using DSHI_diplom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DSHI_diplom.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;
        private readonly DiplomContext _context;

        public NoteController(INoteService service, DiplomContext context)
        {
            _service = service;
            _context = context;
        }
        [HttpGet("filter")]
        public async Task<ActionResult<List<Note>>> GetFilteredNotes(
        [FromQuery] string instrument = null,
        [FromQuery] string composer = null,
        [FromQuery] string class_ = null,
        [FromQuery] string musicalForm = null)
        {
            var notes = await _service.GetFilteredNotesAsync(instrument, composer, class_, musicalForm);
            return Ok(notes);
        }
        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<Note>>> GetFilteredNotesBySearch(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return BadRequest("Search text cannot be empty.");
            }

            var notes = await _service.GetFilteredNotesBySearchAsync(searchText);
            if (notes == null || notes.Count == 0)
            {
                return NotFound("No notes found matching the search criteria.");
            }

            return Ok(notes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetById(int id)
        {
            var note = await _service.GetByIdAsync(id);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Note note)
        {
            await _service.AddAsync(note);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Note note)
        {
            if (id != note.Id) return BadRequest();
            await _service.UpdateAsync(note);
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
