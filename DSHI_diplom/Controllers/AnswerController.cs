using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/answer")]
    [ApiController]
    public class AnswerController : ControllerBase
    {

        private readonly IAnswerService _service;

        public AnswerController(IAnswerService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Answer>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetById(int id)
        {
            var answer = await _service.GetByIdAsync(id);
            if (answer == null) return NotFound();
            return Ok(answer);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Answer answer)
        {
            await _service.AddAsync(answer);
            return CreatedAtAction(nameof(GetById), new { id = answer.Id }, answer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Answer answer)
        {
            if (id != answer.Id) return BadRequest();
            await _service.UpdateAsync(answer);
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
