using StudentConnect.API.src.Extensions;
using StudentConnect.API.src.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services
    .ConfigureMsSqlServer(builder.Configuration)
                .ConfigureBusinessServices()
                .ConfigureIdentity(builder.Configuration)
                .ConfigureJwtAuthentication(builder.Configuration)
                .ConfigureBusinessServices();



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalErrorHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseAuthorization();
app.UseMiddleware<BanCheckMiddleware>();

app.MapControllers();

app.Run();
