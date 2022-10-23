using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    ContractNumber = table.Column<string>(type: "nchar(18)", fixedLength: true, maxLength: 18, nullable: false),
                    CustomerNumber = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentPeriod = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.ContractNumber);
                });

            migrationBuilder.CreateTable(
                name: "ContractAggregate",
                columns: table => new
                {
                    AggregateId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Version = table.Column<uint>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractAggregate", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "ContractEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Version = table.Column<uint>(type: "bigint", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractEvent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerNumber = table.Column<string>(type: "nchar(6)", fixedLength: true, maxLength: 6, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerNumber);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductNumber = table.Column<string>(type: "nchar(18)", fixedLength: true, maxLength: 18, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductNumber);
                });

            migrationBuilder.InsertData(
                table: "Contract",
                columns: new[] { "ContractNumber", "Amount", "CustomerNumber", "EndDate", "PaymentPeriod", "ProductNumber", "StartDate" },
                values: new object[] { "CTR-20220502-9999", 20000m, "C13976", new DateTime(2034, 5, 2, 15, 40, 35, 877, DateTimeKind.Local), "Monthly", "FAC-00011", new DateTime(2022, 5, 2, 15, 40, 35, 876, DateTimeKind.Local) });

            migrationBuilder.InsertData(
                table: "ContractAggregate",
                columns: new[] { "AggregateId", "Version" },
                values: new object[] { "CTR-20220502-9999", 1u });

            migrationBuilder.InsertData(
                table: "ContractEvent",
                columns: new[] { "Id", "AggregateId", "EventData", "EventType", "Timestamp", "Version" },
                values: new object[] { new Guid("30be1187-9f8a-4c4c-b9d2-9c7bede5d73b"), "CTR-20220502-9999", "{\"ContractNumber\": \"CTR-20220502-9999\",\"CustomerNumber\": \"C13976\",\"ProductNumber\": \"FAC-00011\",\"Amount\": 20000,\"StartDate\": \"2022-05-02T12:40:35.876Z\",\"EndDate\": \"2034-05-02T12:40:35.877Z\",\"EventId\": \"f0074479-4cea-41ff-a669-bdb3649f6e7b\"}", "ContractRegistered", new DateTime(2022, 10, 23, 10, 5, 56, 413, DateTimeKind.Local).AddTicks(3700), 1u });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerNumber", "Address", "EmailAddress", "Name" },
                values: new object[,]
                {
                    { "C13976", "Mainstreet 5463", "jd@example.com", "John Doe" },
                    { "C13977", "First Av. 16743", "eric.dewitt@example.com", "Eric Dewitt" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductNumber", "Description" },
                values: new object[] { "FAC-00011", "Standard long term loan" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "ContractAggregate");

            migrationBuilder.DropTable(
                name: "ContractEvent");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
