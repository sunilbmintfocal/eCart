using MintCart.Configuration;
using MintCart.Api.Core;
using MintCart.Api.Contact.Business.Interface;
using MintCart.Api.Contact.Business.Interactor;
using MintCart.Api.Contact.Domain.Interfaces.Contact;
using MintCart.Api.Contact.Data.Repository.Contact;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
	MintCartConfigurationBuilder.BuildConfiguration(config, builder.Environment.EnvironmentName);
});

// Configure Core Services (Logger, Authentication, Authorization, Swagger, etc.)
builder.ConfigureCoreMintCartService();

// Register Contact Module Specific Services
builder.Services.AddScoped<IContactInteractor, ContactInteractor>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

var app = builder.Build();

// Configure Core Middleware (Authentication, Authorization, Swagger UI, etc.)
app.ConfigureMintCartCore();

app.Run();