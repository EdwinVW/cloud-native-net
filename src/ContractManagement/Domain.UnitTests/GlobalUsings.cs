global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using FluentAssertions;
global using Moq;

global using Domain.Common;

global using ContractManagement.Domain.Aggregates.Contract;
global using ContractManagement.Domain.Aggregates.Contract.Commands;
global using ContractManagement.Domain.Aggregates.Contract.DomainEvents;
global using ContractManagement.Domain.Aggregates.Contract.Enums;

global using ContractManagement.Domain.Services;

global using Domain.UnitTests.Mocks;
global using Domain.UnitTests.TestDataBuilders.Events;
global using Domain.UnitTests.TestDataBuilders.Commands;
