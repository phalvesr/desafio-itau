using System.Text.Json.Serialization;
using LetsCodeItau.Dolly.Api.Filters;
using LetsCodeItau.Dolly.Application.Gateways;
using LetsCodeItau.Dolly.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
})
.AddJsonOptions(options =>
{
    var enumConverter = new JsonStringEnumConverter();
    options.JsonSerializerOptions.Converters.Add(enumConverter);
});

builder.Services.AddJwtBearerTokenAuth(builder.Configuration);
builder.Services.AddAuthorization(configure =>
{
    configure.AddPolicy("BASIC", policyConfigure => policyConfigure.RequireRole(""));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddServices(builder.Configuration);

CreateTables(builder);

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

void CreateTables(WebApplicationBuilder builder)
{
    var scope = builder.Services.BuildServiceProvider().CreateScope();
    var migration = scope.ServiceProvider.GetRequiredService<IMigrationGateway>();

    migration.EnsureCreation();
}
