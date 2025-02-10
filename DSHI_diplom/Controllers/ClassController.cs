using DSHI_diplom.Components.Pages;
using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;
namespace DSHI_diplom.Controllers
{
    [Route("api/class")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _service;

        public ClassController(IClassService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<Class>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetById(int id)
        {
            var _class = await _service.GetByIdAsync(id);
            if (_class == null) return NotFound();
            return Ok(_class);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Class _class)
        {
            await _service.AddAsync(_class);
            return CreatedAtAction(nameof(GetById), new { id = _class.Id }, _class);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Class _class)
        {
            if (id != _class.Id) return BadRequest();
            await _service.UpdateAsync(_class);
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
