using Restaurante.BL;
using Restaurante.DA;

var builder = WebApplication.CreateBuilder(args);

// Capa de acceso a datos (EF Core + SQL Server). La UI no conoce a EF
// directamente; la conexión queda encapsulada dentro de Restaurante.DA.
builder.Services.AddDataAccess(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);

// Capa de lógica de negocio (servicios).
builder.Services.AddBusinessLogic();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mesas}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
