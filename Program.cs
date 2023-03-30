using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(d =>
{
    d.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gerenciador_de_Departamentos.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Rafael",
            Email = "rafaelnuttimaldonado@gmail.com"
        }
    });

    var xmlFile = "dep-manager-maven.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    d.IncludeXmlComments(xmlPath);
});

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
