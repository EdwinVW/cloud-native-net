global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.EntityFrameworkCore;
global using System.Net;
global using System.Text.Json.Serialization;

global using FluentValidation.AspNetCore;

global using Application.Common.Interfaces;
global using Application.Common.Exceptions;

global using ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

global using ContractManagement.Infrastructure.Persistence.EFCore;

global using Domain.Common.Exceptions;

global using ContractManagement.WebApi.Filters;