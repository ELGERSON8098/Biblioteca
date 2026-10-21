using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registrar IAutorService -> AutorService (Scoped)
builder.Services.AddScoped<Biblioteca.Services.IAutorService, Biblioteca.Services.AutorService>();

// Registrar DbContext con SQL Server
var connectionString = builder.Configuration.GetConnectionString("BibliotecaConnection");
builder.Services.AddDbContext<BibliotecaContext>(options => options.UseSqlServer(connectionString));

// Configurar Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<BibliotecaContext>()
    .AddDefaultTokenProviders();

// Configurar rutas de Identity para autorización (acceso denegado / login)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Crear roles y usuarios de prueba si no existen (seeding)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Crear roles
        string[] roles = { "Admin", "Bibliotecario", "Usuario" };
        foreach (var roleName in roles)
        {
            if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
        }

        // Usuario Admin de prueba
        var adminEmail = "admin@example.com";
        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = "admin", Email = adminEmail, EmailConfirmed = true };
            userManager.CreateAsync(adminUser, "Admin1234").GetAwaiter().GetResult();
        }
        if (!userManager.IsInRoleAsync(adminUser, "Admin").GetAwaiter().GetResult())
        {
            userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }

        // Usuario Bibliotecario de prueba
        var bibliotecarioEmail = "bibliotecario@example.com";
        var bibliotecarioUser = userManager.FindByEmailAsync(bibliotecarioEmail).GetAwaiter().GetResult();
        if (bibliotecarioUser == null)
        {
            bibliotecarioUser = new IdentityUser { UserName = "bibliotecario", Email = bibliotecarioEmail, EmailConfirmed = true };
            userManager.CreateAsync(bibliotecarioUser, "Biblio1234").GetAwaiter().GetResult();
        }
        if (!userManager.IsInRoleAsync(bibliotecarioUser, "Bibliotecario").GetAwaiter().GetResult())
        {
            userManager.AddToRoleAsync(bibliotecarioUser, "Bibliotecario").GetAwaiter().GetResult();
        }

        // Usuario estándar de prueba
        var testEmail = "test@example.com";
        var testUser = userManager.FindByEmailAsync(testEmail).GetAwaiter().GetResult();
        if (testUser == null)
        {
            testUser = new IdentityUser { UserName = "testuser", Email = testEmail, EmailConfirmed = true };
            userManager.CreateAsync(testUser, "Test1234").GetAwaiter().GetResult();
        }
        if (!userManager.IsInRoleAsync(testUser, "Usuario").GetAwaiter().GetResult())
        {
            userManager.AddToRoleAsync(testUser, "Usuario").GetAwaiter().GetResult();
        }
    }
    catch
    {
        // ignorar errores de seeding en tiempo de arranque
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
