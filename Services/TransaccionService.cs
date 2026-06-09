using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services
{
    public class TransaccionService
    {
        private readonly CompraVendeYaDbContext _context;

        public TransaccionService(CompraVendeYaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaccion>> ObtenerTransacciones()
        {
            return await _context.Transacciones.ToListAsync();
        }

        public async Task<Transaccion> CrearTransaccion(Transaccion transaccion)
        {
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return transaccion;
        }
    }
}