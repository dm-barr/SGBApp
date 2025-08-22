using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SGBApp.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Agregar logging a consola
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Obtener logger
var logger = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddConsole();
}).CreateLogger<Program>();

// Cadena de conexión
var connectionString = "Server=tcp:sgbapp.database.windows.net,1433;Initial Catalog=SGBAppDB;Persist Security Info=False;User ID=sgbadmin;Password=GestionBiblioteca1510;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

// 🔹 Probar conexión
try
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        logger.LogInformation("✅ Conexión exitosa a SQL Server");
    }
}
catch (Exception ex)
{
    logger.LogError("❌ Error de conexión: {Message}", ex.Message);
}

// 🔹 Configuración de DbContext con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// 🔹 Configuración de Identity (sin DefaultUI)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 🔹 Agregar Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// 🔹 Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 🔹 Ruta para Razor Pages
app.MapRazorPages();

app.Run();
