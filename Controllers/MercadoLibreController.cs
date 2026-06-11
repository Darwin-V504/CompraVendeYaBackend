using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MercadoLibreController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MercadoLibreController> _logger;

    public MercadoLibreController(ILogger<MercadoLibreController> logger)
    {
        // ✅ Configuración CORRECTA para Mercado Libre
        var handler = new HttpClientHandler
        {
            AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
        };

        _httpClient = new HttpClient(handler);
        _httpClient.BaseAddress = new Uri("https://api.mercadolibre.com/");

        // ✅ IMPORTANTE: User-Agent OBLIGATORIO
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "es-MX,es;q=0.9");

        _logger = logger;
    }

    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchProperties(
        [FromQuery] string siteId = "MLM",
        [FromQuery] string? query = null,
        [FromQuery] int limit = 20)
    {
        try
        {
            // Construir URL de búsqueda
            var searchUrl = $"/sites/{siteId}/search?limit={Math.Min(limit, 50)}";
            if (!string.IsNullOrEmpty(query))
                searchUrl += $"&q={Uri.EscapeDataString(query)}";

            _logger.LogInformation($"Consultando ML: {searchUrl}");

            var response = await _httpClient.GetAsync(searchUrl);

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"ML API error: {response.StatusCode}");
                _logger.LogWarning($"Respuesta: {jsonResponse}");

                return StatusCode((int)response.StatusCode, new
                {
                    error = $"Error al consultar Mercado Libre: {response.StatusCode}",
                    source = "mercadolibre",
                    total = 0,
                    results = new List<object>()
                });
            }

            var mlData = JsonSerializer.Deserialize<MercadoLibreSearchResponse>(jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var propiedadesMapeadas = MapToPropiedades(mlData);

            return Ok(new
            {
                source = "mercadolibre",
                total = mlData?.Paging?.Total ?? 0,
                results = propiedadesMapeadas
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en búsqueda de ML");
            return StatusCode(500, new { error = $"Error interno: {ex.Message}" });
        }
    }

    private List<PropiedadMercadoLibre> MapToPropiedades(MercadoLibreSearchResponse? mlData)
    {
        var resultado = new List<PropiedadMercadoLibre>();
        if (mlData?.Results == null) return resultado;

        foreach (var item in mlData.Results)
        {
            resultado.Add(new PropiedadMercadoLibre
            {
                IdExterno = item.Id ?? "",
                Titulo = item.Title ?? "Sin título",
                Precio = item.Price ?? 0,
                Moneda = item.CurrencyId ?? "MXN",
                Direccion = item.Address?.CityName ??
                           item.SellerAddress?.City?.Name ??
                           "Ubicación no especificada",
                Descripcion = item.Subtitle ?? "",
                UrlImagen = item.Thumbnail ?? "",
                UrlDetalle = item.Permalink ?? "",
                Condicion = item.Condition ?? "unknown",
                TipoOperacion = item.BuyingMode == "buy_it_now" ? "Venta" : "No especificado"
            });
        }
        return resultado;
    }
}

// Clases para deserializar
public class MercadoLibreSearchResponse
{
    public List<MlResult>? Results { get; set; }
    public MlPaging? Paging { get; set; }
}

public class MlResult
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public decimal? Price { get; set; }
    [JsonPropertyName("currency_id")] public string? CurrencyId { get; set; }
    public string? Thumbnail { get; set; }
    public string? Permalink { get; set; }
    public string? Condition { get; set; }
    [JsonPropertyName("buying_mode")] public string? BuyingMode { get; set; }
    public MlAddress? Address { get; set; }
    [JsonPropertyName("seller_address")] public MlSellerAddress? SellerAddress { get; set; }
}

public class MlAddress
{
    [JsonPropertyName("city_name")] public string? CityName { get; set; }
}

public class MlSellerAddress
{
    public MlCity? City { get; set; }
}

public class MlCity
{
    public string? Name { get; set; }
}

public class MlPaging
{
    public int Total { get; set; }
}

public class PropiedadMercadoLibre
{
    public string? IdExterno { get; set; }
    public string? Titulo { get; set; }
    public decimal Precio { get; set; }
    public string? Moneda { get; set; }
    public string? Direccion { get; set; }
    public string? Descripcion { get; set; }
    public string? UrlImagen { get; set; }
    public string? UrlDetalle { get; set; }
    public string? Condicion { get; set; }
    public string? TipoOperacion { get; set; }
}