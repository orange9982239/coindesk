using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    // GET: api/currency
    [HttpGet]
    public async Task<IActionResult> GetCurrencies()
    {
        var currencies = await _currencyService.GetAllCurrenciesAsync();
        return Ok(currencies);
    }

    // GET: api/currency/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCurrency(int id)
    {
        var currency = await _currencyService.GetCurrencyByIdAsync(id);
        if (currency == null)
            return NotFound();
            
        return Ok(currency);
    }

    // POST: api/currency
    [HttpPost]
    public async Task<IActionResult> AddCurrency([FromBody] Currency currency)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _currencyService.AddCurrencyAsync(currency);
        return CreatedAtAction(nameof(GetCurrency), new { id = result.Id }, result);
    }

    // PUT: api/currency
    [HttpPut]
    public async Task<IActionResult> UpdateCurrency([FromBody] Currency currency)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _currencyService.UpdateCurrencyAsync(currency);
        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/currency/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCurrency(int id)
    {
        var result = await _currencyService.DeleteCurrencyAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    // GET: api/currency/updateData
    [HttpGet("updateData")]
    public async Task<IActionResult> UpdateCurrencyData()
    {
        var result = await _currencyService.UpdateExternalCurrencyDataAsync();
        if (!result)
            return StatusCode(500, "Failed to update currency data");

        return Ok("Currencies updated successfully.");
    }
}