using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Servicios.Api.Seguridad.Core.Application;
using Servicios.Api.Seguridad.Core.Entities;
using Servicios.Api.Seguridad.Core.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();
app.MapIdentityApi<Usuario>();
app.Run();
