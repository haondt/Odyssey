using Haondt.Web.Extensions;
using Odyssey.Domain.Extensions;
using Odyssey.UI.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var testConfigFile = Path.Combine(Environment.CurrentDirectory, "appsettings.Test.json");
    if (File.Exists(testConfigFile))
        builder.Configuration.AddJsonFile(testConfigFile, optional: true, reloadOnChange: true);
}

// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddHaondtWebServices(builder.Configuration, options =>
    {
        options.HtmxScriptUri = "/static/shared/vendored/htmx.org/dist/htmx.min.js";
        options.HyperscriptScriptUri = "/static/shared/vendored/hyperscript.org/dist/_hyperscript.min.js";
    })
    .AddOdysseyDomainServices(builder.Configuration)
    .AddOdysseyUI(builder.Configuration);

builder.Services.AddMvc();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();

app
    .UseHaondtWeb()
    .UseOdysseyUI();

app.Run();
