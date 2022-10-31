global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Design;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.Logging;
global using System.Reflection;

global using Application.Common.Interfaces;

global using ContractManagement.Application.Interfaces;
global using ContractManagement.Application.EventHandlers.IntegrationEvents;
global using CustomerManagement.Application.IntegrationEvents;

global using ContractManagement.Domain.Aggregates.Contract;
global using ContractManagement.Domain.Aggregates.Contract.DomainEvents;

global using ContractManagement.Domain.Aggregates.Account;

global using ContractManagement.Infrastructure.Persistence.EFCore;
global using ContractManagement.Infrastructure.Persistence.EFCore.Repositories;

global using Contractmanagement.Features.ContractCancellation;
global using Contractmanagement.Features.ContractMaintenance;
global using Contractmanagement.Features.ContractRegistration;

global using Infrastructure.Common.Messaging.MassTransit;
global using Infrastructure.Common.UnitOfWork;

global using Infrastructure.Common.Persistence.EFCore.Repositories.Aggregate;
global using Infrastructure.Common.Persistence.EFCore.Configurations;

global using ProductManagement.Application.IntegrationEvents;
