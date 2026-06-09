using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Data
{
    public class CompraVendeYaDbContext : DbContext
    {
        public CompraVendeYaDbContext(DbContextOptions<CompraVendeYaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaccion> Transacciones { get; set; }
    }
}