using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordValidation.Entites;
using PasswordValidation.InfraStructers;
using PasswordValidation.InfraStructers.DAL.EF.Context;
using PasswordValidation.Models;

namespace PasswordValidation
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string ss = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<UserManager<AppUser>,CustomUserManager<AppUser>>();
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IPasswordValidator<AppUser>, MyCustomPasswordValidator>();
            services.AddScoped<IUserPasswordChangeHistoryService, UserPasswordChangeHistoryService>();
            services.AddScoped<IUserPasswordBlackListService, UserPasswordBlackListService>();
            services.AddTransient<IValidationPassword, ValidationPassword_EFCore>();


            services.AddIdentity<AppUser, IdentityRole>(option=>
            {
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 4;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc();

            services.Configure<PasswordValidationConfig>(configuration.GetSection("PasswordValidationConfig"));
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
