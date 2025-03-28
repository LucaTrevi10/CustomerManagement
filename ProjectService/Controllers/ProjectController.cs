using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectService.Data;
using ProjectService.Models;
using System.Net.Http;


namespace ProjectService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectDbContext _context;
        private readonly HttpClient _httpClient;

        public ProjectController(ProjectDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                var projects = await _context.Projects
                    .Select(p => new ProjectDto
                    {
                        ProjectId = p.ProjectId,
                        ProjectName = p.ProjectName,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        PaymentDueDate = p.PaymentDueDate,
                        CustomerId = p.CustomerId,
                        IsComplete = p.IsComplete,
                        CompanyName = null // Lo valorizziamo dopo
                    })
                    .ToListAsync();

                // Creo una mappa per evitare chiamate duplicate
                var customerIds = projects.Select(p => p.CustomerId).Distinct();
                var customerTasks = customerIds.ToDictionary(
                    customerId => customerId,
                    customerId => _httpClient.GetFromJsonAsync<CustomerDto>($"http://localhost:5089/api/customer/{customerId}")
                );

                // Attendiamo il completamento di tutte le richieste HTTP
                await Task.WhenAll(customerTasks.Values);

                // Assegniamo la CompanyName ai progetti
                foreach (var project in projects)
                {
                    if (customerTasks.TryGetValue(project.CustomerId, out var task) && task.Result != null)
                    {
                        project.CompanyName = task.Result.CompanyName;
                    }
                }

                return Ok(projects);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante il recupero dei progetti: {ex.Message}");
                return BadRequest();
            }
        }


        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var project = await _context.Projects
                .Where(p => p.ProjectId == id)
                .Select(p => new ProjectDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    PaymentDueDate = p.PaymentDueDate,
                    CustomerId = p.CustomerId,
                    IsComplete = p.IsComplete
                })
                .FirstOrDefaultAsync();

            if (project == null)
            {
                return NotFound();
            }

            try
            {
                // Chiamata HTTP al CustomerService per ottenere il CompanyName
                var customerResponse = await _httpClient.GetFromJsonAsync<CustomerDto>($"http://localhost:5089/api/customer/{project.CustomerId}");

                if (customerResponse != null)
                {
                    project.CompanyName = customerResponse.CompanyName;
                }
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine($"Errore chiamata CustomerService: {ex.Message}");
            }

            return Ok(project);
        }

        // POST: api/Project
        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { message = "Dati non validi", errors });
            }

            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Errore nel salvataggio", error = ex.Message });
            }
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            var existingProject = await _context.Projects.FindAsync(id);
            if (existingProject == null)
            {
                return NotFound();
            }

            existingProject.ProjectName = project.ProjectName;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.PaymentDueDate = project.PaymentDueDate;
            existingProject.CustomerId = project.CustomerId;
            existingProject.IsComplete = project.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projects.Any(p => p.ProjectId == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/projects/{projectId}/payments
        //[HttpGet("{projectId}/payments")]
        //public async Task<IActionResult> GetPaymentsByProject(int projectId)
        //{
        //    var payments = await _context.Payments
        //        .Where(p => p.ProjectId == projectId)
        //        .ToListAsync();

        //    return Ok(payments);
        //}
    }
}
