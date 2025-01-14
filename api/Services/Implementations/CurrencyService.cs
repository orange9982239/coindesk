using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class CurrencyService : ICurrencyService
{
    private readonly MssqlDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public CurrencyService(MssqlDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<Currency>> GetAllCurrenciesAsync()
    {
        return await _context.Currencies.ToListAsync();
    }

    public async Task<Currency> GetCurrencyByIdAsync(int id)
    {
        return await _context.Currencies.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Currency> AddCurrencyAsync(Currency currency)
    {
        currency.UpdatedAt = DateTime.UtcNow;
        _context.Currencies.Add(currency);
        await _context.SaveChangesAsync();
        return currency;
    }

    public async Task<bool> UpdateCurrencyAsync(Currency currency)
    {
        var existingCurrency = await _context.Currencies.FindAsync(currency.Id);
        if (existingCurrency == null)
            return false;

        existingCurrency.CurrencyCode = currency.CurrencyCode;
        existingCurrency.CurrencyName = currency.CurrencyName;
        existingCurrency.ExchangeRate = currency.ExchangeRate;
        existingCurrency.UpdatedAt = DateTime.UtcNow;

        _context.Currencies.Update(existingCurrency);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteCurrencyAsync(int id)
    {
        var currency = await _context.Currencies.FindAsync(id);
        if (currency == null)
            return false;

        _context.Currencies.Remove(currency);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateExternalCurrencyDataAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");

            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (!root.TryGetProperty("bpi", out var bpiElement) || bpiElement.ValueKind != JsonValueKind.Object)
                return false;

            foreach (var currency in bpiElement.EnumerateObject())
            {
                var currencyCode = currency.Name;
                var currencyData = currency.Value;

                if (currencyData.TryGetProperty("rate_float", out var rateFloatElement) && 
                    rateFloatElement.TryGetDecimal(out var rateFloat))
                {
                    var existingCurrency = await _context.Currencies
                        .FirstOrDefaultAsync(c => c.CurrencyCode == currencyCode);

                    if (existingCurrency != null)
                    {
                        existingCurrency.ExchangeRate = rateFloat;
                        existingCurrency.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        var newCurrency = new Currency
                        {
                            CurrencyCode = currencyCode,
                            CurrencyName = $"請更新幣別對應中文名稱[{currencyCode}]",
                            ExchangeRate = rateFloat,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _context.Currencies.AddAsync(newCurrency);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}