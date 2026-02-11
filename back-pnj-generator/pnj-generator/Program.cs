using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.Interfaces.Services;
using pnj_generator.Services;

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
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres")
    );
});

builder.Services.AddScoped<IIdentityService, IdentityService>();

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

