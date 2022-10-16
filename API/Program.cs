using API.Contexts;
using API.Infrastructure.BackGroundTasks;
using API.Infrastructure.Caching;
using API.Infrastructure.Hubs;
using API.Infrastructure.Middlewares;
using API.Repositories;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<DataContext>(p => p.UseSqlServer(configuration.GetConnectionString("DataContext")));
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
builder.Services.AddScoped<ICacheManager, CacheManager>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddHostedService<APIBackGroundService>();

builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });

builder.Services.AddStackExchangeRedisCache(opt => { opt.Configuration = "localhost:6379";});

builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;
    opt.KeepAliveInterval = TimeSpan.FromHours(2);
});

CorsPolicyBuilder cbuilder = new CorsPolicyBuilder().AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
CorsPolicy policy = cbuilder.Build();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("MyCors", policy);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("JWTBearer", opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateLifetime = true
    };
    opt.Events = new JwtBearerEvents { OnTokenValidated = OnTokenValidated };
});

static Task OnTokenValidated(TokenValidatedContext context)
{
    var token = context.SecurityToken;
    ClaimsPrincipal? userPrincipal = context?.Principal;
    return Task.FromResult(0);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var supportedCultures = new[] { "en-US", "fr-FR", "ta-IN" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);


app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.UseSessionMiddleware();

app.MapControllers();

app.UseCors("MyCors");

app.MapHub<MessageHub>("/MessageHub");

app.Run();
