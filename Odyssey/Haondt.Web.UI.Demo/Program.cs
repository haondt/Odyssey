using Haondt.Web.Core.Middleware;
using Haondt.Web.Core.ModelBinders;
using Haondt.Web.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Demo;
using Haondt.Web.UI.Extensions;
using Haondt.Web.UI.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddHaondtUIHyperscriptScripts()
    .AddHaondtWebServices(builder.Configuration)
    .Configure<HtmxOptions>(o =>
    {
        o.Extensions.Add("morph");
    });

builder.Services.AddMvc(options =>
{
    options.ModelBinderProviders.Insert(0, new OptionalModelBinderProvider());
    options.ModelBinderProviders.Insert(0, new AbsoluteDateTimeModelBinderProvider());
});

builder.Services
    .AddHaondtUI(builder.Configuration)
    .AddScoped<IHeadEntryDescriptor>(_ => new ScriptDescriptor
    {
        Uri = "https://unpkg.com/idiomorph@0.7.4/dist/idiomorph-ext.min.js",
        CrossOrigin = "anonymous"
    }).AddScoped<IHeadEntryDescriptor>(_ => new StyleSheetDescriptor
    {
        Uri = "/static/Haondt.Web.UI.Demo.styles.css",
    })
    .AddScoped<IHeadEntryDescriptor>(_ => new MetaDescriptor
    {
        Name = "htmx-config",
        Content = @"{
            ""responseHandling"": [
                { ""code"": ""204"", ""swap"": false },
                { ""code"": ""405"", ""swap"": false },
                { ""code"": "".*"", ""swap"": true }
            ],
            ""scrollIntoViewOnBoost"": false,
            ""historyCacheSize"": 0,
            ""historyRestoreAsHxRequest"": true,
            ""disableInheritance"": true
        }",
    })
    .AddSingleton<ILayoutComponentFactory, DemoLayoutComponentFactory>();

builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseHaondtWeb();

app.UseMiddleware<RenderContextMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();
app.AddHaondtWebUIEndpoints();

app.Run();
