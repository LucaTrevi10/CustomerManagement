using CustomerService.Data;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JourneyController : Controller
    {
        private readonly CustomerServiceDbContext _context;

        public JourneyController(CustomerServiceDbContext context)
        {
            _context = context;
        }

        // GET: api/Journey/Customer/{customerId}
        [HttpGet("Customer/{customerId}")]
        public async Task<ActionResult<List<JourneyStepDto>>> GetCustomerJourney(int customerId)
        {
            var journeySteps = await _context.JourneySteps
                .Where(j => j.CustomerId == customerId)
                .OrderBy(j => j.StepOrder)
                .Select(j => new JourneyStepDto
                {
                    JourneyStepId = j.JourneyStepId,
                    StepName = j.StepName,
                    StepDate = j.StepDate,
                    StepOrder = j.StepOrder
                })
                .ToListAsync();

            if (journeySteps == null || journeySteps.Count == 0)
            {
                return NotFound($"Nessun step trovato per il customer {customerId}");
            }
            return Ok(journeySteps);
        }

        // POST: api/Journey/Customer/{customerId}
        [HttpPost("Customer/{customerId}")]
        public async Task<ActionResult<JourneyStep>> AddJourneyStep(int customerId, JourneyStepDto journeyStepDto)
        {
            // Verifica che il customer esista
            var customer = await _context.Customer.FindAsync(customerId);
            if (customer == null)
            {
                return NotFound("Customer non trovato.");
            }

            var newStep = new JourneyStep
            {
                CustomerId = customerId,
                StepName = journeyStepDto.StepName,
                StepDate = journeyStepDto.StepDate,
                StepOrder = journeyStepDto.StepOrder
            };

            _context.JourneySteps.Add(newStep);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerJourney), new { customerId = customerId }, newStep);
        }

        // PUT: api/Journey/{journeyStepId}
        [HttpPut("{journeyStepId}")]
        public async Task<IActionResult> UpdateJourneyStep(int journeyStepId, JourneyStepDto journeyStepDto)
        {
            var journeyStep = await _context.JourneySteps.FindAsync(journeyStepId);
            if (journeyStep == null)
            {
                return NotFound("Step non trovato.");
            }

            journeyStep.StepName = journeyStepDto.StepName;
            journeyStep.StepDate = journeyStepDto.StepDate;
            journeyStep.StepOrder = journeyStepDto.StepOrder;

            _context.Entry(journeyStep).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Journey/{journeyStepId}
        [HttpDelete("{journeyStepId}")]
        public async Task<IActionResult> DeleteJourneyStep(int journeyStepId)
        {
            var journeyStep = await _context.JourneySteps.FindAsync(journeyStepId);
            if (journeyStep == null)
            {
                return NotFound("Step non trovato.");
            }

            _context.JourneySteps.Remove(journeyStep);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
