using MusicKG.HondaPlugins.Services.Helpers;
using MusicKG.HondaPlugins.VehicleFault.Rest.Filters;
using MusicKG.HondaPlugins.Services;
using MusicKG.HondaPlugins.DataAccess.Extensions;
using MusicKG.HondaPlugins.DataAccess.Settings;
using MusicKG.Scheduler.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using MusicKG.HondaPlugins.WarningCalculator;

namespace MusicKG.HondaPlugins.VehicleFault.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).Enrich.FromLogContext().CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "MusicKG",
                        ValidateAudience = true,
                        ValidAudience = "MusicKG",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.RsaPublicKey
                    };
                });
            services.AddCors();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.OutputFormatters.RemoveType(typeof(SystemTextJsonOutputFormatter));
            }).AddNewtonsoftJson();

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Honda Vehicle Fault API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "MusicKG.HondaPlugins.VehicleFault.Rest.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.AddHttpClient().AddConfiguration<VehicleDataConstructorSettings>(Configuration)
                .AddScheduler(Configuration.GetSection("Scheduler"), null)
                .AddHondaDataAccess(Configuration.GetSection("HondaDataAccess"))
                .AddHondaServices(Configuration)
                .AddWarningCalculator(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var supportedCultures = new[] { new CultureInfo("zh-CN") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("zh-CN"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "api/swagger/MusicKG.HondaPlugins.VehicleFault.Rest/{documentName}/swagger.json";

                options.PreSerializeFilters.Add((doc, req) =>
                {
                    var servers = new List<OpenApiServer> { };

                    servers.Add(new OpenApiServer { Url = $"{req.Scheme}://{req.Host.Value}" });

                    if (!"https".Equals(req.Scheme, StringComparison.OrdinalIgnoreCase))
                        servers.Add(new OpenApiServer { Url = $"https://{req.Host.Value}" });

                    doc.Servers = servers;
                });
            });

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "api/swagger";

                options.SwaggerEndpoint("MusicKG.HondaPlugins.VehicleFault.Rest/v1/swagger.json", "Honda Vehicle Fault API");
            });
        }
    }
}