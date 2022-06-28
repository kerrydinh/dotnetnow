using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using AutoMapper;
using DotNetNow.API.Middleware;
using DotNetNow.Application;
using DotNetNow.Auth.UserService;
using DotNetNow.Domain.Entity;
using DotNetNow.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DotNetNow.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private const string _myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy(name: _myAllowSpecificOrigins,
                                  builder =>
                                  {
                                      //builder.WithOrigins("http://localhost:4200", "http://localhost:4300")
                                      builder.AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader();
                                  });
            });

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ApplicationMappingModule());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddDbContext<CoreDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("DotNetNow.Persistence"));
            });
            
            services.AddIdentityCore<AppUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;

            })
            .AddEntityFrameworkStores<CoreDbContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddDefaultTokenProviders();


            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie("Identity.Application")
            .AddGoogle(options =>
                  {
                      options.ClientId = Configuration["GoogleAPI:ClientId"];
                      options.ClientSecret = Configuration["GoogleAPI:ClientSecret"];
                      options.SignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.Audience = Configuration["Auth0Setting:Audience"];
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Auth0Setting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth0Setting:SecretKey"]))
                };
                //cfg.SecurityTokenValidators.Clear();
                //cfg.SecurityTokenValidators.Add(new GoogleTokenValidator(Configuration));
            });


            /*.AddJwtBearer(cfg =>
             {
                 cfg.RequireHttpsMetadata = false;
                 cfg.SaveToken = true;

                 cfg.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidIssuer = Configuration["Tokens:Issuer"],
                     ValidAudience = Configuration["Tokens:Issuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                 };

             });*/

            services.AddHttpClient();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();

            services.AddSwaggerGen();


            services
                .AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(
                    opt => {
                        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        opt.SerializerSettings.Converters = new List<JsonConverter> { new StringEnumConverter() };
                    });

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<PersistenceModule>();
            builder.RegisterModule<ApplicationModule>();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("api/error");
            }

            app.UseMiddleware<JwtMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();
            app.UseCors(_myAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseSwagger();

            app.UseRequestLocalization();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
