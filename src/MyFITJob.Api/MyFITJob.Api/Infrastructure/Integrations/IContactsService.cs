using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.Infrastructure.Integrations;

public interface IContactsService
{
    Task<CompanyInfo?> GetCompanyInfoAsync(int companyId);
    Task<CompanyInfo> CreateCompanyAsync(CreateCompanyDto companyDto);
} 