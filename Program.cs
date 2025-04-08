using Microsoft.EntityFrameworkCore;
using Studenti.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<FakultetContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentiCS"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", policy =>
{
    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .WithOrigins(
              "http://127.0.0.1:5288",
              "https://127.0.0.1:5288",
              "https://localhost:5288",
              "http://localhost:5288",
              "https://127.0.0.1:5500",
              "http://127.0.0.1:5500",
              "http://localhost:5500"
          );
});

});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CORS");



app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();


