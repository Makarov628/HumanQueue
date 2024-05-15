﻿// <auto-generated />
using System;
using HQ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HQ.Infrastructure.Migrations
{
    [DbContext(typeof(HQDbContext))]
    [Migration("20230704162811_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HQ.Domain.QueueAggregate.QueueAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DefaultCulture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Queues", (string)null);
                });

            modelBuilder.Entity("HQ.Domain.ServiceAggregate.ServiceAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Literal")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("uuid");

                    b.Property<int>("RequestNumberCounter")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Services", (string)null);
                });

            modelBuilder.Entity("HQ.Domain.TerminalAggregate.TerminalAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ExternalPrinterId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Terminals", (string)null);
                });

            modelBuilder.Entity("HQ.Domain.UserAggregate.UserAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("HQ.Domain.WindowAggregate.WindowAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AttachedUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid>("QueueId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Windows", (string)null);
                });

            modelBuilder.Entity("HQ.Domain.ServiceAggregate.ServiceAggregate", b =>
                {
                    b.OwnsMany("HQ.Domain.ServiceAggregate.Entities.Request", "Requests", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasColumnName("RequestId");

                            b1.Property<Guid>("ParentServiceId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("CalledByWindowId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<Guid>("CreatedForServiceId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("CreatedFromTerminalId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Culture")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<int>("Status")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("timestamp without time zone");

                            b1.HasKey("Id", "ParentServiceId");

                            b1.HasIndex("ParentServiceId");

                            b1.ToTable("ServiceRequests", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ParentServiceId");
                        });

                    b.OwnsMany("HQ.Domain.ServiceAggregate.Entities.WindowLink", "WindowLinks", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("CreatedAt")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<Guid>("ServiceId")
                                .HasColumnType("uuid");

                            b1.Property<DateTime>("UpdatedAt")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<Guid>("WindowId")
                                .HasColumnType("uuid");

                            b1.HasKey("Id");

                            b1.HasIndex("ServiceId");

                            b1.ToTable("ServiceWindowLinks", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ServiceId");
                        });

                    b.Navigation("Requests");

                    b.Navigation("WindowLinks");
                });
#pragma warning restore 612, 618
        }
    }
}