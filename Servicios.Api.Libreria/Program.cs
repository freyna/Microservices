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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
