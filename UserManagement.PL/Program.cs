using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.BLL.Interfaces;
using UserManagement.BLL.Repositories;
using UserManagement.DAL.Contexts;
using UserManagement.DAL.Models;
using UserManagement.PL.MappingProfiles;
using UserManagement.PL.ViewModels;
using UserManagement.PL.Helpers;

namespace UserManagement.PL
{
    public class Program
    {
        //Entry Point
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure services that allow DI
            builder.Services.AddControllersWithViews();
            //Allow Dependancy Ingection for DB Context
            builder.Services.AddDbContext<OurCompanyDbContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
                );
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "Account/Login";
                        options.AccessDeniedPath = "Home/Error";
                    });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<OurCompanyDbContext>()
                    .AddDefaultTokenProviders();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));

            builder.Services.AddTransient<IEmailSettings, EmailSettings>();
            builder.Services.AddTransient<ISmsService, SmsService>();

            #endregion

            var app = builder.Build();

            #region Configure Pipelines
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                app.UseHttpsRedirection();
                //request required files
                app.UseStaticFiles();

                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            #endregion

            app.Run();
        }

        
    }
}
