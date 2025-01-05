using API.Data;
using API.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<MessageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MessageDbConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMessageRepository,MessageRepository>();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
var app = builder.Build();
//app.MapGet("/", () => "Hello World!");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        options.RoutePrefix = string.Empty; // Swagger will be available at the app's root URL
    });
}


//app.MapGet("/", () => "Hello World!");
app.MapControllers();
app.Run();
