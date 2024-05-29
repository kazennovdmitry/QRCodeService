using QRCodeService.Interfaces;
using QRCodeService.BusinessServices;
using QRCodeService.Repositories;
using QRCodeService.Connectors;
using Microsoft.AspNetCore.Authentication;
using QRCodeService.Authentication;
using Microsoft.OpenApi.Models;

namespace QRCodeService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<IQRCodeFileRepository, QRCodeFileRepository>();
            builder.Services.AddScoped<IQRCodeBankConnector, QRCodeBankConnector>();
            builder.Services.AddScoped<IQRCodeBusinessService, QRCodeBusinessService>();
            builder.Services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                (BasicAuthenticationDefaults.AuthenticationScheme, null);

            var configuration = builder.Configuration;
            builder.Services.Configure<AuthenticationSettings>(configuration.GetSection("Authentication"));

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "QR PH Service",
                    Description = "NetBankPH QRCode ASP.NET Web API",
                    Contact = new OpenApiContact
                    {
                        Name = configuration["Contact:Name"],
                        Email = configuration["Contact:Email"]
                    }
                });
            });

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QR PH Service V1");
                c.RoutePrefix = string.Empty;

            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}