using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonApi.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<PersonContext>(opt =>
    opt.UseInMemoryDatabase("PersonList"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


// Load data from the file into the database
LoadPersonsFromFile(app.Services).Wait();

// Run application
app.Run();


// Function to read out data stored in file
async Task LoadPersonsFromFile(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PersonContext>();

    var filePath = "PersonList.json";
    if (File.Exists(filePath))  // Check if file exist
    {
        // Read data out file: "PersonList.json"
        var jsonContent = await File.ReadAllTextAsync(filePath);
        // Save data as List<Person>
        var thepersons = JsonSerializer.Deserialize<List<Person>>(jsonContent);
        if (thepersons != null)
        {
            /* 
            foreach (var person in thepersons)
            {
                context.Persons.Add(person);
                foreach (var account in person.Accounts)
                {
                    context.Accounts.Add(account);
                }
            }
            */

            // Add read out data to dbcontext
            context.Persons.AddRange(thepersons);
            await context.SaveChangesAsync();
        }
    }
}

