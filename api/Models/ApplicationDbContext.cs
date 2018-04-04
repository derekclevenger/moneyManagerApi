using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace api.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Categories> Category { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budget { get; set; }

    }
}
