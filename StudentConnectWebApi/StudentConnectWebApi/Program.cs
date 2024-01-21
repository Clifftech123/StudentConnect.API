using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentConnectWebApi.models;
using StudentConnectWebApi.src.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//  Add Authentication
builder.Services.AddAuthentication()
  .AddBearerToken(IdentityConstants.BearerScheme);


// Add authorization
builder.Services.AddAuthorizationBuilder();



// config db context
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=appdb;Trusted_Connection=True;")
);


builder.Services.AddIdentityCore<AppUser>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();




var app = builder.Build();

app.MapIdentityApi<AppUser>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
