using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Meridian.IngestionManagement.Infrastructure.Services;

using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using Amazon.Extensions.NETCore.Setup;
using Amazon;
using Amazon.S3;
using Meridian.IngestionManagement.Interface.Services;
using Meridian.IngestionManagement.Core.BackgroundTasks;
using Meridian.IngestionManagement.Infrastructure.Data;
using Meridian.IngestionManagement.Core.Interfaces.Services;
using Meridian.IngestionManagement.Core.Interfaces.Repositories;
using Meridian.IngestionManagement.Core.Services;
using Microsoft.EntityFrameworkCore;
using Meridian.IngestionManagement.Core.Interfaces.BackgroundTasks;
namespace Meridian.IngestionManagement
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

            services.AddDbContext<AppDbContext>(builder => builder.UseNpgsql(connectionString));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Meridian.IngestionManagement", Version = "v1" });
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

            #region Add S3 to the ASP.NET Core dependency injection framework.

            AWSOptions awsOptions = new AWSOptions
            {
                Region = RegionEndpoint.GetBySystemName(Configuration.GetSection("S3")["S3BucketRegion"])
            };
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();

            #endregion
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddScoped<IBucketService, S3BucketService>();
            services.AddTransient<IMetaDataService, MetaDataService>();
            services.AddTransient<IMetaDataRepository, MetaDataRepository>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IManifestService, ManifestService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dataContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            dataContext.Database.Migrate();

            app.UseRouting();

            app.UseAuthentication();

            app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meridian.IngestionManagement v1"));

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
