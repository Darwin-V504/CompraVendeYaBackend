namespace CompraVendeYaBackend.Models;

public class Propiedad
{
    public int      IdPropiedad        { get; set; }
    public int?     IdTipo             { get; set; }
    public int?     IdZona             { get; set; }
    public int?     IdPropietario      { get; set; }
    public int?     IdAgente           { get; set; }
    public int?     IdEstado           { get; set; }
    public int?     IdOperacion        { get; set; }
    public string   Titulo             { get; set; } = string.Empty;
    public string?  Descripcion        { get; set; }
    public decimal  Precio             { get; set; }
    public string   Direccion          { get; set; } = string.Empty;
    public decimal? AreaConstruida     { get; set; }
    public decimal? AreaTerreno        { get; set; }
    public int?     Habitaciones       { get; set; }
    public int?     Banos              { get; set; }
    public int?     Garajes            { get; set; }
    public int?     AnoConstruccion    { get; set; }
    public string?  Coordenadas        { get; set; }
    public DateOnly FechaPublicacion   { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public TipoPropiedad?  TipoPropiedad { get; set; }
    public Zona?           Zona          { get; set; }
    public Propietario?    Propietario   { get; set; }
    public Usuario?        Agente        { get; set; }
    public EstadoPropiedad? Estado       { get; set; }
    public Operacion?      Operacion     { get; set; }

    public ICollection<FotoPropiedad> Fotos        { get; set; } = new List<FotoPropiedad>();
    public ICollection<Transaccion>   Transacciones { get; set; } = new List<Transaccion>();
}