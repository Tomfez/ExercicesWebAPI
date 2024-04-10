using Duende.Bff;
using Duende.Bff.Yarp;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddBff().AddRemoteApis();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie("cookie", options =>
    {
        options.Cookie.Name = "__Host-blazor";
        options.Cookie.SameSite = SameSiteMode.Strict;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];

        options.ClientId = "BlazorJO";
        options.ClientSecret = "$xcuk%7yRLiKqD#r";
        options.ResponseType = "code";
        options.ResponseMode = "query";

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("entreprise");
        options.Scope.Add("offline_access");

        options.MapInboundClaims = false;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

builder.Services.AddUserAccessTokenManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseBff();
app.UseAuthorization();

app.MapBffManagementEndpoints();


app.MapRazorPages();

app.MapControllers()
    .RequireAuthorization()
    .AsBffApiEndpoint();

app.MapRemoteBffApiEndpoint("/Taches", builder.Configuration["ApiUrl"] + "Taches")
       .RequireAccessToken(TokenType.User);

app.MapRemoteBffApiEndpoint("/Logiciels", builder.Configuration["ApiUrl"] + "Logiciels")
       .RequireAccessToken(TokenType.User);

app.MapFallbackToFile("index.html");

app.Run();
