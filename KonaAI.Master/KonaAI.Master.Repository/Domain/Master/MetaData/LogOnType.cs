using KonaAI.Master.Repository.Common.Domain;
using KonaAI.Master.Repository.Domain.Master.App;

namespace KonaAI.Master.Repository.Domain.Master.MetaData;

/// <summary>
/// Represents a type of Single Sign-On (SSO) configuration, including its metadata.
/// Inherits metadata properties such as name, description, and order from <see cref="BaseMetaDataDomain"/>.
/// </summary>
public class LogOnType : BaseMetaDataDomain
{
    /// <summary>
    /// Gets or sets the collection of users that are associated with this logon type.
    /// </summary>
    /// <remarks>
    /// This is the inverse navigation of the foreign key held on the <c>User.LogOnTypeId</c> property.
    /// </remarks>
    public ICollection<User> Users { get; set; } = new List<User>();
}