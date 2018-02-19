using System;
using System.IO;
using AuthenticationServices;
using AuthenticationServices.Jwt;
using Database.EntityFrameworkCore;
using Domain.Identity;
using LogService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Servises.Interfaces;
using Servises.Interfaces.AuthenticationServices;
using Swashbuckle.AspNetCore.Swagger;
using Utils.NonStatic;
using Web.Logging.DataBaseLogger;
using Web.Logging.FileLogger;
using Web.Middlewares;

namespace Web
{
    internal class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "CodeSampleA5ASPC2",
                        Description = "CodeSampleA5ASPC2",
                        Contact = new Contact { Name = "Dmitry Protko", Email = "mirypoko@gmail.com", Url = "https://github.com/mirypoko/CodeSampleA5ASPC2" },
                        Version = "v1"
                    });

                var basePath = AppContext.BaseDirectory;

                var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var fileName = Path.GetFileName(assemblyName + ".xml");
                c.IncludeXmlComments(Path.Combine(basePath, fileName));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, UserRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<SecurityStampValidatorOptions>(options
                => options.ValidationInterval = TimeSpan.FromSeconds(60));

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidIssuer = AuthJwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = AuthJwtOptions.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = AuthJwtOptions.SymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddMvc();

            services.AddTransient<IHttpUtilsService, HttpUtilsService>();
            services.AddTransient<IFilmsService, FilmsService.FilmsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISignInService, SignInService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IJwtTokensService, JwtTokensService>();
            services.AddTransient<IEmailService, EmailService.EmailService>();
            services.AddTransient<ILoggingService, LoggingService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<LogExceptionHandlerMiddleware>();

            Enum.TryParse(Configuration["LogLevel"], true, out LogLevel logLevel);
            loggerFactory.AddConsole(logLevel);
            loggerFactory.AddDebug(logLevel);
            loggerFactory.AddContext(logLevel, Configuration.GetConnectionString("DefaultConnection"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"), logLevel);
            }

            app.UseDefaultFiles();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web V1");
            });

            app.UseMvcWithDefaultRoute();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
        }
    }
}
