using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.DataBase;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDB>(options => options.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=PizzaApp;"));

builder.Services.AddRazorPages();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
