using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 啟用 Controller 支援
builder.Services.AddControllers();
// 註冊 HttpClientFactory
builder.Services.AddHttpClient();
// 註冊 CurrencyService
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊 MssqlDbContext
builder.Services.AddDbContext<MssqlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 啟用 Controller 路由
app.MapControllers();

app.Run();
