using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthCalendar.Models;
using SQLitePCL;

namespace HealthCalendar.DAL
{
    public class AuthDbContext : IdentityDbContext<User>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Ensures User-Table has the name User so it's the same as in the Migrations-files
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User");
        }
    }
}