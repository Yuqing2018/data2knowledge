using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using MusicKG.Service.Helpers;
using MusicKG.WebApi.Extensions;
using MusicKG.WebApi.Filters;
using MusicKG.WebApi.Helpers;
using MusicKG.WebApi.Contract.ViewModels;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using System;

namespace MusicKG.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                options.OutputFormatters.RemoveType(typeof(SystemTextJsonOutputFormatter));
            }).AddNewtonsoftJson();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new JsonResult(new ErrorViewModel
                    {
                        Message = string.Join("", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)),
                    });
                    result.StatusCode = (int)HttpStatusCode.BadRequest;

                    return result;
                };
            });

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MusicKG Web API",
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
                var xmlPath = Path.Combine(basePath, "MusicKG.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.RegisterSettingsAndServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                options.RouteTemplate = "api/swagger/MusicKG.WebApi/{documentName}/swagger.json";

                options.PreSerializeFilters.Add((doc, req) =>
                {
                    var servers = new List<OpenApiServer> { };

                    if (!"https".Equals(req.Scheme, StringComparison.OrdinalIgnoreCase))
                        servers.Add(new OpenApiServer { Url = $"https://{req.Host.Value}" });

                    servers.Add(new OpenApiServer { Url = $"{req.Scheme}://{req.Host.Value}" });

                    doc.Servers = servers;
                });
            });

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "api/swagger";

                options.SwaggerEndpoint("MusicKG.WebApi/v1/swagger.json", "MusicKG Web API");
            });
        }
    }
}
