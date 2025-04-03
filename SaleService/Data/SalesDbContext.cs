using Microsoft.EntityFrameworkCore;
using SaleService.Models;

namespace SaleService.Data
{
    public class SalesDbContext : DbContext
    {
        public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
        {
        }

        public DbSet<SalesPipeline> SalesPipeline { get; set; }
    }
}
