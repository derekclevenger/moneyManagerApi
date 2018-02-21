using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols;
using api.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace api
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


            //var connection = @"Server=(localdb)\mssqllocaldb;Database=EFGetStarted.AspNetCore.NewDb;Trusted_Connection=True;ConnectRetryCount=0";
            ////var connection = @"Server=;Database=cit-416;UserID=root;Password=bObsBaby!2;ConnectRetryCount=0";
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));

            services.AddMvc();


            //the below works trying to make connection strings
            //var connection = @"server=cit-416.cvcozdifo215.us-east-2.rds.amazonaws.com;Initial Catalog=cit416;MultipleActiveResultSets=true;Integrated Security=False;User ID=root;Password=bObsBaby!2";
            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
           
            //Not working for the moment.
            var connectionString = Configuration["DbContextSettings:connectionString"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
            });

            //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("name=connectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });


            app.UseMvc();
        }
    }
}

