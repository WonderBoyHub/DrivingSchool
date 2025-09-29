using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using bgk.Components;
using bgk.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices();

// Note: Payment services will need to be handled via API calls to a backend server
// as Mollie API requires server-side implementation for security reasons
builder.Services.AddScoped<IPaymentService, PaymentService>();

await builder.Build().RunAsync();