﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.Converters;
using SmartValley.Data.SQL.Core;
using SmartValley.Domain.Core;

namespace SmartValley.Data.SQL.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-preview1-28290")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartValley.Domain.AllotmentEvent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("FinishDate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<long>("ProjectId");

                    b.Property<DateTimeOffset?>("StartDate");

                    b.Property<int>("Status");

                    b.Property<string>("TokenContractAddress")
                        .IsRequired();

                    b.Property<string>("TokenTicker")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<long>("TotalTokens");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("AllotmentEvents");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Area", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("MaxScore");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.AreaScoring", b =>
                {
                    b.Property<long>("ScoringId");

                    b.Property<int>("AreaId");

                    b.Property<int>("ExpertsCount");

                    b.Property<double?>("Score");

                    b.HasKey("ScoringId", "AreaId");

                    b.HasIndex("ScoringId", "AreaId");

                    b.ToTable("AreaScorings");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Country", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(2);

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Estimate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<long>("ExpertScoringId");

                    b.Property<int?>("Score");

                    b.Property<long>("ScoringCriterionId");

                    b.HasKey("Id");

                    b.HasIndex("ExpertScoringId");

                    b.HasIndex("ScoringCriterionId");

                    b.ToTable("Estimates");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.EthereumTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("Hash")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Hash")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("EthereumTransactions");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Expert", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<bool>("IsAvailable");

                    b.Property<bool>("IsInHouse");

                    b.HasKey("UserId");

                    b.ToTable("Experts");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertApplication", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ApplicantId");

                    b.Property<DateTimeOffset>("ApplyDate");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("BitcointalkLink")
                        .HasMaxLength(400);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<long>("CountryId");

                    b.Property<string>("CvUrl")
                        .HasMaxLength(200);

                    b.Property<string>("Description")
                        .HasMaxLength(1500);

                    b.Property<string>("DocumentNumber")
                        .HasMaxLength(30);

                    b.Property<int>("DocumentType");

                    b.Property<string>("FacebookLink")
                        .HasMaxLength(400);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("LinkedInLink")
                        .HasMaxLength(400);

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(200);

                    b.Property<string>("ScanUrl")
                        .HasMaxLength(200);

                    b.Property<int>("Sex");

                    b.Property<int>("Status");

                    b.Property<string>("Why")
                        .IsRequired()
                        .HasMaxLength(1500);

                    b.HasKey("Id");

                    b.HasIndex("ApplicantId");

                    b.HasIndex("CountryId");

                    b.ToTable("ExpertApplications");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertApplicationArea", b =>
                {
                    b.Property<long>("ExpertApplicationId");

                    b.Property<int>("AreaId");

                    b.Property<int>("Status");

                    b.HasKey("ExpertApplicationId", "AreaId");

                    b.HasIndex("AreaId");

                    b.ToTable("ExpertApplicationAreas");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertArea", b =>
                {
                    b.Property<long>("ExpertId");

                    b.Property<int>("AreaId");

                    b.HasKey("ExpertId", "AreaId");

                    b.HasIndex("AreaId");

                    b.ToTable("ExpertAreas");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertScoring", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Area");

                    b.Property<string>("Conclusion");

                    b.Property<long>("ExpertId");

                    b.Property<long>("ScoringId");

                    b.HasKey("Id");

                    b.HasIndex("ExpertId");

                    b.HasIndex("ScoringId");

                    b.ToTable("ExpertScorings");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Feedback", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1500);

                    b.HasKey("Id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Project", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuthorId");

                    b.Property<string>("BitcoinTalk")
                        .HasMaxLength(200);

                    b.Property<int>("Category");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(200);

                    b.Property<long>("CountryId");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000);

                    b.Property<Guid>("ExternalId");

                    b.Property<string>("Facebook")
                        .HasMaxLength(200);

                    b.Property<string>("Github")
                        .HasMaxLength(200);

                    b.Property<DateTimeOffset?>("IcoDate");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(200);

                    b.Property<bool>("IsPrivate");

                    b.Property<string>("Linkedin")
                        .HasMaxLength(200);

                    b.Property<string>("Medium")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("Reddit")
                        .HasMaxLength(200);

                    b.Property<int>("Stage");

                    b.Property<string>("Telegram")
                        .HasMaxLength(200);

                    b.Property<string>("Twitter")
                        .HasMaxLength(200);

                    b.Property<string>("Website")
                        .HasMaxLength(200);

                    b.Property<string>("WhitePaperLink")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CountryId");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ProjectTeamMember", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About")
                        .HasMaxLength(500);

                    b.Property<string>("Facebook")
                        .HasMaxLength(200);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Linkedin")
                        .HasMaxLength(200);

                    b.Property<string>("PhotoUrl")
                        .HasMaxLength(200);

                    b.Property<long>("ProjectId");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectTeamMembers");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Role", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Scoring", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("AcceptingDeadline");

                    b.Property<Address>("ContractAddress")
                        .HasConversion(new ValueConverter<Address, string>(v => default(string), v => default(Address)))
                        .HasMaxLength(42);

                    b.Property<DateTimeOffset>("CreationDate");

                    b.Property<long>("ProjectId");

                    b.Property<double?>("Score");

                    b.Property<DateTimeOffset>("ScoringDeadline");

                    b.Property<DateTimeOffset?>("ScoringEndDate");

                    b.Property<DateTimeOffset?>("ScoringStartDate");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.HasIndex("ScoringEndDate");

                    b.ToTable("Scorings");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ScoringApplicationQuestion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ExtendedInfo");

                    b.Property<string>("GroupKey")
                        .IsRequired();

                    b.Property<int>("GroupOrder");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<long?>("ParentId");

                    b.Property<string>("ParentTriggerValue");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("ScoringApplicationQuestions");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ScoringCriteriaMapping", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ScoringApplicationQuestionId");

                    b.Property<long>("ScoringCriterionId");

                    b.HasKey("Id");

                    b.HasIndex("ScoringApplicationQuestionId");

                    b.HasIndex("ScoringCriterionId");

                    b.ToTable("ScoringCriteriaMappings");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ScoringCriterion", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AreaType");

                    b.Property<string>("GroupKey");

                    b.Property<int>("GroupOrder");

                    b.Property<bool>("HasMiddleScoreOption");

                    b.Property<int>("Order");

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.ToTable("ScoringCriteria");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ScoringOffer", b =>
                {
                    b.Property<long>("ScoringId");

                    b.Property<int>("AreaId");

                    b.Property<long>("ExpertId");

                    b.Property<int>("Status");

                    b.HasKey("ScoringId", "AreaId", "ExpertId");

                    b.HasIndex("ExpertId");

                    b.HasIndex("ScoringId", "AreaId", "ExpertId");

                    b.ToTable("ScoringOffers");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Subscription", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Phone")
                        .HasMaxLength(50);

                    b.Property<long>("ProjectId");

                    b.Property<string>("Sum")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<Address>("Address")
                        .HasConversion(new ValueConverter<Address, string>(v => default(string), v => default(Address)))
                        .HasMaxLength(42);

                    b.Property<DateTime?>("BirthDate");

                    b.Property<string>("BitcointalkLink")
                        .HasMaxLength(400);

                    b.Property<bool>("CanCreatePrivateProjects");

                    b.Property<string>("City")
                        .HasMaxLength(100);

                    b.Property<long?>("CountryId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FacebookLink")
                        .HasMaxLength(400);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<bool>("IsEmailConfirmed");

                    b.Property<string>("LinkedInLink")
                        .HasMaxLength(400);

                    b.Property<DateTime?>("RegistrationDate");

                    b.Property<string>("SecondName")
                        .HasMaxLength(50);

                    b.Property<int?>("Sex");

                    b.HasKey("Id");

                    b.HasIndex("Address")
                        .IsUnique();

                    b.HasIndex("CountryId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.UserRole", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplication", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Articles");

                    b.Property<int?>("Category");

                    b.Property<string>("ContactEmail");

                    b.Property<long?>("CountryId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTimeOffset?>("IcoDate");

                    b.Property<bool>("IsSubmitted");

                    b.Property<string>("ProjectDescription");

                    b.Property<long>("ProjectId");

                    b.Property<string>("ProjectName");

                    b.Property<DateTimeOffset?>("Saved");

                    b.Property<long?>("ScoringStartTransactionId");

                    b.Property<string>("Site");

                    b.Property<int?>("Stage");

                    b.Property<DateTimeOffset?>("Submitted");

                    b.Property<string>("WhitePaper");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.HasIndex("ScoringStartTransactionId");

                    b.ToTable("ScoringApplications");
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplicationAdviser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("FacebookLink");

                    b.Property<string>("FullName");

                    b.Property<string>("LinkedInLink");

                    b.Property<string>("Reason");

                    b.Property<long?>("ScoringApplicationId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ScoringApplicationId");

                    b.ToTable("ScoringApplicationAdvisers");
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplicationAnswer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("QuestionId");

                    b.Property<long>("ScoringApplicationId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("ScoringApplicationId");

                    b.ToTable("ScoringApplicationAnswers");
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplicationTeamMember", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("AdditionalInformation");

                    b.Property<string>("FacebookLink");

                    b.Property<string>("FullName");

                    b.Property<string>("LinkedInLink");

                    b.Property<string>("ProjectRole");

                    b.Property<long?>("ScoringApplicationId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ScoringApplicationId");

                    b.ToTable("ScoringApplicationTeamMembers");
                });

            modelBuilder.Entity("SmartValley.Domain.AllotmentEvent", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Project")
                        .WithMany("AllotmentEvents")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.AreaScoring", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Scoring", "Scoring")
                        .WithMany("AreaScorings")
                        .HasForeignKey("ScoringId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Estimate", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.ExpertScoring", "ExpertScoring")
                        .WithMany("Estimates")
                        .HasForeignKey("ExpertScoringId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.ScoringCriterion", "ScoringCriterion")
                        .WithMany()
                        .HasForeignKey("ScoringCriterionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.EthereumTransaction", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Expert", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.User", "User")
                        .WithOne("Expert")
                        .HasForeignKey("SmartValley.Domain.Entities.Expert", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertApplication", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.User", "Applicant")
                        .WithMany()
                        .HasForeignKey("ApplicantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.Country")
                        .WithMany("ExpertApplications")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertApplicationArea", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Area")
                        .WithMany("ExpertApplicationAreas")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.ExpertApplication", "ExpertApplication")
                        .WithMany("ExpertApplicationAreas")
                        .HasForeignKey("ExpertApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertArea", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Area", "Area")
                        .WithMany()
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.Expert", "Expert")
                        .WithMany("ExpertAreas")
                        .HasForeignKey("ExpertId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ExpertScoring", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Expert", "Expert")
                        .WithMany()
                        .HasForeignKey("ExpertId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.Scoring", "Scoring")
                        .WithMany("ExpertScorings")
                        .HasForeignKey("ScoringId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Project", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.User", "Author")
                        .WithMany("Projects")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SmartValley.Domain.Entities.Country", "Country")
                        .WithMany("Projects")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ProjectTeamMember", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Project")
                        .WithMany("TeamMembers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Scoring", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Project")
                        .WithOne("Scoring")
                        .HasForeignKey("SmartValley.Domain.Entities.Scoring", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ScoringCriteriaMapping", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.ScoringApplicationQuestion", "ScoringApplicationQuestion")
                        .WithMany()
                        .HasForeignKey("ScoringApplicationQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.ScoringCriterion", "ScoringCriterion")
                        .WithMany()
                        .HasForeignKey("ScoringCriterionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.ScoringOffer", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Expert", "Expert")
                        .WithMany()
                        .HasForeignKey("ExpertId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.Scoring")
                        .WithMany("ScoringOffers")
                        .HasForeignKey("ScoringId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.Subscription", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.User", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Country", "Country")
                        .WithMany("Users")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("SmartValley.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplication", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.Country", "Country")
                        .WithMany("ScoringApplications")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SmartValley.Domain.Entities.Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.Entities.EthereumTransaction", "ScoringStartTransaction")
                        .WithMany()
                        .HasForeignKey("ScoringStartTransactionId");

                    b.OwnsOne("SmartValley.Domain.Entities.SocialNetworks", "SocialNetworks", b1 =>
                        {
                            b1.Property<long?>("ScoringApplicationId");

                            b1.Property<string>("BitcoinTalk")
                                .HasColumnName("BitcointalkLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Facebook")
                                .HasColumnName("FacebookLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Github")
                                .HasColumnName("GitHubLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Linkedin")
                                .HasColumnName("LinkedInLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Medium")
                                .HasColumnName("MediumLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Reddit")
                                .HasColumnName("RedditLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Telegram")
                                .HasColumnName("TelegramLink")
                                .HasMaxLength(500);

                            b1.Property<string>("Twitter")
                                .HasColumnName("TwitterLink")
                                .HasMaxLength(500);

                            b1.ToTable("ScoringApplications");

                            b1.HasOne("SmartValley.Domain.ScoringApplication")
                                .WithOne("SocialNetworks")
                                .HasForeignKey("SmartValley.Domain.Entities.SocialNetworks", "ScoringApplicationId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplicationAdviser", b =>
                {
                    b.HasOne("SmartValley.Domain.ScoringApplication")
                        .WithMany("Advisers")
                        .HasForeignKey("ScoringApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplicationAnswer", b =>
                {
                    b.HasOne("SmartValley.Domain.Entities.ScoringApplicationQuestion", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SmartValley.Domain.ScoringApplication", "ScoringApplication")
                        .WithMany("Answers")
                        .HasForeignKey("ScoringApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SmartValley.Domain.ScoringApplicationTeamMember", b =>
                {
                    b.HasOne("SmartValley.Domain.ScoringApplication")
                        .WithMany("TeamMembers")
                        .HasForeignKey("ScoringApplicationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
