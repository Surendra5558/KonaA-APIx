using KonaAI.Master.Repository.Common.Extensions;
using KonaAI.Master.Repository.Domain.Tenant.ClientUserMetaData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonaAI.Master.Repository.Configuration.Tenant.ClientUserMetaData;

/// <summary>
/// Configures the <see cref="ClientProjectCountry"/> entity mapping.
/// </summary>
public class ClientProjectCountryConfiguration : IEntityTypeConfiguration<ClientProjectCountry>
{
    public void Configure(EntityTypeBuilder<ClientProjectCountry> builder)
    {
        // Map to the expected schema/table to avoid default pluralized/no-schema table name
        builder.BaseClientMetaDataConfiguration("ClientProjectCountry", "ClientUserMetaData");
    }
}