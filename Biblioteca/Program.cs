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

var app = builder.Build();

// Crear usuario de prueba si no existe (seeding)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var testEmail = "test@example.com";
        var testUser = userManager.FindByEmailAsync(testEmail).GetAwaiter().GetResult();
        if (testUser == null)
        {
            var user = new IdentityUser { UserName = "testuser", Email = testEmail, EmailConfirmed = true };
            var result = userManager.CreateAsync(user, "Test1234").GetAwaiter().GetResult();
            // no hacemos nada si falla; se puede revisar en logs
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
