global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using FluentAssertions;
global using Moq;

global using Domain.Common;

global using ContractManagement.Domain.Aggregates.ContractAggregate;
global using ContractManagement.Domain.Aggregates.ContractAggregate.Commands;
global using ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;
global using ContractManagement.Domain.Aggregates.ContractAggregate.Enums;

global using ContractManagement.Domain.Services;

global using Domain.UnitTests.Mocks;
global using Domain.UnitTests.TestDataBuilders.Events;
global using Domain.UnitTests.TestDataBuilders.Commands;
