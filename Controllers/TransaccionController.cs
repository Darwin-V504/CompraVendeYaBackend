using CompraVendeYaBackend.Models;
using CompraVendeYaBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CompraVendeYaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly TransaccionService _transaccionService;

        public TransaccionController(TransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaccion>>> ObtenerTransacciones()
        {
            var transacciones = await _transaccionService.ObtenerTransacciones();
            return Ok(transacciones);
        }

        [HttpPost]
        public async Task<ActionResult<Transaccion>> CrearTransaccion(Transaccion transaccion)
        {
            var nuevaTransaccion = await _transaccionService.CrearTransaccion(transaccion);
            return CreatedAtAction(nameof(ObtenerTransacciones), nuevaTransaccion);
        }
    }
}