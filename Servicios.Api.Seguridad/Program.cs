using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servicios.Api.Seguridad.Core.Application;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.Persistence;
using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Servicios.Api.Seguridad.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SeguridadContexto>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConectionDB"));
});

builder.Services.AddIdentityCore<Usuario>()
    .AddEntityFrameworkStores<SeguridadContexto>()
    .AddApiEndpoints();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddAutoMapper(typeof(RegisterHandler));

builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidation>();
builder.Services.AddScoped<IValidator<RegisterCommand>, RegisterValidation>();

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

builder.Services.AddScoped<IUsuarioSesion, UsuarioSesion>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

//Allow to consume the endpoints from any site.
//We can add a rule to set specific sites.
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsRule", rule =>
    {
        rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
    });
});


var app = builder.Build();

using (var context = app.Services.CreateScope())
{
    var services = context.ServiceProvider;

    try
    {
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var contextEF = services.GetRequiredService<SeguridadContexto>();

        SeguridadData.InsertarUsuario(contextEF, userManager).Wait();
    }
    catch (Exception ex)
    {
        var logging = services.GetRequiredService<ILogger<Program>>();
        logging.LogError(ex, "Error al registrar usuario");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<Usuario>();
//Indicating the app can use the rule created before.
app.UseCors("CorsRule");
app.Run();
