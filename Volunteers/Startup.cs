using AutoMapper;
using BL;
using DL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volunteers.MiddleWare;

namespace Volunteers
{
    public class Startup
    {
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the run
        // . Use this method to add services to the container.  
        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(config, o => o.BindNonPublicProperties = true);

            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("key").Value);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddScoped<IMatchingFunctionBL, MatchingFunctionBL>();
            services.AddResponseCaching();
            services.AddScoped<IImageBL, ImageBL>();
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IUserDL, UserDL>();
            services.AddScoped< IDriverDL, DriverDL>();
            services.AddScoped<IDriverBL, DriverBL>();
            services.AddScoped<IPersonDL, PersonDL>();
            services.AddScoped<IPassengerRequestBL,PassengerRequestBL>();
            services.AddScoped<IPassengerRequestDL,PassengerRequestDL>();
            services.AddScoped<IDriveBL, DriveBL>();
            services.AddScoped<IDriveDL, DriveDL>();
            services.AddScoped<IDriverRequestBL, DriverRequestBL>();
            services.AddScoped<IDriverRequestDL, DriverRequestDL>();
            services.AddScoped<IRatingDL, RatingDL>();
            services.AddScoped<IRatingBL, RatingBL>();
            services.AddScoped<IPasswordHashHelper, PasswordHashHelper>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Volunteers", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}

                    }
                });
        });
            services.AddDbContext<VolunteersContext>(options => options.UseSqlServer(Configuration.GetConnectionString("VolunteersApp")), ServiceLifetime.Scoped);
            services.AddAutoMapper(typeof(Startup));

            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILogger<Startup> logger)
        {
            logger.LogInformation("server is up!");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Volunteers v1"));
            }
         

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCacheMiddleware();
            app.UseAuthentication();
            app.UseCSPMiddleware();
            app.UseRouting();
            
            app.Map("/api", (api1) =>
            {
                api1.UseRouting();
                api1.UseRatingMiddleware();
                api1.UseAuthorization();

                api1.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
                api1.UseErrorMiddleware();

            }
            );
            app.UseAuthorization();


            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });


             
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
