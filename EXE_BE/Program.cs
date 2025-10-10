using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Net.payOS;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Register PostgreSQL DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository and service
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<ChallengeProgressRepository>();
builder.Services.AddScoped<ChallengeProgressService>();

builder.Services.AddScoped<ChallengeRepository>();
builder.Services.AddScoped<ChallengeService>();

builder.Services.AddScoped<FoodUsageService>();
builder.Services.AddScoped<FoodUsageRepository>();

builder.Services.AddScoped<FoodItemRepository>();
builder.Services.AddScoped<PlasticItemRepository>();
builder.Services.AddScoped<CategorySelect>();

builder.Services.AddScoped<EnergyUsageRepository>();
builder.Services.AddScoped<EnergyUsageService>();

builder.Services.AddScoped<PlasticUsageRepository>();
builder.Services.AddScoped<PlasticUsageService>();

builder.Services.AddScoped<TrafficUsageRepository>();
builder.Services.AddScoped<TrafficUsageService>();

builder.Services.AddScoped<UserActivitiesSerivce>();
builder.Services.AddScoped<UserActivitiesRepository>();

builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<TransactionService>();

builder.Services.AddScoped<NotifyRepository>();
builder.Services.AddScoped<NotifyService>();

builder.Services.AddScoped<RecommendService>();
builder.Services.AddScoped<PayOS>(payOs =>
{
    var config = payOs.GetRequiredService<IConfiguration>();
    return new PayOS(config["PayOS:ClientId"], config["PayOS:ApiKey"], config["PayOs:ChecksumKey"]);
});

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "exeapi",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "DefaultKeyForDevelopmentPurposesOnly12345678901234"))
        };
    });
builder.Services.AddAuthorizationBuilder()
            .AddPolicy("admin", policy =>
            {
                policy.RequireRole("Admin");
                //policy.AddAuthenticationSchemes(Access);
                //policy.RequireClaim("TokenType", "access");
            })
            .AddPolicy("staff", policy =>
            {
                policy.RequireRole("Staff");
            })
            .AddPolicy("user", policy =>
            {
                policy.RequireRole("User");
            });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc(
        "v1",
        new OpenApiInfo { Title = "EXE", Version = "v1" }
    );
    o.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        }
    );
    o.AddSecurityRequirement(
        new OpenApiSecurityRequirement
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
                Array.Empty<string>()
            }
        }
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
