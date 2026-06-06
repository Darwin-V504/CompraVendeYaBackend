using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Data;

public class BienesRaicesDbContext : DbContext
{
    public BienesRaicesDbContext(DbContextOptions<BienesRaicesDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<Propiedad> Propiedades { get; set; }
    public DbSet<TipoPropiedad> TiposPropiedad { get; set; }
    public DbSet<Zona> Zonas { get; set; }
    public DbSet<EstadoPropiedad> EstadosPropiedad { get; set; }
    public DbSet<Operacion> Operaciones { get; set; }
    public DbSet<FotoPropiedad> FotosPropiedad { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar nombres de tablas en minúsculas
        modelBuilder.Entity<Usuario>().ToTable("usuario");
        modelBuilder.Entity<Cliente>().ToTable("cliente");
        modelBuilder.Entity<Propietario>().ToTable("propietario");
        modelBuilder.Entity<Propiedad>().ToTable("propiedad");
        modelBuilder.Entity<TipoPropiedad>().ToTable("tipopropiedad");
        modelBuilder.Entity<Zona>().ToTable("zona");
        modelBuilder.Entity<EstadoPropiedad>().ToTable("estadopropiedad");
        modelBuilder.Entity<Operacion>().ToTable("operacion");
        modelBuilder.Entity<FotoPropiedad>().ToTable("fotopropiedad");
        modelBuilder.Entity<Transaccion>().ToTable("transaccion");
        modelBuilder.Entity<RefreshToken>().ToTable("refreshtoken");

        // Configurar relaciones
        modelBuilder.Entity<Propiedad>()
            .HasOne(p => p.TipoPropiedad)
            .WithMany(t => t.Propiedades)
            .HasForeignKey(p => p.IdTipo);

        modelBuilder.Entity<Propiedad>()
            .HasOne(p => p.Zona)
            .WithMany(z => z.Propiedades)
            .HasForeignKey(p => p.IdZona);

        modelBuilder.Entity<Propiedad>()
            .HasOne(p => p.Propietario)
            .WithMany(pr => pr.Propiedades)
            .HasForeignKey(p => p.IdPropietario);

        modelBuilder.Entity<Propiedad>()
            .HasOne(p => p.Estado)
            .WithMany(e => e.Propiedades)
            .HasForeignKey(p => p.IdEstado);

        modelBuilder.Entity<Propiedad>()
            .HasOne(p => p.Operacion)
            .WithMany(o => o.Propiedades)
            .HasForeignKey(p => p.IdOperacion);

        modelBuilder.Entity<FotoPropiedad>()
            .HasOne(f => f.Propiedad)
            .WithMany(p => p.Fotos)
            .HasForeignKey(f => f.IdPropiedad);

        modelBuilder.Entity<Transaccion>()
            .HasOne(t => t.Propiedad)
            .WithMany(p => p.Transacciones)
            .HasForeignKey(t => t.IdPropiedad);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UsuarioId);
    }
}  