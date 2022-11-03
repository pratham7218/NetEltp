using APIApps.CustomOps.CustomMiddlewares;
using APIApps.Models;
using APIApps.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the eShoppingCodeContext in DI COntainer
// Also Provide the ConectionString fpr SQL Server

builder.Services.AddDbContext<eShoppingCodiContext>(options => 
{
    // pass the Connection String
    // using the builder.Configuration.GetConnectionString()
    // this will read the "ConnectionString" section of appsettings.json
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection"));
});

// REgister the Custom Service classes in DI Container

builder.Services.AddScoped<IDbAccessService<Category,int>, CategoryDataAccessService>();
builder.Services.AddScoped<IDbAccessService<Product,int>,ProductDataAccessService>();

// REgister the HTTP Pipeline for API COntrollers
// THis will loo for API Controllers instance and execute it
// AddJsonOptions() an additional Service to manage the Response Formatting
builder.Services.AddControllers().AddJsonOptions(options => {
    // ReSet the JSON Serialization to the format for
    // Property Naming as per provided in ENtity class
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});


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

// USe the Custom MIddleware
app.UseAppExceptionHandler();

// Map the API Controller to the Incomming Request
app.MapControllers();

app.Run();
