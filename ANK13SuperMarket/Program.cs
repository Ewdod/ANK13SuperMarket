using ANK13SuperMarket.Context;
using Microsoft.EntityFrameworkCore;

namespace ANK13SuperMarket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MarketDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Baglanti")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllerRoute(
                    name: "CustomRoute",
                    pattern: "custom/{param:CustomRouteConstraint}",
                    defaults: new { controller = "Home", action = "CustomAction" }
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Market}/{action=Index}/{id?}");

            app.Run();
        }
    }
}