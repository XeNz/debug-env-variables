using Oakton;
using Oakton.Environment;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ApplyOaktonExtensions();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.CheckEnvironment<IConfiguration>("Can connect to the application database",
    config => throw new InvalidOperationException("Intentional fail here"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


return await app.RunOaktonCommands(args);