using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly MssqlDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrencyController(MssqlDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    // 1. GET: api/currency
    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        var currencies = await _context.Currencies.ToListAsync();
        return Ok(currencies);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCurrencies(int id)
    {
        var currencies = await _context.Currencies.Where(x=>x.Id==id).ToListAsync();
        return Ok(currencies);
    }


    // 2. POST: api/currency
    [HttpPost]
    public async Task<IActionResult> AddCurrency([FromBody] Currency currency)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        currency.UpdatedAt = DateTime.UtcNow;
        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCurrencies), new { id = currency.Id }, currency);
    }

    // 3. PUT: api/currency
    [HttpPut]
    public async Task<IActionResult> UpdateCurrency([FromBody] Currency currency)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingCurrency = await _context.Currencies.FindAsync(currency.Id);
        if (existingCurrency == null)
            return NotFound();

        existingCurrency.CurrencyCode = currency.CurrencyCode;
        existingCurrency.CurrencyName = currency.CurrencyName;
        existingCurrency.ExchangeRate = currency.ExchangeRate;
        existingCurrency.UpdatedAt = DateTime.UtcNow;

        _context.Currencies.Update(existingCurrency);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // 4. DELETE: api/currency
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurrency(int id)
    {
        var currency = await _context.Currencies.FindAsync(id);
        if (currency == null)
            return NotFound();

        _context.Currencies.Remove(currency);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // // 5. GET: api/currency/updateData
    // [HttpGet("updateData")]
    // public async Task<IActionResult> UpdateCurrencyData()
    // {
    //     var client = _httpClientFactory.CreateClient();
    //     var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");

    //     if (!response.IsSuccessStatusCode)
    //         return StatusCode((int)response.StatusCode, "Failed to fetch data from external API.");

    //     var json = await response.Content.ReadAsStringAsync();
    //     var coindeskData = JsonSerializer.Deserialize<CoindeskResponse>(json);

    //     if (coindeskData?.Bpi == null)
    //         return BadRequest("Invalid data format from external API.");

    //     foreach (var currency in coindeskData.Bpi)
    //     {
    //         var existingCurrency = await _context.Currencies
    //             .FirstOrDefaultAsync(c => c.CurrencyCode == currency.Key);

    //         if (existingCurrency != null)
    //         {
    //             existingCurrency.ExchangeRate = currency.Value.RateFloat;
    //             existingCurrency.UpdatedAt = DateTime.UtcNow;
    //         }
    //     }

    //     await _context.SaveChangesAsync();
    //     return Ok("Currencies updated successfully.");
    // }

    // // Coindesk API response model
    // public class CoindeskResponse
    // {
    //     public BpiData Bpi { get; set; }
    // }

    // public class BpiData
    // {
    //     public Dictionary<string, CurrencyRate> Bpi { get; set; }
    // }

    // public class CurrencyRate
    // {
    //     public string Code { get; set; }
    //     public string Description { get; set; }
    //     public decimal RateFloat { get; set; }
    // }
}