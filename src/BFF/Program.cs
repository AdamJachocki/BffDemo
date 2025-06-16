using BFF.Abstractions;
using BFF.Services;
using BFF.Transforms;

namespace BFF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<IAuthCookieService, AuthCookieService>();
            builder.Services.AddSingleton<IAccessTokenEncoder, AccessTokenEncoder>();

            builder.Services.AddSingleton<AccessTokenRequestTransform>();
            builder.Services.AddSingleton<AccessTokenResponseTransform>();
            builder.Services.AddReverseProxy()
                .AddTransforms<TransformProvider>()
                .LoadFromConfig(builder.Configuration.GetSection("ProxyConfig"));

            var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>()!;
            var corsExposedHeaders = builder.Configuration.GetSection("CorsExposedHeaders").Get<string[]>()!;
            builder.Services.AddCors(o =>
            {
                o.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins(corsOrigins)
                        .AllowCredentials()
                        .WithExposedHeaders(corsExposedHeaders);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseCors();

            app.MapReverseProxy();

            app.Run();
        }
    }
}
