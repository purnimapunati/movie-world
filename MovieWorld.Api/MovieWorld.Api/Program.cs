using MovieWorld.Infra;
using MovieWorld.Service;
using MovieWorld.Service.Ioc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddAutoMapper(x =>
{
    x.AddProfile<MoviesMapping>();
}, new List<Type>(), ServiceLifetime.Singleton);

services
    .AddScoped<IApiClientFactory, ApiClientFactory>()
    .AddScoped<IMovieService, MovieService>();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
