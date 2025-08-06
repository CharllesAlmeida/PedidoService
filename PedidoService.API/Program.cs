using FluentValidation;
using PedidoService.API.Configurations;
using PedidoService.API.Extensions;
using PedidoService.Application.Queries;
using PedidoService.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureKestrel();
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.ConfigureMongo(builder.Configuration);
builder.Services.ConfigureRabbitMq(builder.Configuration);
builder.Services.ConfigureRedis(builder.Configuration); 
builder.Services.RegisterApplicationServices();
builder.Services.AddMediatRHandlers();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CriarPedidoDtoValidator>();
builder.Services.AddScoped<IPedidoQueryService, PedidoQueryService>();

var app = builder.Build();

app.ApplyMigrations();
app.UseSwaggerIfDevelopment();
app.MapControllers();

app.Run();
