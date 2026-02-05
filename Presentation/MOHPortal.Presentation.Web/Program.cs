using MOHPortal.Core.Application.Resolver;
using Smidge;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

// Register your services.
builder.Services.ResolveAppServices(builder.Configuration);

bool useBackOffice = builder.Configuration.GetValue<bool>("UseBackOffice");

builder.Services.AddSmidge(builder.Configuration.GetSection("smidge"));

WebApplication app = builder.Build();


// 1. Boot Umbraco first
await app.BootUmbracoAsync();

// 2. IMPORTANT: Initialize Routing before Smidge or Umbraco Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// --------------------------------



// 3. Now you can use Smidge
app.UseSmidge();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
