using RightMove.Db;
using RightMove.Db.Repositories;
using RightMove.Db.Services;
using RightMove.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services
	.AddTransient<IRightMovePropertyRepository, RightMovePropertyRepository>()
	.AddSingleton<IDbConfiguration>(o => new DbConfiguration(settings.DbPath))
	.AddTransient<IDatabaseService, DatabaseService>();

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

app.Run();
