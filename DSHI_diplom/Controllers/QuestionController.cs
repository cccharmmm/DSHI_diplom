using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/question")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Question>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetById(int id)
        {
            var question = await _service.GetByIdAsync(id);
            if (question == null) return NotFound();
            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Question question)
        {
            await _service.AddAsync(question);
            return CreatedAtAction(nameof(GetById), new { id = question.Id }, question);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Question question)
        {
            if (id != question.Id) return BadRequest();
            await _service.UpdateAsync(question);
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
