using Hiper.Desafio.Infra.Context;
using Hiper.Desafio.Infra.Repositories;
using Hiper.Desafio.Domain.Interfaces; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("ConnectionString 'DefaultConnection' não encontrada.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// DI
// Scoped: um por requisição HTTP
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Controllers e Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(); // .NET 9 standard

// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS e o mapeamento de Controllers
app.UseCors("DefaultPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();