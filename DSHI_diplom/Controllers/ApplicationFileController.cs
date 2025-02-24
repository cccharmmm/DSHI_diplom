using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Controllers
{
    [Route("api/applicationFile")]
    [ApiController]
    public class ApplicationFileController : ControllerBase
    {
        private readonly IApplicationFileService _service;

        public ApplicationFileController(IApplicationFileService service)
        {
            _service = service;
        }
        [Route("api/files")]
        public class FilesController : ControllerBase
        {
            [HttpGet("{fileName}")]
            public IActionResult GetFile(string fileName)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationFile>> GetById(int id)
        {
            var applicationFile = await _service.GetByIdAsync(id);
            if (applicationFile == null) return NotFound();
            return Ok(applicationFile);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ApplicationFile applicationFile)
        {
            await _service.AddAsync(applicationFile);
            return CreatedAtAction(nameof(GetById), new { id = applicationFile.Id }, applicationFile);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ApplicationFile applicationFile)
        {
            if (id != applicationFile.Id) return BadRequest();
            await _service.UpdateAsync(applicationFile);
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
