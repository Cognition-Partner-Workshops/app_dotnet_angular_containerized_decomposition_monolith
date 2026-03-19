using Microsoft.EntityFrameworkCore;
using CustomerMicroservice.Infrastructure;
using CustomerMicroservice.Models;
using CustomerMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

/************* ADD SERVICES *************/

var connectionString = builder.Configuration.GetConnectionString("CustomerConnection") ??
                throw new InvalidOperationException("Connection string 'CustomerConnection' not found.");

builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add CORS
builder.Services.AddCors();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Business Services
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

/************* CONFIGURE REQUEST PIPELINE *************/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.MapControllers();

/************* SEED DATABASE *************/

using var scope = app.Services.CreateScope();
try
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    await dbContext.Database.MigrateAsync();

    if (!await dbContext.Customers.AnyAsync())
    {
        dbContext.Customers.Add(new Customer
        {
            Name = "Ebenezer Monney",
            Email = "contact@ebenmonney.com",
            Gender = Gender.Male
        });

        dbContext.Customers.Add(new Customer
        {
            Name = "Itachi Uchiha",
            Email = "uchiha@narutoverse.com",
            PhoneNumber = "+81123456789",
            Address = "Some fictional Address, Street 123, Konoha",
            City = "Konoha",
            Gender = Gender.Male
        });

        dbContext.Customers.Add(new Customer
        {
            Name = "John Doe",
            Email = "johndoe@anonymous.com",
            PhoneNumber = "+18585858",
            Address = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio.
                    Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at elementum imperdiet",
            City = "Lorem Ipsum",
            Gender = Gender.Male
        });

        dbContext.Customers.Add(new Customer
        {
            Name = "Jane Doe",
            Email = "Janedoe@anonymous.com",
            PhoneNumber = "+18585858",
            Address = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio.
                    Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at elementum imperdiet",
            City = "Lorem Ipsum",
            Gender = Gender.Male
        });

        await dbContext.SaveChangesAsync();
    }
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "An error occurred whilst creating/seeding database");
}

/************* RUN APP *************/

app.Run();
