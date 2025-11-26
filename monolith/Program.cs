using Microsoft.EntityFrameworkCore;
using Optivem.AtddAccelerator.EShop.Monolith.Api.Exceptions;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Repositories;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Services;
using Optivem.AtddAccelerator.EShop.Monolith.Core.Services.External;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Database=eshop;Username=postgres;Password=postgres";

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Register services
builder.Services.AddScoped<OrderService>();

// Register HTTP clients for external services
builder.Services.AddHttpClient<ErpGateway>(client =>
{
    var erpUrl = builder.Configuration["ExternalServices:ErpApiUrl"] ?? "http://localhost:3100";
    client.BaseAddress = new Uri(erpUrl);
});

builder.Services.AddHttpClient<TaxGateway>(client =>
{
    var taxUrl = builder.Configuration["ExternalServices:TaxApiUrl"] ?? "http://localhost:3101";
    client.BaseAddress = new Uri(taxUrl);
});

// Add exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure middleware pipeline
app.UseExceptionHandler();

app.UseCors();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
