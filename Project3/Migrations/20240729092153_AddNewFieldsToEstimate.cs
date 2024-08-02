using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToEstimate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='claim_details' AND xtype='U')
                BEGIN
                    CREATE TABLE [claim_details] (
                        [id] int NOT NULL IDENTITY,
                        [claim_number] numeric(18,0) NULL,
                        [policy_number] numeric(18,0) NULL,
                        [policy_start_date] date NULL,
                        [policy_end_date] date NULL,
                        [customer_name] nvarchar(max) NULL,
                        [place_of_accident] nvarchar(max) NULL,
                        [date_of_accident] nvarchar(max) NULL,
                        [insured_amount] numeric(18,0) NULL,
                        [claimable_amount] numeric(18,0) NULL,
                        CONSTRAINT [PK_claim_details] PRIMARY KEY ([id])
                    );
                END
            ");
            //newly added
            migrationBuilder.AddColumn<string>(
                name: "VehicleVersion",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleBodyNumber",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleEngineNumber",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VehicleNumber",
                table: "estimate",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriverAge",
                table: "estimate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DriverGender",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "AntiTheftDevice",
                table: "estimate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SelectedCoverages",
                table: "estimate",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DrivingHistory",
                table: "estimate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MultiPolicy",
                table: "estimate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SafeDriver",
                table: "estimate",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedCost",
                table: "estimate",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='claim_details' AND xtype='U')
                BEGIN
                    CREATE TABLE [claim_details] (
                        [id] int NOT NULL IDENTITY,
                        [claim_number] numeric(18,0) NULL,
                        [policy_number] numeric(18,0) NULL,
                        [policy_start_date] date NULL,
                        [policy_end_date] date NULL,
                        [customer_name] nvarchar(max) NULL,
                        [place_of_accident] nvarchar(max) NULL,
                        [date_of_accident] nvarchar(max) NULL,
                        [insured_amount] numeric(18,0) NULL,
                        [claimable_amount] numeric(18,0) NULL,
                        CONSTRAINT [PK_claim_details] PRIMARY KEY ([id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='company_expenses' AND xtype='U')
                BEGIN
                    CREATE TABLE [company_expenses] (
                        [id] int NOT NULL IDENTITY,
                        [date_of_expenses] nvarchar(max) NULL,
                        [type_of_expense] nvarchar(max) NULL,
                        [amount_of_expense] numeric(18,0) NULL,
                        CONSTRAINT [PK_company_expenses] PRIMARY KEY ([id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='estimate' AND xtype='U')
                BEGIN
                    CREATE TABLE [estimate] (
                        [id] int NOT NULL IDENTITY,
                        [customer_id] int NULL,
                        [estimate_number] numeric(18,0) NULL,
                        [customer_name] nvarchar(max) NULL,
                        [customer_phone_number] numeric(18,0) NULL,
                        [vehicle_name] nvarchar(max) NULL,
                        [vehicle_model] nvarchar(max) NULL,
                        [VehicleVersion] nvarchar(max) NULL,
                        [vehicle_rate] numeric(18,0) NULL,
                        [VehicleBodyNumber] nvarchar(max) NULL,
                        [VehicleEngineNumber] nvarchar(max) NULL,
                        [VehicleNumber] decimal(18,2) NULL,
                        [vehicle_warranty] nvarchar(max) NULL,
                        [vehicle_policy_type] nvarchar(max) NULL,
                        [DriverAge] int NOT NULL,
                        [DriverGender] nvarchar(max) NOT NULL,
                        [Location] nvarchar(max) NOT NULL,
                        [Usage] nvarchar(max) NOT NULL,
                        [AntiTheftDevice] bit NOT NULL,
                        [SelectedCoverages] nvarchar(max) NOT NULL,
                        [DrivingHistory] int NOT NULL,
                        [MultiPolicy] bit NOT NULL,
                        [SafeDriver] bit NOT NULL,
                        [EstimatedCost] decimal(18,2) NOT NULL,
                        CONSTRAINT [PK_estimate] PRIMARY KEY ([id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='company_billing_policy' AND xtype='U')
                BEGIN
                    CREATE TABLE [company_billing_policy] (
                        [id] int NOT NULL IDENTITY,
                        [customer_id] int NULL,
                        [customer_name] nvarchar(max) NULL,
                        [policy_number] numeric(18,0) NULL,
                        [customer_add_prove] nvarchar(max) NULL,
                        [customer_phone_number] numeric(18,0) NULL,
                        [bill_no] numeric(18,0) NULL,
                        [vehicle_name] nvarchar(max) NULL,
                        [vehicle_model] nvarchar(max) NULL,
                        [vehicle_rate] numeric(18,0) NULL,
                        [vehicle_body_number] nvarchar(max) NULL,
                        [vehicle_engine_number] nvarchar(max) NULL,
                        [date] datetime NULL,
                        [amount] numeric(18,0) NULL,
                        CONSTRAINT [PK_company_billing_policy] PRIMARY KEY ([id])
                    );
                END
            ");
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='insurance_process' AND xtype='U')
                BEGIN
                    CREATE TABLE [insurance_process] (
                        [id] int NOT NULL IDENTITY,
                        [customer_id] int NULL,
                        [customer_name] nvarchar(max) NULL,
                        [customer_add] nvarchar(max) NULL,
                        [customer_phone_number] numeric(18,0) NULL,
                        [policy_number] numeric(18,0) NULL,
                        [policy_date] nvarchar(max) NULL,
                        [policy_duration] numeric(18,0) NULL,
                        [vehicle_number] numeric(18,0) NULL,
                        [vehicle_name] nvarchar(max) NULL,
                        [vehicle_model] nvarchar(max) NULL,
                        [vehicle_version] nvarchar(max) NULL,
                        [vehicle_rate] numeric(18,0) NULL,
                        [vehicle_warranty] nvarchar(max) NULL,
                        [vehicle_body_number] nvarchar(max) NULL,
                        [vehicle_engine_number] nvarchar(max) NULL,
                        [customer_add_prove] nvarchar(max) NULL,
                        CONSTRAINT [PK_insurance_process] PRIMARY KEY ([id])
                    );
                END
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='vehicle_information' AND xtype='U')
                BEGIN
                    CREATE TABLE [vehicle_information] (
                        [id] int NOT NULL IDENTITY,
                        [vehicle_name] nvarchar(max) NULL,
                        [vehicle_owner_name] nvarchar(max) NULL,
                        [vehicle_model] nvarchar(max) NULL,
                        [vehicle_version] nvarchar(max) NULL,
                        [vehicle_rate] numeric(18,0) NULL,
                        [vehicle_body_number] nvarchar(max) NULL,
                        [vehicle_engine_number] nvarchar(max) NULL,
                        [vehicle_number] numeric(18,0) NULL,
                        [Location] nvarchar(max) NULL,
                        [Usage] nvarchar(max) NULL,
                        [WarrantyType] nvarchar(max) NULL,
                        [PolicyType] nvarchar(max) NULL,
                        CONSTRAINT [PK_vehicle_information] PRIMARY KEY ([id])
                    );
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "claim_details");

            migrationBuilder.DropTable(
                name: "company_billing_policy");

            migrationBuilder.DropTable(
                name: "company_expenses");

            migrationBuilder.DropTable(
                name: "estimate");

            migrationBuilder.DropTable(
                name: "insurance_process");

            migrationBuilder.DropTable(
                name: "vehicle_information");
            //new add
            migrationBuilder.DropTable(
                name: "claim_details");

            migrationBuilder.DropColumn(
                name: "VehicleVersion",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "VehicleBodyNumber",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "VehicleEngineNumber",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "VehicleNumber",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "DriverAge",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "DriverGender",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "AntiTheftDevice",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "SelectedCoverages",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "DrivingHistory",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "MultiPolicy",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "SafeDriver",
                table: "estimate");

            migrationBuilder.DropColumn(
                name: "EstimatedCost",
                table: "estimate");
        }
    }
}
