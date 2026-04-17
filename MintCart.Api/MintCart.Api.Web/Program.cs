using MintCart.Configuration;
using MintCart.Api.Core;
using MintCart.Api.Customer.Business.Interface;
using MintCart.Api.Customer.Business.Interactor;
using MintCart.Api.Customer.Domain.Interfaces.Customer;
using MintCart.Api.Customer.Data.Repository.Customer;
using MintCart.Api.Customer.Data;
using MintCart.Api.Dashboard.Business.Interface;
using MintCart.Api.Dashboard.Business.Interactor;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    MintCartConfigurationBuilder.BuildConfiguration(config, builder.Environment.EnvironmentName);
});

// Configure Core Services
builder.ConfigureCoreMintCartService();

// Register Customer Module Specific Services
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString")));

builder.Services.AddScoped<ICustomerInteractor, CustomerInteractor>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Register Dashboard Module Specific Services
builder.Services.AddScoped<IDashboardInteractor, DashboardInteractor>();

var app = builder.Build();

// Configure Core Middleware
app.ConfigureMintCartCore();

app.Run();
