using System.Text;
using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddDbContext<BienesRaicesDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios
builder.Services.AddHttpClient();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PropiedadService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PropietarioService>();
builder.Services.AddScoped<TransaccionService>();
builder.Services.AddScoped<ZonaService>();

// Configurar JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "CompraVendeYa123";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CompraVendeYa API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresa el token JWT: Bearer {tu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Endpoint temporal para probar la conexión a BD
app.MapGet("/api/test-database", async (BienesRaicesDbContext db) =>
{
    try
    {
        var canConnect = await db.Database.CanConnectAsync();
        var tipoCount = await db.TiposPropiedad.CountAsync();

        return Results.Ok(new
        {
            success = true,
            connected = canConnect,
            message = "✅ Conexión exitosa a la base de datos",
            tables = new
            {
                tipoPropiedad = tipoCount,
                message = "Datos cargados correctamente"
            }
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            success = false,
            error = ex.Message,
            innerError = ex.InnerException?.Message
        });
    }
});

app.Run();