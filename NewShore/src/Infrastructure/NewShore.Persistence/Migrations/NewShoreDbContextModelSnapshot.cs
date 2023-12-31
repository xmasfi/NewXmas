﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NewShore.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NewShore.Persistence.Migrations
{
    [DbContext(typeof(NewShoreDbContext))]
    partial class NewShoreDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("NewShore.Domain.Entities.Flight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid>("JourneyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<double>("Price")
                        .HasMaxLength(255)
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("JourneyId");

                    b.ToTable("flights");
                });

            modelBuilder.Entity("NewShore.Domain.Entities.Journey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("journeys");
                });

            modelBuilder.Entity("NewShore.Domain.Entities.Flight", b =>
                {
                    b.HasOne("NewShore.Domain.Entities.Journey", null)
                        .WithMany("Flights")
                        .HasForeignKey("JourneyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("NewShore.Domain.ValueObjects.Transport", "Transport", b1 =>
                        {
                            b1.Property<Guid>("FlightId")
                                .HasColumnType("uuid");

                            b1.Property<string>("FlightCarrier")
                                .HasColumnType("text");

                            b1.Property<string>("FlightNumber")
                                .HasColumnType("text");

                            b1.HasKey("FlightId");

                            b1.ToTable("Transport");

                            b1.WithOwner()
                                .HasForeignKey("FlightId");
                        });

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("NewShore.Domain.Entities.Journey", b =>
                {
                    b.Navigation("Flights");
                });
#pragma warning restore 612, 618
        }
    }
}
