using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {

        private readonly ISubjectService _service;

        public SubjectController(ISubjectService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Subject>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetById(int id)
        {
            var subject = await _service.GetByIdAsync(id);
            if (subject == null) return NotFound();
            return Ok(subject);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Subject subject)
        {
            await _service.AddAsync(subject);
            return CreatedAtAction(nameof(GetById), new { id = subject.Id }, subject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Subject subject)
        {
            if (id != subject.Id) return BadRequest();
            await _service.UpdateAsync(subject);
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
