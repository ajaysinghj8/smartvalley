﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Entities;
using System;

namespace SmartValley.Data.SQL.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20171127123048_ApplicationAndProjectValidation")]
    partial class ApplicationAndProjectValidation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartValley.Domain.Entities.Application", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BlockchainType")
                        .HasMaxLength(100);

                    b.Property<string>("FinancialModelLink")
                        .HasMaxLength(400);

                    b.Property<string>("HardCap")
                        .HasMaxLength(40);

                    b.Property<bool>("InvestmentsAreAttracted");

                    b.Property<string>("MvpLink")
                        .HasMaxLength(400);

                    b.Property<long>("ProjectId");

                    b.Property<string>("ProjectStatus")
                        .HasMaxLength(100);

                    b.Property<string>("SoftCap")
                        .HasMaxLength(40);

                    b.Property<string>("WhitePaperLink")
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Estimate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<string>("ExpertAddress")
                        .IsRequired()
                        .HasMaxLength(42);

                    b.Property<long>("ProjectId");

                    b.Property<int>("QuestionIndex");

                    b.Property<int>("Score");

                    b.Property<int>("ScoringCategory");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Estimates");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("AnalystEstimatesCount");

                    b.Property<string>("AuthorAddress")
                        .IsRequired()
                        .HasMaxLength(42);

                    b.Property<string>("Country")
                        .HasMaxLength(100);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<Guid>("ExternalId");

                    b.Property<byte>("HrEstimatesCount");

                    b.Property<byte>("LawyerEstimatesCount");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ProjectAddress")
                        .IsRequired()
                        .HasMaxLength(42);

                    b.Property<string>("ProjectArea")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<double?>("Score");

                    b.Property<byte>("TechnicalEstimatesCount");

                    b.HasKey("Id");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.TeamMember", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ApplicationId");

                    b.Property<string>("FacebookLink")
                        .HasMaxLength(200);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("LinkedInLink")
                        .HasMaxLength(200);

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationId");

                    b.ToTable("TeamMembers");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Application", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Estimate", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.TeamMember", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Application", "Application")
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
