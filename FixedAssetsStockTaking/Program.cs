using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FixedAssetsStockTaking.Data;
using FixedAssetsStockTaking.Helpers;
using Microsoft.AspNetCore.Authorization;
using RolesWithouFixedAssetsStockTakingtIdentity.Middleware;
using Microsoft.AspNetCore.Authentication.Negotiate;

namespace FixedAssetsStockTaking
{
    public class Program
    {
        public static void Main(string[] args)
        {   



            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<FixedAssetsStockTakingContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("FixedAssetsStockTakingContext") ?? throw new InvalidOperationException("Connection string 'FixedAssetsStockTakingContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Admin", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "Admin" })));
                opt.AddPolicy("Poweruser", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "Admin", "Operator" })));
                //opt.AddPolicy("User", policy => policy.Requirements.Add(new RoleRequirement(new string[] { "ADMIN", "POWERUSER", "USER" })));
            });

            //needs to be scoped, otherwise DI of DbContext doesn't work in customauthorization
            builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorization>();

            //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Inventars}/{action=Index}/{id?}");

            app.Run();
        }
    }
}