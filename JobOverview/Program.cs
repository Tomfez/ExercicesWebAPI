using JobOverview.Data;
using JobOverview.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JobOverview
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApiDocument(options =>
            {
                options.Title = "API JobOverview";
                options.Description = "<strong>API JobOverview pour formation ASP.Net Core.<br/>Code dispo sur <a href='https://github.com/developpeur-pro/ExercicesWebAPI'>ce référentiel GitHub</a></strong>";
                options.Version = "v1";
            });

            string? connect = builder.Configuration.GetConnectionString("JobOverviewConnect");
            builder.Services.AddDbContext<ContexteJobOverview>(opt => opt
                .UseSqlServer(connect)
                //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging());

            #region Auth
            // Ajoute le service d'authentification par porteur de jetons JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   // url d'accès au serveur d'identité
                   options.Authority = builder.Configuration["IdentityServerUrl"];
                   options.TokenValidationParameters.ValidateAudience = false;

                   // Tolérance sur la durée de validité du jeton
                   options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
               });

            // Ajoute le service d'autorisation
            builder.Services.AddAuthorization(options =>
            {
                // Spécifie que tout utilisateur de l'API doit être authentifié
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                // Seuls les managers peuvent gérer les tâches
                options.AddPolicy("GererTaches", p => p.RequireClaim("manager"));

                // Seuls les chefs de service peuvent gérer les équipes et les personnes
                options.AddPolicy("GererEquipes", p => p.RequireClaim("métier", "CDS"));
            });
            #endregion

            builder.Services.AddScoped<IServiceLogiciels, ServiceLogiciels>();
            builder.Services.AddScoped<IServiceEquipes, ServiceEquipes>();
            builder.Services.AddScoped<IServiceTaches, ServiceTaches>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
