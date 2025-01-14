using NUnit.Framework;
using Moq;
using Moq.Protected; // 添加這行來使用 Protected()
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading; // 添加這行來使用 CancellationToken

namespace api.UnitTests.Services
{
    [TestFixture]
    public class CurrencyServiceTests
    {
        private CurrencyService _currencyService;
        private MssqlDbContext _context;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;

        [SetUp]
        public void Setup()
        {
            // 設置 In-Memory Database
            var options = new DbContextOptionsBuilder<MssqlDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new MssqlDbContext(options);

            // 設置 Mock HttpClientFactory
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            
            _currencyService = new CurrencyService(_context, _mockHttpClientFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetAllCurrenciesAsync_ShouldReturnAllCurrencies()
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency { Id = 1, CurrencyCode = "USD", CurrencyName = "美元", ExchangeRate = 1.0m },
                new Currency { Id = 2, CurrencyCode = "EUR", CurrencyName = "歐元", ExchangeRate = 0.85m }
            };
            await _context.Currencies.AddRangeAsync(currencies);
            await _context.SaveChangesAsync();

            // Act
            var result = await _currencyService.GetAllCurrenciesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.Any(c => c.CurrencyCode == "USD"), Is.True);
            Assert.That(result.Any(c => c.CurrencyCode == "EUR"), Is.True);
        }

        [Test]
        public async Task GetCurrencyByIdAsync_WithValidId_ShouldReturnCurrency()
        {
            // Arrange
            var currency = new Currency 
            { 
                Id = 1, 
                CurrencyCode = "USD", 
                CurrencyName = "美元", 
                ExchangeRate = 1.0m 
            };
            await _context.Currencies.AddAsync(currency);
            await _context.SaveChangesAsync();

            // Act
            var result = await _currencyService.GetCurrencyByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CurrencyCode, Is.EqualTo("USD"));
        }

        [Test]
        public async Task AddCurrencyAsync_ShouldAddNewCurrency()
        {
            // Arrange
            var currency = new Currency 
            { 
                CurrencyCode = "JPY", 
                CurrencyName = "日圓", 
                ExchangeRate = 110.0m 
            };

            // Act
            var result = await _currencyService.AddCurrencyAsync(currency);

            // Assert
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.UpdatedAt, Is.Not.EqualTo(default(DateTime)));
            
            var savedCurrency = await _context.Currencies.FindAsync(result.Id);
            Assert.That(savedCurrency, Is.Not.Null);
            Assert.That(savedCurrency.CurrencyCode, Is.EqualTo("JPY"));
        }

        [Test]
        public async Task UpdateCurrencyAsync_WithValidCurrency_ShouldUpdateCurrency()
        {
            // Arrange
            var currency = new Currency 
            { 
                Id = 1, 
                CurrencyCode = "USD", 
                CurrencyName = "美元", 
                ExchangeRate = 1.0m 
            };
            await _context.Currencies.AddAsync(currency);
            await _context.SaveChangesAsync();

            currency.ExchangeRate = 1.1m;

            // Act
            var result = await _currencyService.UpdateCurrencyAsync(currency);

            // Assert
            Assert.That(result, Is.True);
            var updatedCurrency = await _context.Currencies.FindAsync(1);
            Assert.That(updatedCurrency.ExchangeRate, Is.EqualTo(1.1m));
        }

        [Test]
        public async Task DeleteCurrencyAsync_WithValidId_ShouldDeleteCurrency()
        {
            // Arrange
            var currency = new Currency 
            { 
                Id = 1, 
                CurrencyCode = "USD", 
                CurrencyName = "美元", 
                ExchangeRate = 1.0m 
            };
            await _context.Currencies.AddAsync(currency);
            await _context.SaveChangesAsync();

            // Act
            var result = await _currencyService.DeleteCurrencyAsync(1);

            // Assert
            Assert.That(result, Is.True);
            var deletedCurrency = await _context.Currencies.FindAsync(1);
            Assert.That(deletedCurrency, Is.Null);
        }

        [Test]
        public async Task UpdateExternalCurrencyDataAsync_WithValidResponse_ShouldUpdateCurrencies()
        {
            // Arrange
            var jsonResponse = @"{
                ""bpi"": {
                    ""USD"": {
                        ""rate_float"": 50000.0
                    },
                    ""EUR"": {
                        ""rate_float"": 42000.0
                    }
                }
            }";

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var client = new HttpClient(mockHandler.Object);
            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(client);

            // Act
            var result = await _currencyService.UpdateExternalCurrencyDataAsync();

            // Assert
            Assert.That(result, Is.True);
            var currencies = await _context.Currencies.ToListAsync();
            Assert.That(currencies.Count, Is.EqualTo(2));
            Assert.That(currencies.Any(c => c.CurrencyCode == "USD"), Is.True);
            Assert.That(currencies.Any(c => c.CurrencyCode == "EUR"), Is.True);
        }
    }
}