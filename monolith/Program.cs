using Microsoft.EntityFrameworkCore;
using Optivem.EShop.Monolith.Api.Exceptions;
using Optivem.EShop.Monolith.Core.Repositories;
using Optivem.EShop.Monolith.Core.Services;
using Optivem.EShop.Monolith.Core.Services.External;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            // Check if this is a JSON binding error by examining ModelState keys
            // JSON binding errors have keys that start with "$" (e.g., "$.quantity")
            var hasJsonBindingError = context.ModelState.Keys.Any(key => key.StartsWith("$"));
            
            if (hasJsonBindingError)
            {
                // Return 400 for JSON binding errors (type conversion failures)
                var problemDetails = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(context.ModelState)
                {
                    Status = 400
                };
                return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(problemDetails);
            }
            else
            {
                // Return 422 for semantic validation errors (Required, Range, etc.)
                var problemDetails = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(context.ModelState)
                {
                    Status = 422
                };
                return new Microsoft.AspNetCore.Mvc.UnprocessableEntityObjectResult(problemDetails);
            }
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

// Add services
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddHttpClient<ErpGateway>();
builder.Services.AddHttpClient<TaxGateway>();

// Add exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

// Fallback to index.html for root
app.MapFallbackToFile("index.html");

// Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
