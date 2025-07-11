﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkTimeTable.DataBase;

#nullable disable

namespace WorkTimeTable.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WorkTimeTable.DataBase.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contract");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("LeaderId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LeaderId")
                        .IsUnique()
                        .HasFilter("[LeaderId] IS NOT NULL");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.DepartmentHierarchy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("DepartmentHierarchy");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Timetable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContractId")
                        .HasColumnType("int");

                    b.Property<int>("Workerid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("Workerid");

                    b.ToTable("Timetable");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Worker");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Department", b =>
                {
                    b.HasOne("WorkTimeTable.DataBase.Worker", "Leader")
                        .WithOne("DepartmentUnderControl")
                        .HasForeignKey("WorkTimeTable.DataBase.Department", "LeaderId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Leader");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.DepartmentHierarchy", b =>
                {
                    b.HasOne("WorkTimeTable.DataBase.DepartmentHierarchy", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Timetable", b =>
                {
                    b.HasOne("WorkTimeTable.DataBase.Contract", "Contract")
                        .WithMany("Timetables")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkTimeTable.DataBase.Worker", "Worker")
                        .WithMany("Timetables")
                        .HasForeignKey("Workerid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Worker", b =>
                {
                    b.HasOne("WorkTimeTable.DataBase.Department", "Department")
                        .WithMany("Workers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Department");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Contract", b =>
                {
                    b.Navigation("Timetables");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Department", b =>
                {
                    b.Navigation("Workers");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.DepartmentHierarchy", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("WorkTimeTable.DataBase.Worker", b =>
                {
                    b.Navigation("DepartmentUnderControl");

                    b.Navigation("Timetables");
                });
#pragma warning restore 612, 618
        }
    }
}
