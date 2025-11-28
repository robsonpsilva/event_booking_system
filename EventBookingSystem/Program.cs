using EventBookingSystem.Components;
using EventBookingSystem.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.

// 1. Configuração do EF Core para usar SQLite
// Lê a string de conexão "DefaultConnection" do appsettings.Development.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. REGISTRO DO SERVIÇO DE EVENTOS
// O EventService é adicionado como Scoped, garantindo que um novo contexto
// de banco de dados seja criado por solicitação (request) no ambiente Blazor Server.
builder.Services.AddScoped<EventService>();


// Configuração do Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configura o pipeline de solicitação HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();