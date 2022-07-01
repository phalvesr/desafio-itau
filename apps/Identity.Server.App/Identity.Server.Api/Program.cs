using Identity.Server.Api.Filters;
using Identity.Server.Domain.Gateways;
using Identity.Server.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
}).AddValidations();

builder.Services.AddAuthorization();
builder.Services.AddJwtBearerTokenAuth(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServicesConfiguration(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await AssurDatabaseCreation(app);

app.Run();

async Task AssurDatabaseCreation(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var migrationRepository = scope.ServiceProvider.GetRequiredService<IDataMigrationRepository>();

    await migrationRepository.MigrateDataAsync();
}
