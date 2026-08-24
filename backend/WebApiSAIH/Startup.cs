using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SAIH_Backend.Servicios.Autenticacion;
using SAIH_Backend.Servicios.Implementacion;
using SAIH_Backend.Servicios.Interfaces;
using System.IO;
using System.Text;
using WebApiSAIH.Models;
using WebApiSAIH.Services;
using WebApiSAIH.Services.Implementacion;
using WebApiSAIH.Services.Interfaces;

namespace WebApiSAIH
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ConfiguracionJWT>(Configuration.GetSection("ConfiguracionJWT"));
            // Configure Entityframecore with SQL SErver
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ProdConnection"));
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddCors();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["ConfiguracionJWT:JWT_Secret"])),
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IServicioEmployee, ServicioEmployee>();
            services.AddTransient<ISendGridEmailService, SendGridEmailService>();
            services.AddScoped<IServicioRegion, ServicioRegion>();
            services.AddScoped<IServicioDepartment, ServicioDepartment>();
            services.AddScoped<IServicioRole, ServicioRole>();
            services.AddScoped<IServicioSite, ServicioSite>();
            services.AddScoped<IServicioAuditLog, ServicioAuditLog>();
            services.AddScoped<IServicioEstadistica, ServicioEstadistica>();
            services.AddScoped<IServicioResource, ServicioResource>();
            services.AddScoped<IServicioGoal, ServicioGoal>();
            services.AddScoped<IServicioResourceGoal, ServicioResourceGoal>();
            services.AddScoped<IServicioDeliverable, ServicioDeliverable>();
            services.AddScoped<IServicioDocument, ServicioDocument>();
            services.AddScoped<IServicioTaskItem, ServicioTaskItem>();
            services.AddScoped<IServicioProject, ServicioProject>();
            services.AddScoped<IServicioProjectTask, ServicioProjectTask>();

            services.AddControllers();
            services.AddRazorPages();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiSAIH", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiSAIH v1"));
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiSAIH v1"));
                //app.UseHsts(); Enforces HTTPS via the HSTS protocol
            }

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("content-disposition"));

            //app.UseHttpsRedirection(); Redirects requests to HTTPS

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
