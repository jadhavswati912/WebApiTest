using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WebApiTest;
using WebApiTest.Context;
using WebApiTest.Interface;
using WebApiTest.Services;

var builder = WebApplication.CreateBuilder(args);
var startup=new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);


// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddControllers().AddNewtonsoftJson();
//builder.Services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<IProdut,ProductServicecs>();//dependency injection DI
//builder.Services.AddScoped<ICustomer1,CustomerServices>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();
startup.Configure(app);


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwaggerUI(Options =>
//    {
//        Options.SwaggerEndpoint("/openapi/v1.json", "Demo Api");

//    });
    
//     app.MapScalarApiReference();
     
//}

//app.UseHttpsRedirection(); //there as a four middleware//built in middelware nd costome middelware

//app.UseAuthorization();//middleware is small piece of code its handeling request and responce

//app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
