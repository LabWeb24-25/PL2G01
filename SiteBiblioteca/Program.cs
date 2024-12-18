using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiteBiblioteca.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Torna o cookie não persistente (apenas sessão atual)
    options.Cookie.IsEssential = true; // Opcional, usado para LGPD
    options.ExpireTimeSpan = TimeSpan.FromSeconds(5); // Tempo até expirar
    options.SlidingExpiration = false; // Não renova o cookie automaticamente
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";

    // Define o cookie como não persistente
    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        OnSigningIn = context =>
        {
            context.Properties.IsPersistent = false; // Desabilita persistência
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Desativa a exigência de confirmação de conta
    options.User.RequireUniqueEmail = true; // Exige que os emails sejam únicos
})
.AddRoles<IdentityRole>() // Adiciona suporte a papéis (Roles)
.AddEntityFrameworkStores<ApplicationDbContext>(); // Configura o uso do Entity Framework para persistência dos dados

builder.Services.AddControllersWithViews(); // Adiciona suporte a controladores com views

builder.Services.AddScoped<RoleManager<IdentityRole>>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    SeedRoles.Seed(roleManager);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
