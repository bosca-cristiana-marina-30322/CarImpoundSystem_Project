using CarImpoundSystem.Data;
using CarImpoundSystem.Services;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionString));

// Add Authorization services
builder.Services.AddAuthorization();

// Register AuthService
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "deleteUser",
    pattern: "Admin/DeleteUser/{id}",
    defaults: new { controller = "Admin", action = "DeleteUser" },
    constraints: new { httpsMethod = new HttpMethodRouteConstraint("DELETE") });
app.MapControllerRoute(
    name: "adminEditUser",
    pattern: "Admin/EditUser/{id?}",
    defaults: new { controller = "Admin", action = "EditUser" }
);



app.Run();
