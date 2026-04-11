using MintCart.Configuration;
using MintCart.Api.Core;
using MintCart.Api.Customer.Business.Interface;
using MintCart.Api.Customer.Business.Interactor;
using MintCart.Api.Customer.Domain.Interfaces.Customer;
using MintCart.Api.Customer.Data.Repository.Customer;
using Microsoft.EntityFrameworkCore;
using MintCart.Api.Customer.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
	MintCartConfigurationBuilder.BuildConfiguration(config, builder.Environment.EnvironmentName);
});

// Configure Core Services (Logger, Authentication, Authorization, Swagger, etc.)
builder.ConfigureCoreMintCartService();

// Register Customer Module Specific Services
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString")));

builder.Services.AddScoped<ICustomerInteractor, CustomerInteractor>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

// Configure Core Middleware (Authentication, Authorization, Swagger UI, etc.)
app.ConfigureMintCartCore();

app.Run();
