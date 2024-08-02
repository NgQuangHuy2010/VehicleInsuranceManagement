using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project3.Models;

public partial class VehicleInsuranceManagementContext : DbContext
{
    public VehicleInsuranceManagementContext()
    {
    }

    public VehicleInsuranceManagementContext(DbContextOptions<VehicleInsuranceManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClaimDetail> ClaimDetails { get; set; }

    public virtual DbSet<CompanyBillingPolicy> CompanyBillingPolicies { get; set; }

    public virtual DbSet<CompanyExpense> CompanyExpenses { get; set; }

    public virtual DbSet<Estimate> Estimates { get; set; }

    public virtual DbSet<InsuranceProcess> InsuranceProcesses { get; set; }

    public virtual DbSet<VehicleInformation> VehicleInformations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-AU9QS0E;Initial Catalog=VehicleInsuranceManagement;Persist Security Info=True;User ID=sa;Password=chuong123;Encrypt=True;Trust Server Certificate=True\n");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClaimDetail>(entity =>
        {
            entity.ToTable("claim_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClaimNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("claim_number");
            entity.Property(e => e.ClaimableAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("claimable_amount");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.DateOfAccident).HasColumnName("date_of_accident");
            entity.Property(e => e.InsuredAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("insured_amount");
            entity.Property(e => e.PlaceOfAccident).HasColumnName("place_of_accident");
            entity.Property(e => e.PolicyEndDate).HasColumnName("policy_end_date");
            entity.Property(e => e.PolicyNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("policy_number");
            entity.Property(e => e.PolicyStartDate).HasColumnName("policy_start_date");
        });

        modelBuilder.Entity<CompanyBillingPolicy>(entity =>
        {
            entity.ToTable("company_billing_policy");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("amount");
            entity.Property(e => e.BillNo)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("bill_no");
            entity.Property(e => e.CustomerAddProve).HasColumnName("customer_add_prove");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhoneNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("customer_phone_number");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.PolicyNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("policy_number");
            entity.Property(e => e.VehicleBodyNumber).HasColumnName("vehicle_body_number");
            entity.Property(e => e.VehicleEngineNumber).HasColumnName("vehicle_engine_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehicleRate)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_rate");
        });

        modelBuilder.Entity<CompanyExpense>(entity =>
        {
            entity.ToTable("company_expenses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AmountOfExpense)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("amount_of_expense");
            entity.Property(e => e.DateOfExpenses).HasColumnName("date_of_expenses");
            entity.Property(e => e.TypeOfExpense).HasColumnName("type_of_expense");
        });

        modelBuilder.Entity<Estimate>(entity =>
        {
            entity.ToTable("estimate");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhoneNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("customer_phone_number");
            entity.Property(e => e.EstimateNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("estimate_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehiclePolicyType).HasColumnName("vehicle_policy_type");
            entity.Property(e => e.VehicleRate)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_rate");
            entity.Property(e => e.VehicleWarranty).HasColumnName("vehicle_warranty");
        });

        modelBuilder.Entity<InsuranceProcess>(entity =>
        {
            entity.ToTable("insurance_process");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerAdd).HasColumnName("customer_add");
            entity.Property(e => e.CustomerAddProve).HasColumnName("customer_add_prove");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhoneNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("customer_phone_number");
            entity.Property(e => e.PolicyDate).HasColumnName("policy_date");
            entity.Property(e => e.PolicyDuration)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("policy_duration");
            entity.Property(e => e.PolicyNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("policy_number");
            entity.Property(e => e.VehicleBodyNumber).HasColumnName("vehicle_body_number");
            entity.Property(e => e.VehicleEngineNumber).HasColumnName("vehicle_engine_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehicleNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_number");
            entity.Property(e => e.VehicleRate)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_rate");
            entity.Property(e => e.VehicleVersion).HasColumnName("vehicle_version");
            entity.Property(e => e.VehicleWarranty).HasColumnName("vehicle_warranty");
        });

        modelBuilder.Entity<VehicleInformation>(entity =>
        {
            entity.ToTable("vehicle_information");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.VehicleBodyNumber).HasColumnName("vehicle_body_number");
            entity.Property(e => e.VehicleEngineNumber).HasColumnName("vehicle_engine_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehicleNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_number");
            entity.Property(e => e.VehicleOwnerName).HasColumnName("vehicle_owner_name");
            entity.Property(e => e.VehicleRate)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_rate");
            entity.Property(e => e.VehicleVersion).HasColumnName("vehicle_version");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
