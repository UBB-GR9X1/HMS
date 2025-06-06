using HMS.Backend.Data;
using Microsoft.EntityFrameworkCore;
using HMS.Backend.Repositories.Interfaces;
using HMS.Backend.Repositories;
using HMS.Backend.Repositories.Implementations;
using HMS.Backend.Extensions;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Load .env variables
DotNetEnv.Env.Load();

// Get DB_HOST, make sure to escape backslash if needed
var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "IF_YOU_DONT_HAVE_.ENV_SETUP_THIS_CRASHES";

// Build the connection string dynamically
var connectionString = $"{dbHost}";
Console.WriteLine($"[DEBUG] Final connection string: {connectionString}");
// Use connection string for DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

// Other services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

string secretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? throw new InvalidOperationException("JWT_SECRET environment variable is not set.");
string issuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new InvalidOperationException("ISSUER environment variable is not set.");
string audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new InvalidOperationException("AUDIENCE environment variable is not set.");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidIssuer = issuer,
            ValidAudience = audience,
            ClockSkew = TimeSpan.Zero,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
