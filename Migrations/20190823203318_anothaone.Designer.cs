﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using csharp_belt.Models;

namespace csharp_belt.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190823203318_anothaone")]
    partial class anothaone
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("csharp_belt.Models.Activity", b =>
                {
                    b.Property<int>("ActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnName("date");

                    b.Property<string>("Description")
                        .HasColumnName("description");

                    b.Property<int?>("DurationTime")
                        .HasColumnName("duration_time");

                    b.Property<string>("DurationUnit")
                        .HasColumnName("duration_unit");

                    b.Property<string>("Time")
                        .HasColumnName("time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("ActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("csharp_belt.Models.Rsvp", b =>
                {
                    b.Property<int>("RsvpId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<int>("ActivityId")
                        .HasColumnName("activity_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("RsvpId");

                    b.HasIndex("ActivityId");

                    b.HasIndex("UserId");

                    b.ToTable("Rsvps");
                });

            modelBuilder.Entity("csharp_belt.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnName("updated_at");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("csharp_belt.Models.Activity", b =>
                {
                    b.HasOne("csharp_belt.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("csharp_belt.Models.Rsvp", b =>
                {
                    b.HasOne("csharp_belt.Models.Activity", "Rsvps")
                        .WithMany("AllParticipants")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("csharp_belt.Models.User", "Rsvped")
                        .WithMany("AllActivities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}