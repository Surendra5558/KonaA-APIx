using KonaAI.Master.Repository.Common.Constants;
using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Master.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Master.App;

/// <summary>
/// Configures the entity mapping and property constraints for the <see cref="User"/> entity.
/// Sets up table schema, unique indexes, required fields, maximum lengths, relationships, and default values.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the <see cref="User"/> entity's schema, property constraints, indexes, and relationships
    /// using the provided <see cref="EntityTypeBuilder{User}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="User"/> entity.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.BaseConfiguration("User");

        builder.HasIndex(x => x.UserName)
            .IsUnique();

        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.HasIndex(x => x.Email)
            .IsUnique();
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(DbColumnLength.NameEmail);

        builder.Property(x => x.Password)
            .HasMaxLength(DbColumnLength.Description);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(DbColumnLength.PhoneNumber);

        builder.Property(x => x.PhoneNumberCountryCode)
            .HasMaxLength(DbColumnLength.CountryCode);

        // Make sure the relationship mirrors the LogOnType configuration to prevent a shadow FK
        builder.HasOne(x => x.LogOnType)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.LogOnTypeId)
            .IsRequired();

        // Relationship with RoleType
        builder.HasOne(x => x.RoleType)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleTypeId)
            .IsRequired();

        builder.Property(x => x.IsResetRequested)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasData(
            new User
            {
                RowId = Guid.Parse("D5D27767-5732-4753-A37E-B99E332AC582"),
                Id = 1,
                UserName = "system@konaai.com",
                Email = "system@konaai.com",
                LogOnTypeId = 1,
                FirstName = "Default",
                LastName = "User",
                CreatedBy = "Default User",
                CreatedById = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1,
                RoleTypeId = 2,
                IsActive = false
            },
            new User
            {
                RowId = Guid.Parse("C53A0608-C123-4B1F-89A6-BCC1E396BC35"),
                Id = 2,
                UserName = "kkona@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "kkona@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Krishna Chaitanya",
                LastName = "Kona",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 1,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("EBAAE8CD-1224-4018-A56B-F7E0AFF8C8FA"),
                Id = 3,
                UserName = "nsyed@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "nsyed@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Nishal",
                LastName = "Syed",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 2,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("5C69E682-BFCA-4FE3-9EC3-392F21AA23A4"),
                Id = 4,
                UserName = "gnunepally@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "gnunepally@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Geha Reddy",
                LastName = "Nunepally",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 3,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("6AF2D35A-CEB0-47A5-A8A6-9E1FDBF14C6C"),
                Id = 5,
                UserName = "rk@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "rk@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Rama Krishna",
                LastName = "Pavani",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 4,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("EA833280-5675-41DE-BFCE-C2653D46DBEA"),
                Id = 6,
                UserName = "lkadire@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "lkadire@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Rama Krishna",
                LastName = "Pavani",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 5,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("462B2AA4-2D61-4626-A28A-2CB470014216"),
                Id = 7,
                UserName = "bpenmetsa@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "bpenmetsa@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Balaram Raju",
                LastName = "Penmetsa",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 6,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("90E344BA-AB61-48FE-80B6-C001968DE198"),
                Id = 8,
                UserName = "rdudekula@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "rdudekula@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Reshma",
                LastName = "Dudekula",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 7,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("C4D99798-2686-4B07-8044-23C7B2B32F12"),
                Id = 9,
                UserName = "svajjha@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "svajjha@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Srivalli",
                LastName = "Vajjha",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 5,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("046EAEC1-13DF-452F-83E5-A3D99C2D8336"),
                Id = 10,
                UserName = "lpasumarthi@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "lpasumarthi@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Likitha",
                LastName = "Pasumarthi",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 5,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("C81C5C0F-C1A6-4256-95EC-ED0CF0DFA955"),
                Id = 11,
                UserName = "rmadam@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "rmadam@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Rajath",
                LastName = "Bhargav",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 5,
                ModifiedBy = "Default User",
                ModifiedById = 1
            },
            new User
            {
                RowId = Guid.Parse("05F830A5-ACD2-40B2-857F-477098B24B58"),
                Id = 12,
                UserName = "skilari@konaai.com",
                Password = "$2b$12$YAP.oTkyjlqbzrIRvAK9l.fLapZLtJy/eRLHMD0.KjTmS5CZARf6y",
                Email = "skilari@konaai.com",
                LogOnTypeId = 3,
                FirstName = "Sai",
                LastName = "Krishna",
                CreatedBy = "Default User",
                CreatedById = 1,
                RoleTypeId = 5,
                ModifiedBy = "Default User",
                ModifiedById = 1
            }

        );
    }
}