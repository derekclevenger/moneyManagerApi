using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace api.Model
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // Specify the path of the database here
        //    //optionsBuilder.UseSqlite("Filename=./figuringOut.db");
        //    optionsBuilder.UseSqlServer("name=connectionString");
        //}

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApplicationDbContextExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApplicationDbContext>();
        }
    }
}
