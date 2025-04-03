using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleService.Data;
using SaleService.Models;

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
    }
}
