using KonaAI.Master.Model.Common;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;

namespace KonaAI.Master.Business.Master.UserMetaData.Profile;

/// <summary>
/// AutoMapper profile that maps <see cref="ProjectAuditResponsibility"/> to <see cref="MetaDataViewModel"/>.
/// </summary>
/// <remarks>
/// Defines a convention-based mapping relying on matching member names.
/// </remarks>
public class ProjectAuditResponsibilityProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectAuditResponsibilityProfile"/> class
    /// and configures mapping from <see cref="MetaDataViewModel"/> to <see cref="ProjectAuditResponsibility"/>.
    /// </summary>
    public ProjectAuditResponsibilityProfile()
    {
        CreateMap<ProjectAuditResponsibility, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile that maps <see cref="ProjectDepartment"/> to <see cref="MetaDataViewModel"/>.
/// </summary>
/// <remarks>
/// Defines a convention-based mapping relying on matching member names.
/// </remarks>
public class ProjectDepartmentProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectDepartmentProfile"/> class
    /// and configures mapping from <see cref="MetaDataViewModel"/> to <see cref="ProjectDepartment"/>.
    /// </summary>
    public ProjectDepartmentProfile()
    {
        CreateMap<ProjectDepartment, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile that maps <see cref="MetaDataViewModel"/> to <see cref="ProjectRiskArea"/>.
/// </summary>
/// <remarks>
/// Defines a convention-based mapping relying on matching member names.
/// </remarks>
public class ProjectRiskAreaProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectRiskAreaProfile"/> class
    /// and configures mapping from <see cref="MetaDataViewModel"/> to <see cref="ProjectRiskArea"/>.
    /// </summary>
    public ProjectRiskAreaProfile()
    {
        CreateMap<ProjectRiskArea, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile that maps <see cref="MetaDataViewModel"/> to <see cref="ProjectUnit"/>.
/// </summary>
/// <remarks>
/// Defines a convention-based mapping relying on matching member names.
/// </remarks>
public class ProjectUnitProfile : AutoMapper.Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectUnitProfile"/> class
    /// and configures mapping from <see cref="MetaDataViewModel"/> to <see cref="ProjectUnit"/>.
    /// </summary>
    public ProjectUnitProfile()
    {
        CreateMap<ProjectUnit, MetaDataViewModel>();
    }
}

/// <summary>
/// AutoMapper profile that maps <see cref="Country"/> to <see cref="MetaDataViewModel"/>.
/// </summary>
/// <remarks>
/// Adds mapping needed by the master Country endpoint.
/// </remarks>
public class CountryProfile : AutoMapper.Profile
{
    public CountryProfile()
    {
        CreateMap<Country, MetaDataViewModel>();
    }
}