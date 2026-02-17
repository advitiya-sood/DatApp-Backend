using DatApp.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using DatApp.Services;
using DatApp.Helpers;

namespace DatApp
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
    // 1. Database Connection
    services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
    
    // 2. Controllers (System.Text.Json is default in .NET 8)
    services.AddControllers();
    
    // 3. CORS - Important: Define a policy if you want more control, or keep it open for dev
    services.AddCors();

    services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
    
    // 4. Dependency Injection
    services.AddScoped<IAuthRepo, AuthRepo>();
    services.AddScoped<IUserRepository, UserRepository>();
    
    services.AddScoped<IAuthService, AuthService>();
    services.AddScoped<IUserService, UserService>();
    
    // 5. JWT Authentication
    var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateAudience = false,
                ValidateIssuer = false
            };
        });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    app.UseRouting();

    // CORS must be AFTER UseRouting but BEFORE UseAuthentication/Authorization
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
    }
}
