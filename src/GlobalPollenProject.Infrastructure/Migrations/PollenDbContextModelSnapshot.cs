using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using GlobalPollenProject.Data.Infrastructure;

namespace GlobalPollenProject.Infrastructure.Migrations
{
    [DbContext(typeof(PollenDbContext))]
    partial class PollenDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GlobalPollenProject.Core.Identification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Family");

                    b.Property<string>("Genus");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Rank");

                    b.Property<string>("Species");

                    b.Property<DateTime>("Time");

                    b.Property<int?>("UnknownGrainId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UnknownGrainId");

                    b.HasIndex("UserId");

                    b.ToTable("Identification");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<string>("FileNameThumbnail");

                    b.Property<string>("FocusHighUrl");

                    b.Property<string>("FocusLowUrl");

                    b.Property<string>("FocusMedHighUrl");

                    b.Property<string>("FocusMedLowUrl");

                    b.Property<string>("FocusMedUrl");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsFocusImage");

                    b.Property<int?>("ReferenceSlideId");

                    b.Property<int?>("UnknownGrainId");

                    b.HasKey("Id");

                    b.HasIndex("ReferenceSlideId");

                    b.HasIndex("UnknownGrainId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.KewBackboneTaxon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LatinName");

                    b.Property<string>("LatinNameAuthorship");

                    b.Property<int?>("ParentTaxaId");

                    b.Property<int>("Rank");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.HasIndex("ParentTaxaId");

                    b.ToTable("BackboneTaxa");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.Organisation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Organisation");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.ReferenceCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContactEmail");

                    b.Property<string>("CountryCode");

                    b.Property<string>("Description");

                    b.Property<string>("FocusRegion");

                    b.Property<string>("Institution");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("OwnedBy");

                    b.Property<string>("OwnerId");

                    b.Property<string>("WebAddress");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("ReferenceCollections");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.ReferenceSlide", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BelongsToId");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("MaxDiameter");

                    b.Property<int?>("TaxonId");

                    b.Property<DateTime>("TimeAdded");

                    b.HasKey("Id");

                    b.HasIndex("BelongsToId");

                    b.HasIndex("TaxonId");

                    b.ToTable("ReferenceSlide");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.Taxon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GbifId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LatinName");

                    b.Property<int>("NeotomaId");

                    b.Property<int?>("ParentTaxonId");

                    b.Property<int>("Rank");

                    b.HasKey("Id");

                    b.HasIndex("ParentTaxonId");

                    b.ToTable("Taxa");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.UnknownGrain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AgeYearsBeforePresent");

                    b.Property<bool>("IsDeleted");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<double>("MaxDiameter");

                    b.Property<string>("SubmittedById");

                    b.Property<int?>("TaxonId");

                    b.Property<DateTime>("TimeAdded");

                    b.HasKey("Id");

                    b.HasIndex("SubmittedById");

                    b.HasIndex("TaxonId");

                    b.ToTable("UnknownGrains");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.User", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<double>("BountyScore");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("OrganisationId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<bool>("RequestedDigitisationRights");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Title");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("OrganisationId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.Identification", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.UnknownGrain")
                        .WithMany("Identifications")
                        .HasForeignKey("UnknownGrainId");

                    b.HasOne("GlobalPollenProject.Core.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.Image", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.ReferenceSlide")
                        .WithMany("Images")
                        .HasForeignKey("ReferenceSlideId");

                    b.HasOne("GlobalPollenProject.Core.UnknownGrain")
                        .WithMany("Images")
                        .HasForeignKey("UnknownGrainId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.KewBackboneTaxon", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.KewBackboneTaxon", "ParentTaxa")
                        .WithMany("ChildTaxa")
                        .HasForeignKey("ParentTaxaId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.ReferenceCollection", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.ReferenceSlide", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.ReferenceCollection", "BelongsTo")
                        .WithMany("Slides")
                        .HasForeignKey("BelongsToId");

                    b.HasOne("GlobalPollenProject.Core.Taxon", "Taxon")
                        .WithMany("ReferenceSlides")
                        .HasForeignKey("TaxonId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.Taxon", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.Taxon", "ParentTaxon")
                        .WithMany("ChildTaxa")
                        .HasForeignKey("ParentTaxonId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.UnknownGrain", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.User", "SubmittedBy")
                        .WithMany()
                        .HasForeignKey("SubmittedById");

                    b.HasOne("GlobalPollenProject.Core.Taxon")
                        .WithMany("UnknownGrains")
                        .HasForeignKey("TaxonId");
                });

            modelBuilder.Entity("GlobalPollenProject.Core.User", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.Organisation", "Organisation")
                        .WithMany("Members")
                        .HasForeignKey("OrganisationId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GlobalPollenProject.Core.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GlobalPollenProject.Core.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
