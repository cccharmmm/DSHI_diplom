using DSHI_diplom.Model;
using DSHI_diplom.Services.Implementations;
using DSHI_diplom.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSHI_diplom.Controllers
{
    [Route("api/audioFile")]
    [ApiController]
    public class AudioFileController : ControllerBase
    {
        private readonly IAudioFileService _service;

        public AudioFileController(IAudioFileService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<AudioFile>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AudioFile>> GetById(int id)
        {
            var audioFile = await _service.GetByIdAsync(id);
            if (audioFile == null) return NotFound();
            return Ok(audioFile);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AudioFile audioFile)
        {
            await _service.AddAsync(audioFile);
            return CreatedAtAction(nameof(GetById), new { id = audioFile.Id }, audioFile);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, AudioFile audioFile)
        {
            if (id != audioFile.Id) return BadRequest();
            await _service.UpdateAsync(audioFile);
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
