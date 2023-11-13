using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using Meridian.NotificationManagement.Infrastructure.Data;
using Meridian.NotificationManagement.Core.Interfaces.Services;
using Meridian.NotificationManagement.Infrastructure.Services;
using Meridian.NotificationManagement.Core.Interfaces.Repositories;
using Meridian.NotificationManagement.Model;
using Meridian.NotificationManagement.API;

namespace Meridian.NotificationManagement
{
    public class Startup
    {
        public Startup(IConfiguration config, IWebHostEnvironment env)
        {
            Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
          
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            //OutboundAPI outboundAPI = new OutboundAPI();
           //string connectionString =  outboundAPI.GetfromAWSSecret(key);

            services.AddDbContext<AppDbContext>(builder => builder.UseNpgsql(connectionString));

            services.AddControllers();

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotoficationRepository, NotificationRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Meridian.NotificationManagement", Version = "v1" });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                    },
                    System.Array.Empty<string>()
                  }
                });
                c.EnableAnnotations();
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.Authority = Configuration["JWT:Authority"];
                options.Audience = Configuration["JWT:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role
                };

            });

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            app.UseRouting();

            app.UseAuthentication();

            //app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meridian.NotificationManagement v1"));

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
