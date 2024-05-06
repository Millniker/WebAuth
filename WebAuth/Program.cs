using Microsoft.EntityFrameworkCore;
using Pathnostics.Web.Services;
using Pathnostics.Web.Services.Interface;
using WebAuth.Data;
using WebAuth.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var connection = builder.Configuration.GetConnectionString("Db");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});builder.Services.AddScoped<ITokenService, JwtService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<MetricMiddleware>();
Configurator.Migrate(app.Services);
app.Run();
