using Microsoft.EntityFrameworkCore;
using RightMove.Db;
using RightMove.Db.Entities;
using RightMove.Db.Extensions;
using RightMove.Db.Repositories;
using RightMove.Db.Services;
using RightMove.Web;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200");
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.GetSection("Settings").Get<Settings>();

builder.Services
	.AddTransient<IRightMovePropertyRepository<RightMovePropertyEntity>, RightMovePropertyEFRepository>()
	.AddSingleton<IDbConfiguration>(o => new DbConfiguration(settings.DbPath));
builder.Services.RegisterRightMoveDb();

var envVar = Environment.GetEnvironmentVariable("ConnectionString");

var connectionString = !string.IsNullOrEmpty(envVar)
	? envVar
	//: builder.Configuration.GetSection("ConnectionStrings:MariaDb").Value;
	: builder.Configuration.GetSection("ConnectionStrings:PostGre").Value;

//builder.Services.AddDbContext<RightMoveContext>(
//	options => options.UseMySql(connectionString,
//		new MariaDbServerVersion(new Version(10, 3, 39))));

builder.Services.AddDbContext<RightMoveContext>(
	options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
