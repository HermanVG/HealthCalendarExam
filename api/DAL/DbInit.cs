using Microsoft.EntityFrameworkCore;
namespace HealthCalendar.DAL;

using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;

public static class DbInit
{
    // Seeds users to Db
    public static async Task DbSeed(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        /* ----- Seeding User table: ----- */

        // a UserManager, used to add Users securely
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        // an AuthDbContext, used to check if User table is empty
        AuthDbContext context = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

        // Only seeds if User table is empty
        if (!context.Users.Any())
        {
            var admin = new User
            {
                Name = "Admin",
                UserName = "admin@admin.ad",
                // Password = "Aaaa4@"
                Role = Roles.Admin
            };
            await addUser(userManager, admin, "Aaaa4@");

            var worker1 = new User
            {
                Name = "aaa",
                UserName = "aaa@aaa.aaa",
                // Password = "Aaaa4@"
                Role = Roles.Worker
            };
            await addUser(userManager, worker1, "Aaaa4@");

            var patient1 = new User
            {
                Name = "bbb",
                UserName = "bbb@bbb.bbb",
                // Password = "Aaaa4@"
                Role = Roles.Patient,
                WorkerId = worker1.Id,
                Worker = worker1
            };
            await addUser(userManager, patient1, "Aaaa4@");

            var patient2 = new User
            {
                Name = "ccc",
                UserName = "ccc@ccc.ccc",
                // Password = "Aaaa4@"
                Role = Roles.Patient,
                WorkerId = worker1.Id,
                Worker = worker1
            };
            await addUser(userManager, patient2, "Aaaa4@");
        }

        /* ----- Seeding Other bs: ----- */
        
    }
    
    // adds User to User table
    private static async Task addUser(UserManager<User> userManager, User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        
        // in case of not succeeding, errors are printed
        if (!result.Succeeded)
        {
            Console.WriteLine("[DbInit], something went wrong when adding " +
                             $"user {@user} to User table: \n");
            Console.WriteLine(String.Join("\n",result.Errors));
        }
    }
}