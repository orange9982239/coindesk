
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICurrencyService
{
    Task<IEnumerable<Currency>> GetAllCurrenciesAsync();
    Task<Currency> GetCurrencyByIdAsync(int id);
    Task<Currency> AddCurrencyAsync(Currency currency);
    Task<bool> UpdateCurrencyAsync(Currency currency);
    Task<bool> DeleteCurrencyAsync(int id);
    Task<bool> UpdateExternalCurrencyDataAsync();
}