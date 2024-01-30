using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Request.Module.Application.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterRequestModule(builder.Configuration);

builder.Services.AddServerSideBlazor(e =>
{
    e.DetailedErrors = true;

});

builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 100024;

});

builder.Services.AddSignalR();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
       new[] { "application/octet-stream" });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

AppHelper.UseAutoMigration(builder.Configuration, app.Services);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseResponseCompression();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(
        name: "api",
        pattern: "api/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapBlazorHub();
});

app.Run();
