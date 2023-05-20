using BrowseAndManageVideos_WEB.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BrowseAndManageVideos_WEB.Controllers.DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Movies")));

var app = builder.Build();
var handler = new HttpClientHandler
{
    // Bypass SSL certificate validation
    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
};

var httpClient = new HttpClient(handler);
var response = httpClient.GetAsync("https://example.com").Result;

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

app.Run();
