using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MercadoLibreController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MercadoLibreController> _logger;

    public MercadoLibreController(IHttpClientFactory httpClientFactory, ILogger<MercadoLibreController> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://api.mercadolibre.com/");
        _logger = logger;
    }

    /// <summary>
    /// Busca propiedades en Mercado Libre
    /// </summary>
    /// <param name="siteId">País: MLM (México), MLA (Argentina), MLB (Brasil), etc.</param>
    /// <param name="query">Término de búsqueda</param>
    /// <param name="categoryId">Categoría de inmuebles (ej: MLM1459 para México)</param>
    /// <param name="limit">Cantidad de resultados (máx 50)</param>
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchProperties(
        [FromQuery] string siteId = "MLM",
        [FromQuery] string? query = null,
        [FromQuery] string? categoryId = null,
        [FromQuery] int limit = 20)
    {
        try
        {
            // Construir la URL de búsqueda
            var searchUrl = $"/sites/{siteId}/search?limit={Math.Min(limit, 50)}";

            if (!string.IsNullOrEmpty(query))
                searchUrl += $"&q={Uri.EscapeDataString(query)}";

            if (!string.IsNullOrEmpty(categoryId))
                searchUrl += $"&category={categoryId}";

            _logger.LogInformation($"Consultando ML: {searchUrl}");

            var response = await _httpClient.GetAsync(searchUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning($"ML API error: {response.StatusCode}");
                return StatusCode((int)response.StatusCode, new { error = "Error al consultar Mercado Libre" });
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var mlData = JsonSerializer.Deserialize<MercadoLibreSearchResponse>(jsonResponse);

            // Transformar al formato que tu frontend ya entiende
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
            return StatusCode(500, new { error = "Error interno al consultar Mercado Libre" });
        }
    }

    /// <summary>
    /// Obtiene las categorías de inmuebles disponibles por país
    /// </summary>
    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCategories([FromQuery] string siteId = "MLM")
    {
        try
        {
            var response = await _httpClient.GetAsync($"/sites/{siteId}/categories");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var categories = JsonSerializer.Deserialize<List<MlCategory>>(jsonResponse);

            // Filtrar solo la categoría de inmuebles
            var realEstateCategory = categories?.FirstOrDefault(c =>
                c.Name?.Contains("Inmuebles", StringComparison.OrdinalIgnoreCase) == true ||
                c.Name?.Contains("Imóveis", StringComparison.OrdinalIgnoreCase) == true);

            if (realEstateCategory != null)
            {
                // Obtener subcategorías (tipos de propiedad)
                var subResponse = await _httpClient.GetAsync($"/categories/{realEstateCategory.Id}");
                var subJson = await subResponse.Content.ReadAsStringAsync();
                var categoryDetail = JsonSerializer.Deserialize<MlCategoryDetail>(subJson);

                return Ok(new
                {
                    mainCategory = realEstateCategory,
                    propertyTypes = categoryDetail?.ChildrenCategories
                });
            }

            return Ok(categories?.Where(c => c.Name?.Contains("Inmuebles") == true));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo categorías");
            return StatusCode(500, new { error = "Error al obtener categorías" });
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
                IdExterno = item.Id,
                Titulo = item.Title ?? "Sin título",
                Precio = item.Price ?? 0,
                Moneda = item.CurrencyId ?? "MXN",
                Direccion = item.Address?.CityName ?? item.SellerAddress?.City?.Name ?? "Ubicación no especificada",
                Descripcion = item.Subtitle,
                UrlImagen = item.Thumbnail,
                UrlDetalle = item.Permalink,
                Condicion = item.Condition,
                TipoOperacion = item.BuyingMode == "buy_it_now" ? "Venta" : "No especificado"
            });
        }

        return resultado;
    }
}

// Clases para deserializar la respuesta de ML
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
    public string? CurrencyId { get; set; }
    public string? Thumbnail { get; set; }
    public string? Permalink { get; set; }
    public string? Condition { get; set; }
    public string? BuyingMode { get; set; }
    public MlAddress? Address { get; set; }
    public MlSellerAddress? SellerAddress { get; set; }
}

public class MlAddress
{
    public string? CityName { get; set; }
    public string? StateName { get; set; }
}

public class MlSellerAddress
{
    public MlCity? City { get; set; }
    public MlState? State { get; set; }
}

public class MlCity
{
    public string? Name { get; set; }
}

public class MlState
{
    public string? Name { get; set; }
}

public class MlPaging
{
    public int Total { get; set; }
}

public class MlCategory
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

public class MlCategoryDetail
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public List<MlCategory>? ChildrenCategories { get; set; }
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