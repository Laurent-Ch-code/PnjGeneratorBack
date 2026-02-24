using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.Interfaces.Services;
using pnj_generator.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ✅ Services (avant Build)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://pnj-generator.web.app",
                "https://pnj-generator.firebaseapp.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<INPCGeneratorService, NPCGeneratorService>();

// ✅ Build après avoir enregistré les services
var app = builder.Build();

// ✅ Pipeline (après Build)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS doit être avant MapControllers (et avant Authorization si tu en as)
app.UseCors("AngularDev");

app.UseAuthorization();
app.MapControllers();

app.Run();

