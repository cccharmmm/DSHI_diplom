using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Test>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetById(int id)
        {
            var test = await _service.GetByIdAsync(id);
            if (test == null) return NotFound();
            return Ok(test);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Test test)
        {
            await _service.AddAsync(test);
            return CreatedAtAction(nameof(GetById), new { id = test.Id }, test);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Test test)
        {
            if (id != test.Id) return BadRequest();
            await _service.UpdateAsync(test);
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
