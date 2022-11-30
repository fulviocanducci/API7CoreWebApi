using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication4.Services;
using WebApplication4.Settings;
namespace WebApplication4.Extensions
{
    public static class GlobalExtension
    {
        public static IServiceCollection AddAuthenticationJwt(this IServiceCollection services)
        {
            services.AddAuthentication("jwt").AddJwtBearer("jwt", x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Secret.Bytes),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            return services;
        }

        public static IServiceCollection AddSwaggerGenJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1",
                    Description = "API",
                    Contact = new OpenApiContact
                    {
                        Name = "Fúlvio Cezar Canducci Dias",
                        Email = string.Empty,
                        Url = new Uri("https://api.s2vip.com.br/"),
                    },
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Description = "Authorization API",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return services;
        }

        public static WebApplication UseCorsDefault(this WebApplication app)
        {
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            return app;
        }

        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddAuthenticationJwt();
            services.AddAuthorization();
            services.AddCors();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenJwt();
            services.AddInjection();
            return services;
        }

        public static WebApplication AddWebApplication(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCorsDefault();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }

        public static WebApplication GetWebApplication(this WebApplicationBuilder builder)
        {
            builder.Services.AddServiceCollection();
            WebApplication app = builder.Build();
            app.AddWebApplication();
            return app;
        }

        public static IServiceCollection AddInjection(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}
