using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TorneoDeTitanes.Console.Infraestructura;
using TorneoDeTitanes.Console.Ui;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AgregarJuego(builder.Configuration);

using IHost host = builder.Build();

var torneo = host.Services.GetRequiredService<TorneoConsola>();
await torneo.EjecutarAsync();