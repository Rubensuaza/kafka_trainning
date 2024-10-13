using io.quind.kafka.trainning.api.configurations;
using io.quind.kafka.trainning.repository.config;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region injection dependecies configuration
DependencyInjectionConf.DependencyInjectionConfServices(builder.Services);
#endregion

#region kafka configuration
//var kafkaConfig = new KafkaConfig();
//builder.Services.AddSingleton(kafkaConfig);
#endregion

#region postgresql configuration
var connectionStrring = builder.Configuration.GetConnectionString("PostgresqlConnection");
builder.Services.AddDbContext<DatabaseContext>(options => 
{
    options.UseNpgsql(connectionStrring);
});
#endregion



// Add services to the container.

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();