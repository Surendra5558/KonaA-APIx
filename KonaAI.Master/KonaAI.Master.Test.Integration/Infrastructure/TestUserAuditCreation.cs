using KonaAI.Master.Repository.Domain.Master.App;
using KonaAI.Master.Repository.Common.Model;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace KonaAI.Master.Test.Integration.Infrastructure;

/// <summary>
/// Test to verify if UserAudit entity can be created in the in-memory database.
/// </summary>
public class TestUserAuditCreation
{
    [Fact]
    public void CanCreateUserAuditEntity()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestUserAudit")
            .Options;

        using var context = new TestDbContext(options, new TestUserContextService());

        // Act & Assert
        var userAudit = new UserAudit
        {
            RowId = Guid.NewGuid(),
            UserRowId = Guid.NewGuid(),
            UserId = 1,
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            RoleId = 1,
            RoleRowId = Guid.NewGuid(),
            RoleName = "Test Role",
            RoleNavigation = new List<UserPermission>(),
            ProjectAccess = new List<UserProject>(),
            RefreshToken = "test-refresh-token",
            TokenCreatedDate = DateTime.UtcNow,
            TokenExpiredDate = DateTime.UtcNow.AddHours(1),
            CreatedBy = "Test",
            CreatedOn = DateTime.UtcNow,
            ModifiedBy = "Test",
            ModifiedOn = DateTime.UtcNow,
            IsActive = true
        };

        // This should not throw an exception
        var entry = context.UserAudits.Add(userAudit);

        // Force the entry to be tracked as Added
        entry.State = EntityState.Added;

        var result = context.SaveChanges();

        // Assert
        Assert.Equal(1, result);
    }
}
