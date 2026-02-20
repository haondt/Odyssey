using Haondt.Core.Extensions;
using Haondt.Web.Core.Middleware;
using Haondt.Web.Core.ModelBinders;
using Haondt.Web.Extensions;
using Haondt.Web.Services;
using Haondt.Web.UI.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Odyssey.Client.Authentication.Models;
using Odyssey.Client.Core.Extensions;
using Odyssey.Client.Core.Models;
using Odyssey.Core.Constants;
using Odyssey.Domain.Core.Extensions;
using Odyssey.Games.Client.Core.Extensions;
using Odyssey.Games.Domain.Core.Extensions;
using Odyssey.GrainInterfaces.Core.Extensions;
using Odyssey.Persistence;
using Odyssey.Persistence.Extensions;
using Odyssey.Persistence.Models;
using Odyssey.Services;
using Odyssey.UI.Core.Extensions;
using Odyssey.UI.Core.Middlewares;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    var testConfigFile = Path.Combine(Environment.CurrentDirectory, "appsettings.Test.json");
    if (File.Exists(testConfigFile))
        builder.Configuration.AddJsonFile(testConfigFile, optional: true, reloadOnChange: true);
}

builder.Configuration.AddEnvironmentVariables();



builder.Services
    .AddHaondtWebServices(builder.Configuration, options =>
    {
        options.HtmxScriptUri = "/static/shared/vendored/htmx.org/dist/htmx.min.js";
        options.HyperscriptScriptUri = "/static/shared/vendored/hyperscript.org/dist/_hyperscript.min.js";
    })
    .Configure<HtmxOptions>(o =>
    {
        o.Extensions.Add("morph");
        o.Extensions.Add("loading-states");
    })
    .AddOdysseyGrainInterfacesServices(builder.Configuration)
    .AddOdysseyPersistenceClientServices(builder.Configuration)
    .AddOdysseyDomainServices(builder.Configuration)
    .AddOdysseyClientServices(builder.Configuration)
    .AddOdysseyGames()
    .AddOdysseyGamesClientServices(builder.Configuration);


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// add identity services

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var routeSettings = builder.Configuration.GetRequiredSection<RouteSettings>();
var authSettings = builder.Configuration.GetRequiredSection<AuthenticationSettings>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = routeSettings.BasePath + "/auth/sign-in";
    options.Cookie.HttpOnly = authSettings.HttpOnly;
    options.Cookie.Name = authSettings.CookieName;
    options.Cookie.SameSite = authSettings.SameSite;
    options.Cookie.SecurePolicy = authSettings.Secure ? CookieSecurePolicy.Always : CookieSecurePolicy.SameAsRequest;
    options.ExpireTimeSpan = TimeSpan.FromDays(authSettings.ExpireTimeSpanDays);
    options.SlidingExpiration = authSettings.SlidingExpiration;
});

builder.Services
    .AddIdentityCore<UserDataSurrogate>(options =>
    {
        options.Stores.MaxLengthForKeys = 128;
    })
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.TryAddTransient<IEmailSender, NoOpEmailSender>();
builder.Services.TryAddTransient(typeof(IEmailSender<>), typeof(DefaultMessageEmailSender<>));
builder.Services.Configure<IdentityOptions>(o =>
{
    o.User.AllowedUserNameCharacters = AuthConstants.AllowedUsernameCharacters;
});

// orleans

builder.Services.AddOrleansClient(client =>
{
    client.ConfigureCluster(builder.Configuration);
});


// add other web services

builder.Services
    .AddHaondtUI(builder.Configuration)
    .AddOdysseyUI(builder.Configuration);


builder.Services.AddMvc(options =>
{
    options.ModelBinderProviders.Insert(0, new OptionalModelBinderProvider());
    options.ModelBinderProviders.Insert(0, new AbsoluteDateTimeModelBinderProvider());

});
builder.Services.AddServerSideBlazor();
builder.Services.AddCors(options =>
{
    options.AddPolicy(OdysseyConstants.CorsPolicyName, policy =>
    {
        policy
            .WithOrigins([.. authSettings.Origins])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.

app
    .UseHaondtWeb()
    .UseOdysseyUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors(OdysseyConstants.CorsPolicyName);
app.UseAntiforgery();

app.MapControllers();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<UnmappedRouteHandlerMiddleware>();
app.MapHealthChecks("hc");



await app.RunAsync();
