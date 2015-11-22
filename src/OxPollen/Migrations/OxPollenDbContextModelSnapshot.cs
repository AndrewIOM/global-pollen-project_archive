using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using OxPollen.Models;

namespace OxPollen.Migrations
{
    [DbContext(typeof(OxPollenDbContext))]
    partial class OxPollenDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
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
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("OxPollen.Models.AppUser", b =>
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

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("OrganisationOrganisationId");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Title");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("OxPollen.Models.Grain", b =>
                {
                    b.Property<int>("GrainId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AgeYearsBeforePresent");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("SubmittedById")
                        .IsRequired();

                    b.Property<int?>("TaxonTaxonId");

                    b.Property<DateTime>("TimeAdded");

                    b.HasKey("GrainId");
                });

            modelBuilder.Entity("OxPollen.Models.GrainImage", b =>
                {
                    b.Property<int>("GrainImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<int?>("GrainGrainId");

                    b.Property<double>("ScaleNanoMetres");

                    b.HasKey("GrainImageId");
                });

            modelBuilder.Entity("OxPollen.Models.Identification", b =>
                {
                    b.Property<int>("IdentificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Family");

                    b.Property<string>("Genus");

                    b.Property<int?>("GrainGrainId");

                    b.Property<int>("Rank");

                    b.Property<string>("Species");

                    b.Property<DateTime>("Time");

                    b.Property<string>("UserId");

                    b.HasKey("IdentificationId");
                });

            modelBuilder.Entity("OxPollen.Models.Organisation", b =>
                {
                    b.Property<int>("OrganisationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryCode");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("OrganisationId");
                });

            modelBuilder.Entity("OxPollen.Models.Taxon", b =>
                {
                    b.Property<int>("TaxonId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GbifId");

                    b.Property<string>("LatinName")
                        .IsRequired();

                    b.Property<int>("NeotomaId");

                    b.Property<int?>("ParentTaxaTaxonId");

                    b.Property<int>("Rank");

                    b.HasKey("TaxonId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("OxPollen.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("OxPollen.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("OxPollen.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OxPollen.Models.AppUser", b =>
                {
                    b.HasOne("OxPollen.Models.Organisation")
                        .WithMany()
                        .HasForeignKey("OrganisationOrganisationId");
                });

            modelBuilder.Entity("OxPollen.Models.Grain", b =>
                {
                    b.HasOne("OxPollen.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("SubmittedById");

                    b.HasOne("OxPollen.Models.Taxon")
                        .WithMany()
                        .HasForeignKey("TaxonTaxonId");
                });

            modelBuilder.Entity("OxPollen.Models.GrainImage", b =>
                {
                    b.HasOne("OxPollen.Models.Grain")
                        .WithMany()
                        .HasForeignKey("GrainGrainId");
                });

            modelBuilder.Entity("OxPollen.Models.Identification", b =>
                {
                    b.HasOne("OxPollen.Models.Grain")
                        .WithMany()
                        .HasForeignKey("GrainGrainId");

                    b.HasOne("OxPollen.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OxPollen.Models.Taxon", b =>
                {
                    b.HasOne("OxPollen.Models.Taxon")
                        .WithMany()
                        .HasForeignKey("ParentTaxaTaxonId");
                });
        }
    }
}
