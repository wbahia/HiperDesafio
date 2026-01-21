using Hiper.Desafio.Application.Services;
using Hiper.Desafio.Domain.Interfaces;
using Hiper.Desafio.Domain.Strategies;
using Hiper.Desafio.Infra.Context;
using Hiper.Desafio.Infra.Messaging;
using Hiper.Desafio.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

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
//builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Strategies
builder.Services.AddScoped<IDescontoStrategy, DescontoVipStrategy>();
builder.Services.AddScoped<IDescontoStrategy, DescontoComumStrategy>();

// Calculadora (Contexto da Strategy)
builder.Services.AddScoped<CalculadoraDesconto>();

// Mensageria
builder.Services.AddScoped<MessageBusService>();

// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
    options.AddPolicy("AllowReact",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS e o mapeamento de Controllers
app.UseCors("DefaultPolicy");
app.UseAuthorization();
app.UseCors("AllowReact");

app.MapControllers();

app.Run();