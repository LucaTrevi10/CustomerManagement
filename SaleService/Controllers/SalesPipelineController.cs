using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleService.Data;
using SaleService.Models;
using SaleService.Models.Dtos;

namespace SaleService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesPipelineController : Controller
    {
        private readonly SalesDbContext _context;

        public SalesPipelineController(SalesDbContext context)
        {
            _context = context;
        }

        // GET: api/SalesPipeline
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesPipeline>>> GetSalesPipelines()
        {
            try
            {
                var pipelines = await _context.SalesPipeline.ToListAsync();
                return Ok(pipelines);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/SalesPipeline/{id}/notes
        [HttpPut("{id}/notes")]
        public async Task<IActionResult> UpdateSalesPipelineNoteAsync(int id, [FromBody] UpdateNoteDto dto)
        {                      
            // Trova il record per l'ID indicato
            var pipelineRecord = await _context.SalesPipeline.FindAsync(id);
            if (pipelineRecord == null)
            {
                return NotFound();
            }

            // Aggiorna il campo Notes e la data di aggiornamento
            pipelineRecord.Notes = dto.Note;
            pipelineRecord.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }

}

