using WebApiTest.Context;
using WebApiTest.Interface;
using WebApiTest.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using Product_Details.Model;
using Microsoft.Extensions.Configuration;
using Product_Details.Interface;
using Product_Details.Services;
using DotNet8WebAPI.Helpers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
namespace WebApiTest
{
    public class Startup
    {
        //private object configuration;

        public IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
                        
            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));
            services.AddControllers();
            //services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<IProdut, ProductServicecs>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<TokenService>();
            services.AddOpenApi();

            
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation 
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = ".NET 8 Web API"
                });
                // To Enable authorization using Swagger (JWT) 
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. r\n\r\n Enter 'Bearer'[space] and then your token in the text input               below.\r\n\r\n Example: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                        new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                    }
                                },
                            new string[] {}
                            }
                        });
            });

        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });              
            }


            app.UseHttpsRedirection(); //there as a four middleware//built in middelware nd costome middelware
            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>(); //middleware is small piece of code its handeling request and responce

            app.MapControllers();
                  
        }
    }
}
