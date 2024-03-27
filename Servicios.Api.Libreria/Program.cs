using Servicios.Api.Datos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(options =>
{
    options.ConnectionString = new ConfigurationManager().GetSection("MongoDB").GetSection("ConnectionString").Value;
    options.Database = new ConfigurationManager().GetSection("MongoDB:DataBase").Value;

});

// Add services to the container.
builder.Services.AddSingleton<MongoSettings>();

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
