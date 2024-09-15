using College_managemnt_system.Interfaces;
using College_managemnt_system.Interfaces.Email;
using College_managemnt_system.models;
using College_managemnt_system.models.EmailModel;
using College_managemnt_system.Repos;
using College_managemnt_system.Repos.Email;
using College_managemnt_system.Repos.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Fail("Authorization header is missing or invalid.");
                return;
            }

            var tokenAsString = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(tokenAsString))
            {
                context.Fail("Invalid JWT token.");
                return;
            }
            var token = handler.ReadJwtToken(tokenAsString);

            int userId;

            string isVerficationToken = token.Claims.FirstOrDefault(c => c.Type == "Verify")?.Value;
            string isChnagePasswordToken = token.Claims.FirstOrDefault(c => c.Type == "changepassword")?.Value;


            if (isVerficationToken != null || isChnagePasswordToken != null)
            {
                userId = int.Parse(token.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value);
            }
            else
            {
                userId = int.Parse(token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value);
            }

            var tokenValidationService = context.HttpContext.RequestServices.GetRequiredService<IBlackListTokensRepo>();
            var iatClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value;
            var issuedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(iatClaim)).UtcDateTime;

            if (await tokenValidationService.IsTokenBlacklisted(userId, issuedAt))
            {

                context.Fail("This token has been invalidated.");
            }
            else
            {
                context.Success();
            }
        }
    };


});

builder.Services.AddMemoryCache();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "127.0.0.1:6379";
});




// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IAssistanceCoursesRepo, AssistanceCoursesRepo>();
builder.Services.AddScoped<ITeachingAssistanceRepo, TeachingAssistanceRepo>();
builder.Services.AddScoped<ICSVParser, CSVParser>();
builder.Services.AddScoped<IRootPremissionsRepo, RootPremissionsRepo>();
builder.Services.AddScoped<IStudetnsDepartmentsRepo, StudetnsDepartmentsRepo>();
builder.Services.AddScoped<ISchedulesGroupsRepo, SchedulesGroupsRepo>();
builder.Services.AddScoped<IStudentRepo, StudentRepo>();
builder.Services.AddScoped<IProfessorsRepo, ProfessorsRepo>();
builder.Services.AddScoped<ICoursesSemestersRepo,CoursesSemestersRepo>();
builder.Services.AddScoped<ISchedulesRepo,SchedulesRepo>();
builder.Services.AddScoped<IGroupsRepo , GroupsRepo>();
builder.Services.AddScoped<IClassroomsRepo , ClassroomsRepo>();
builder.Services.AddScoped<IPrereqsCoursesRepo, PrereqsCoursesRepo>();
builder.Services.AddScoped<ICoursesRepo, CoursesRepo>();
builder.Services.AddScoped<IDepartmentsRepo, DepartmentsRepo>();
builder.Services.AddScoped<ISemstersRepo, SemstersRepo>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<ITokensRepo, TokensRepo>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEmailRepo, EmailRepo>();
builder.Services.AddScoped<IBlackListTokensRepo, BlackListTokensRepo>();
builder.Services.AddSingleton<UtilitiesRepo>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert JWT Token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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
}));

builder.Services.AddDbContext<CollegeDBContext>(op =>
{
    op.UseSqlServer(builder.Configuration["ConnectionStrings:default"]);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials(); // Allow credentials (cookies, authorization headers, etc.)
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(C => C.InjectJavascript("/SwaggerJs/autologin.js"));
}

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
