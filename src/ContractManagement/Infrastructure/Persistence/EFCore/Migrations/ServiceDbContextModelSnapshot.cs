﻿// <auto-generated />
using System;
using ContractManagement.Infrastructure.Persistence.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.EFCore.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    partial class ServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ContractAggregate", b =>
                {
                    b.Property<string>("AggregateId")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<long>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("bigint");

                    b.HasKey("AggregateId");

                    b.ToTable("ContractAggregate");

                    b.HasData(
                        new
                        {
                            AggregateId = "CTR-20220502-9999",
                            Version = 1L
                        });
                });

            modelBuilder.Entity("ContractEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AggregateId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("EventData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("ContractEvent");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6177ea8b-d5c7-4462-9106-9b73a681a21a"),
                            AggregateId = "CTR-20220502-9999",
                            EventData = "{\"ContractNumber\": \"CTR-20220502-9999\",\"CustomerNumber\": \"C13976\",\"ProductNumber\": \"FAC-00011\",\"Amount\": 20000,\"StartDate\": \"2022-05-02T12:40:35.876Z\",\"EndDate\": \"2034-05-02T12:40:35.877Z\",\"EventId\": \"f0074479-4cea-41ff-a669-bdb3649f6e7b\"}",
                            EventType = "ContractRegistered",
                            Timestamp = new DateTime(2022, 10, 31, 13, 22, 9, 945, DateTimeKind.Local).AddTicks(4420),
                            Version = 1L
                        });
                });

            modelBuilder.Entity("ContractManagement.Application.ReadModels.Contract", b =>
                {
                    b.Property<string>("ContractNumber")
                        .HasMaxLength(18)
                        .HasColumnType("nchar(18)")
                        .IsFixedLength();

                    b.Property<decimal?>("Amount")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("CustomerNumber")
                        .HasMaxLength(6)
                        .HasColumnType("nchar(6)")
                        .IsFixedLength();

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentPeriod")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("ProductNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ContractNumber");

                    b.ToTable("Contract", (string)null);

                    b.HasData(
                        new
                        {
                            ContractNumber = "CTR-20220502-9999",
                            Amount = 20000m,
                            CustomerNumber = "C13976",
                            EndDate = new DateTime(2034, 5, 2, 14, 40, 35, 877, DateTimeKind.Local),
                            PaymentPeriod = "Monthly",
                            ProductNumber = "FAC-00011",
                            StartDate = new DateTime(2022, 5, 2, 14, 40, 35, 876, DateTimeKind.Local)
                        });
                });

            modelBuilder.Entity("ContractManagement.Application.ReadModels.Customer", b =>
                {
                    b.Property<string>("CustomerNumber")
                        .HasMaxLength(6)
                        .HasColumnType("nchar(6)")
                        .IsFixedLength();

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CustomerNumber");

                    b.ToTable("Customer", (string)null);

                    b.HasData(
                        new
                        {
                            CustomerNumber = "C13976",
                            Address = "Mainstreet 5463",
                            EmailAddress = "jd@example.com",
                            Name = "John Doe"
                        },
                        new
                        {
                            CustomerNumber = "C13977",
                            Address = "First Av. 16743",
                            EmailAddress = "eric.dewitt@example.com",
                            Name = "Eric Dewitt"
                        });
                });

            modelBuilder.Entity("ContractManagement.Application.ReadModels.Product", b =>
                {
                    b.Property<string>("ProductNumber")
                        .HasMaxLength(18)
                        .HasColumnType("nchar(18)")
                        .IsFixedLength();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductNumber");

                    b.ToTable("Product", (string)null);

                    b.HasData(
                        new
                        {
                            ProductNumber = "FAC-00011",
                            Description = "Standard long term loan"
                        });
                });

            modelBuilder.Entity("ContractManagement.Domain.Aggregates.Portfolio.Document", b =>
                {
                    b.Property<string>("DocumentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DocumentType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DocumentUrl")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PortfolioId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DocumentId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Document", (string)null);

                    b.HasData(
                        new
                        {
                            DocumentId = "F656C97A-B211-4618-A39F-E14C6CB2D003",
                            DocumentType = "Passport",
                            DocumentUrl = "file://archivesrv01/contracts/CTR-20220502-9999/F656C97A-B211-4618-A39F-E14C6CB2D003.png",
                            PortfolioId = "CTR-20220502-9999"
                        });
                });

            modelBuilder.Entity("ContractManagement.Domain.Aggregates.Portfolio.Portfolio", b =>
                {
                    b.Property<string>("PortfolioId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("PortfolioId");

                    b.ToTable("Portfolio", (string)null);

                    b.HasData(
                        new
                        {
                            PortfolioId = "CTR-20220502-9999",
                            Version = 1L
                        });
                });

            modelBuilder.Entity("ContractManagement.Domain.Aggregates.Portfolio.Document", b =>
                {
                    b.HasOne("ContractManagement.Domain.Aggregates.Portfolio.Portfolio", null)
                        .WithMany("Documents")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContractManagement.Domain.Aggregates.Portfolio.Portfolio", b =>
                {
                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
