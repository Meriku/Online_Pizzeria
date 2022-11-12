using Microsoft.EntityFrameworkCore;
using Online_Pizzeria.DataBase;
using Online_Pizzeria.Logic;


var builder = WebApplication.CreateBuilder(args);
var connectionString = GetConnectionString();

builder.Services.AddDbContext<ApplicationDB>(options => options.UseSqlServer(connectionString));

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

string GetConnectionString()
{
    var appConfig = builder.Configuration;

    string hostname = appConfig.GetValue<string>("DataBase:RDS_HOSTNAME");
    string port = appConfig.GetValue<string>("DataBase:RDS_PORT");
    string dbname = appConfig.GetValue<string>("DataBase:RDS_DB_NAME");
    string username = appConfig.GetValue<string>("DataBase:RDS_USERNAME");
    string password = appConfig.GetValue<string>("DataBase:RDS_PASSWORD");
    
    return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
}