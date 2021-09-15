using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using PlatformService.Models;
using Microsoft.EntityFrameworkCore;
namespace PlatformService.Data{

    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd){
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void seedData(AppDbContext context, bool isProd)
        {

            if(isProd){
                Console.WriteLine("--> Attempting to apply migrations");
                try{
                    context.Database.Migrate();
                }
                catch(Exception ex){
                    Console.WriteLine($"--> Could not run migrations : {ex.Message}");
                }
            }
            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data");
                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost ="Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost ="Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost ="Free" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We alreadt have data");
            }

        }
    }
}