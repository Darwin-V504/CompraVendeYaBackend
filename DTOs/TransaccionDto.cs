namespace CompraVendeYaBackend.DTOs;

public class TransaccionDto
{
    public int IdTransaccion { get; set; }
    public int? IdPropiedad { get; set; }
    public string? PropiedadTitulo { get; set; }
    public int? IdCliente { get; set; }
    public string? ClienteNombre { get; set; }
    public int? IdAgente { get; set; }
    public string? AgenteNombre { get; set; }
    public int? IdOperacion { get; set; }
    public string? OperacionNombre { get; set; }
    public DateOnly FechaTransaccion { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal? ComisionAgente { get; set; }
    public string? EstadoTransaccion { get; set; }
    public string? Detalles { get; set; }
}

public class CreateTransaccionDto
{
    public int? IdPropiedad { get; set; }
    public int? IdCliente { get; set; }
    public int? IdAgente { get; set; }
    public int? IdOperacion { get; set; }
    public DateOnly FechaTransaccion { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal? ComisionAgente { get; set; }
    public string? EstadoTransaccion { get; set; }
    public string? Detalles { get; set; }
}