using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;

        public NoteController(INoteService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Note>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
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
