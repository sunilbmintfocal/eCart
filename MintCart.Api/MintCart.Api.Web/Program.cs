using MintCart.Configuration;
using MintCart.Api.Core;
using MintCart.Api.Customer.Business.Interface;
using MintCart.Api.Customer.Business.Interactor;
using MintCart.Api.Customer.Domain.Interfaces.Customer;
using MintCart.Api.Customer.Data.Repository.Customer;
using MintCart.Api.Customer.Data;
using MintCart.Api.Dashboard.Business.Interface;
using MintCart.Api.Dashboard.Business.Interactor;
using MintCart.Api.Data.Sale;
using MintCart.Api.Domain.Sale.Interfaces;
using MintCart.Api.Data.Sale.Repository;
using MintCart.Api.Domain.Inventory.Interfaces;
using MintCart.Api.Data.Inventory;
using MintCart.Api.Data.Inventory.Repository;
using MintCart.Api.Business.Inventory.Interface;
using MintCart.Api.Business.Inventory.Interactor;
using MintCart.Api.Business.Reports.Interface;
using MintCart.Api.Business.Reports.Interactor;
using MintCart.Api.Domain.Reports.Interfaces;
using MintCart.Api.Data.Reports;
using MintCart.Api.Data.Reports.Repository;
using MintCart.Api.Domain.Complaint.Interfaces;
using MintCart.Api.Data.Complaint;
using MintCart.Api.Data.Complaint.Repository;
using MintCart.Api.Domain.Recharge.Interfaces;
using MintCart.Api.Data.Recharge;
using MintCart.Api.Data.Recharge.Repository;

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
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"), sqlOptions => sqlOptions.CommandTimeout(60)));

builder.Services.AddScoped<ICustomerInteractor, CustomerInteractor>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// Register Dashboard Module Specific Services
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IDashboardInteractor, DashboardInteractor>();

// Register Sale Module Specific Services
builder.Services.AddDbContext<SaleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"), sqlOptions => sqlOptions.CommandTimeout(60)));

// Register Inventory Module Specific Services
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"), sqlOptions => sqlOptions.CommandTimeout(60)));

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryInteractor, InventoryInteractor>();

// Register Complaint Module Specific Services
builder.Services.AddDbContext<ComplaintDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"), sqlOptions => sqlOptions.CommandTimeout(60)));

builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();

// Register Recharge Module Specific Services
builder.Services.AddDbContext<RechargeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"), sqlOptions => sqlOptions.CommandTimeout(60)));

builder.Services.AddScoped<IRechargeRepository, RechargeRepository>();

// Register Report Module Specific Services
builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetValue<string>("ConnectionString"), sqlOptions => sqlOptions.CommandTimeout(60)));

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportInteractor, ReportInteractor>();

var app = builder.Build();

// Configure Core Middleware
app.ConfigureMintCartCore();

app.Run();
