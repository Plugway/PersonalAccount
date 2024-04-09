﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalAccountEF.Data;

#nullable disable

namespace PersonalAccountEF.Migrations
{
    [DbContext(typeof(PersonalAccountContext))]
    partial class PersonalAccountContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("PersonalAccountEF.Data.Models.Address", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ApartmentNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("PersonalAccountEF.Data.Models.PersonalAccount", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<uint>("AddressId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ApartmentArea")
                        .HasColumnType("REAL");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonalAccountNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("PersonalAccounts");
                });

            modelBuilder.Entity("PersonalAccountEF.Data.Models.Resident", b =>
                {
                    b.Property<uint>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("DateOfBirth")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<uint>("PersonalAccountId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PersonalAccountId");

                    b.ToTable("Residents");
                });

            modelBuilder.Entity("PersonalAccountEF.Data.Models.PersonalAccount", b =>
                {
                    b.HasOne("PersonalAccountEF.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("PersonalAccountEF.Data.Models.Resident", b =>
                {
                    b.HasOne("PersonalAccountEF.Data.Models.PersonalAccount", null)
                        .WithMany("Residents")
                        .HasForeignKey("PersonalAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PersonalAccountEF.Data.Models.PersonalAccount", b =>
                {
                    b.Navigation("Residents");
                });
#pragma warning restore 612, 618
        }
    }
}
