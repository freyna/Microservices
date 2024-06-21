using Servicios.Api.BusinessRules.Autores;
using Servicios.Api.Datos;
using Servicios.Api.Datos.ContextMongoDB;
using Servicios.Api.Datos.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(options =>
{
    options.ConnectionString = builder.Configuration.GetSection("MongoDB").GetSection("ConnectionString").Value;
    options.Database = builder.Configuration.GetSection("MongoDB:DataBase").Value;
});

// Add services to the container.
builder.Services.AddSingleton<MongoSettings>();

builder.Services.AddTransient<IAutorContext, AutorContext>();
builder.Services.AddTransient<IAutorRepository, AutorRepository>();
builder.Services.AddTransient<IAutoresBusinessRules, AutoresBusinessRules>();
builder.Services.AddScoped(typeof(IMongoRepository<>),typeof(MongoRepository<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//Indicating the app can use the rule created before.
app.UseCors("CorsRule");

app.Run();
