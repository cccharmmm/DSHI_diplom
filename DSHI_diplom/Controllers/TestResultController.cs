using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/testResult")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        private readonly ITestResultService _service;

        public TestResultController(ITestResultService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<TestResult>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestResult>> GetById(int id)
        {
            var testResult = await _service.GetByIdAsync(id);
            if (testResult == null) return NotFound();
            return Ok(testResult);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TestResult testResult)
        {
            await _service.AddAsync(testResult);
            return CreatedAtAction(nameof(GetById), new { id = testResult.Id }, testResult);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, TestResult testResult)
        {
            if (id != testResult.Id) return BadRequest();
            await _service.UpdateAsync(testResult);
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
