using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using api.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Web.Http.Cors;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            var connectionString = Configuration["DbContextSettings:connectionString"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                     ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
             });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                                  builder =>
                                 {
                                     builder.AllowAnyOrigin();
                                     builder.AllowAnyHeader();
                                     builder.AllowAnyMethod();

                                 });
            });

            services.AddMvc();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });

            app.UseCors(builder =>
             {
                builder.AllowAnyOrigin();
                 builder.AllowAnyHeader();
                 builder.AllowAnyMethod();
             });

            app.UseMvc();
        }
    }
}

