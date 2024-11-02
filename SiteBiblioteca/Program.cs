using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiteBiblioteca.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configura Identity para exigir confirma��o de conta
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configura as p�ginas Razor com restri��es de acesso
builder.Services.AddRazorPages(options =>
{
    // Exige autentica��o para todas as p�ginas dentro de /Account/Manage
    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");

    // Permite acesso an�nimo especificamente � p�gina de Perfil
    options.Conventions.AllowAnonymousToPage("/Account/Manage/Perfil");
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Certifique-se de adicionar autentica��o
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
