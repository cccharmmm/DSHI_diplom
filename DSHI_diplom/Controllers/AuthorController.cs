using DSHI_diplom.Components.Pages;
using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using DSHI_diplom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSHI_diplom.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetById(int id)
        {
            var author  = await _service.GetByIdAsync(id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Author author)
        {
            await _service.AddAsync(author);
            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Author author)
        {
            if (id != author.Id) return BadRequest();
            await _service.UpdateAsync(author);
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
