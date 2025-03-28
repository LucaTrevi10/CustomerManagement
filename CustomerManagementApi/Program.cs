using CustomerManagementApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CustomerManagementApi.Data.Dtos;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Aggiungi CORS con 'AllowAnyOrigin', che permette a qualsiasi origine di fare richieste
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()    // Permette a qualsiasi dominio di fare richieste
              .AllowAnyMethod()    // Permette qualsiasi metodo HTTP (GET, POST, etc.)
              .AllowAnyHeader();   // Permette qualsiasi header
    });
});

// Registrare AutoMapper e i suoi profili
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Add services to the container.
// Aggiungi i servizi per il DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Usa la policy CORS "AllowAllOrigins"
app.UseCors("AllowAllOrigins");

app.Run();
