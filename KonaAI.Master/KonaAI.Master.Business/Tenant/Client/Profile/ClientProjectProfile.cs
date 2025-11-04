using KonaAI.Master.Model.Common.Constants;
using KonaAI.Master.Model.Tenant.Client.SaveModel;
using KonaAI.Master.Model.Tenant.Client.ViewModel;
using KonaAI.Master.Repository.Domain.Master.MetaData;
using KonaAI.Master.Repository.Domain.Master.UserMetaData;
using KonaAI.Master.Repository.Domain.Tenant.Client;

namespace KonaAI.Master.Business.Tenant.Client.Profile;

public class ClientProjectViewProfile : AutoMapper.Profile
{
    public class TempMapper
    {
        public ClientProject ClientProject { get; set; } = new();

        public ProjectAuditResponsibility ProjectAuditResponsibility { get; set; } = new();

        public ProjectRiskArea ProjectRiskArea { get; set; } = new();

        public Country? Country { get; set; }

        public ProjectUnit? ProjectUnit { get; set; }

        public ProjectDepartment? ProjectDepartment { get; set; }

        public ProjectStatus ProjectStatus { get; set; } = new();
        public string Modules { get; set; } = string.Empty;
    }

    public ClientProjectViewProfile()
    {
        CreateMap<TempMapper, ClientProjectViewModel>()
            // Map base audit fields from domain entity
            .ForMember(dest => dest.RowId, opt
                => opt.MapFrom(src => src.ClientProject.RowId))
            .ForMember(dest => dest.CreatedOn, opt
                => opt.MapFrom(src => src.ClientProject.CreatedOn))
            .ForMember(dest => dest.CreatedBy, opt
                => opt.MapFrom(src => src.ClientProject.CreatedBy))
            .ForMember(dest => dest.ModifiedOn, opt
                => opt.MapFrom(src => src.ClientProject.ModifiedOn))
            .ForMember(dest => dest.ModifiedBy, opt
                => opt.MapFrom(src => src.ClientProject.ModifiedBy))
            .ForMember(dest => dest.IsActive, opt
                => opt.MapFrom(src => src.ClientProject.IsActive))
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.ClientProject.Name))
            .ForMember(dest => dest.StartDate, opt
                => opt.MapFrom(src => src.ClientProject.StartDate))
            .ForMember(dest => dest.StartDateString, opt
                => opt.MapFrom(src => src.ClientProject.StartDate.ToString(Constants.DefaultDateFormat,
                    System.Globalization.CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.EndDate, opt
                => opt.MapFrom(src => src.ClientProject.EndDate))
            .ForMember(dest => dest.EndDateString, opt
                => opt.MapFrom(src =>
                    src.ClientProject.EndDate != null
                        ? src.ClientProject.EndDate.Value.ToString(Constants.DefaultDateFormat,
                            System.Globalization.CultureInfo.InvariantCulture)
                        : null))
            .ForMember(dest => dest.Description, opt
                => opt.MapFrom(src => src.ClientProject.Description))
            .ForMember(dest => dest.AuditResponsibilityName, opt =>
                opt.MapFrom(src => src.ProjectAuditResponsibility.Name))
            .ForMember(dest => dest.AuditResponsibilityRowId, opt =>
                opt.MapFrom(src => src.ProjectAuditResponsibility.RowId))
            .ForMember(dest => dest.RiskAreaName, opt =>
                opt.MapFrom(src => src.ProjectRiskArea.Name))
            .ForMember(dest => dest.RiskAreaRowId, opt =>
                opt.MapFrom(src => src.ProjectRiskArea.RowId))
            .ForMember(dest => dest.CountryName, opt =>
                opt.MapFrom(src => src.Country != null ? src.Country.Name : null))
            .ForMember(dest => dest.CountryRowId, opt =>
                opt.MapFrom(src => src.Country != null ? (Guid?)src.Country.RowId : null))
            .ForMember(dest => dest.BusinessUnitName, opt =>
                opt.MapFrom(src => src.ProjectUnit != null ? src.ProjectUnit.Name : null))
            .ForMember(dest => dest.BusinessUnitRowId, opt =>
                opt.MapFrom(src => src.ProjectUnit != null ? (Guid?)src.ProjectUnit.RowId : null))
            .ForMember(dest => dest.BusinessDepartmentName, opt =>
                opt.MapFrom(src => src.ProjectDepartment != null ? src.ProjectDepartment.Name : null))
            .ForMember(dest => dest.BusinessDepartmentId, opt =>
                opt.MapFrom(src => src.ProjectDepartment != null ? (Guid?)src.ProjectDepartment.RowId : null))
            .ForMember(dest => dest.ProjectStatusName, opt =>
                opt.MapFrom(src => src.ProjectStatus.Name))
            .ForMember(dest => dest.ProjectStatusRowId, opt =>
                opt.MapFrom(src => src.ProjectStatus.RowId))
            .ForMember(dest => dest.Modules, opt =>
                opt.MapFrom(src => src.Modules));
    }
}

public class ClientProjectCreateProfile : AutoMapper.Profile
{
    public class TempMapper : ClientProjectCreateModel
    {
        public long ProjectStatusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated audit responsibility.
        /// </summary>
        public long ProjectAuditResponsibilityId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated risk area.
        /// </summary>
        public long ProjectRiskAreaId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public long? ProjectCountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated business unit.
        /// </summary>
        public long? ProjectUnitId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated business department.
        /// </summary>
        public long? ProjectDepartmentId { get; set; }
    }

    public ClientProjectCreateProfile()
    {
        CreateMap<ClientProjectCreateProfile.TempMapper, ClientProject>()
            .ForMember(dest => dest.Name, opt
                => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.StartDate, opt
                => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt
                => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Description, opt
                => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.ProjectAuditResponsibilityId, opt
                => opt.MapFrom(src => src.ProjectAuditResponsibilityId))
            .ForMember(dest => dest.ProjectRiskAreaId, opt
                => opt.MapFrom(src => src.ProjectRiskAreaId))
            .ForMember(dest => dest.ProjectCountryId, opt
                => opt.MapFrom(src => src.ProjectCountryId))
            .ForMember(dest => dest.ProjectUnitId, opt
                => opt.MapFrom(src => src.ProjectUnitId))
            .ForMember(dest => dest.ProjectDepartmentId, opt
                => opt.MapFrom(src => src.ProjectDepartmentId))
            .ForMember(dest => dest.Modules, opt
                => opt.MapFrom(src => src.Modules));
    }
}