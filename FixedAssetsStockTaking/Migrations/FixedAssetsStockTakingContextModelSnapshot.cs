﻿// <auto-generated />
using System;
using FixedAssetsStockTaking.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FixedAssetsStockTaking.Migrations
{
    [DbContext(typeof(FixedAssetsStockTakingContext))]
    partial class FixedAssetsStockTakingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FixedAssetsStockTaking.Models.CostCenter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InventarID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("InventarID");

                    b.ToTable("CostCenters");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.ExtraLine", b =>
                {
                    b.Property<int>("InventarID")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<string>("MFLine")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("InventarID", "MFLine");

                    b.ToTable("ExtraLines");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.Inventar", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("ClosedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("ClosedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.ToTable("Inventar");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.MijlocFix", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AlocatedCenter")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("COD")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FoundAt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FoundBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FoundLine")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("FoundQ")
                        .HasColumnType("int");

                    b.Property<int>("InventarID")
                        .HasColumnType("int");

                    b.Property<string>("Line")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Locked")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Observations")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VerifiedAt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VerifiedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("WrittenQ")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("InventarID");

                    b.ToTable("MijlocFix");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Name");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.SuperAdmin", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.ToTable("SuperAdmin");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnOrder(0);

                    b.Property<int>("InventarID")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RoleID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Username", "InventarID");

                    b.HasIndex("InventarID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.CostCenter", b =>
                {
                    b.HasOne("FixedAssetsStockTaking.Models.Inventar", "Inventar")
                        .WithMany()
                        .HasForeignKey("InventarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventar");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.ExtraLine", b =>
                {
                    b.HasOne("FixedAssetsStockTaking.Models.Inventar", "Inventar")
                        .WithMany()
                        .HasForeignKey("InventarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventar");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.MijlocFix", b =>
                {
                    b.HasOne("FixedAssetsStockTaking.Models.Inventar", "Inventar")
                        .WithMany("MijlocFixes")
                        .HasForeignKey("InventarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventar");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.User", b =>
                {
                    b.HasOne("FixedAssetsStockTaking.Models.Inventar", "Inventar")
                        .WithMany()
                        .HasForeignKey("InventarID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FixedAssetsStockTaking.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventar");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("FixedAssetsStockTaking.Models.Inventar", b =>
                {
                    b.Navigation("MijlocFixes");
                });
#pragma warning restore 612, 618
        }
    }
}
