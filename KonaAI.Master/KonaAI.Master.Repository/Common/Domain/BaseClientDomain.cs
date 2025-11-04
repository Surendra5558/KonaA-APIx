namespace KonaAI.Master.Repository.Common.Domain;

/// <summary>
/// Represents a base domain entity that includes a client identifier.
/// Inherits common audit and identity properties from <see cref="BaseDomain"/>.
/// </summary>
public class BaseClientDomain : BaseDomain
{
    /// <summary>
    /// Gets or sets the unique identifier of the client associated with this entity.
    /// </summary>
    public virtual long ClientId { get; set; }
}