using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.Infrastructure.Integrations;

public class ContactsService : IContactsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ContactsService> _logger;
    private readonly string _baseUrl;
    private readonly int _timeoutSeconds;

    public ContactsService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ContactsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["ContactsApi:BaseUrl"] ?? "http://localhost:5001/api";
        _timeoutSeconds = int.TryParse(configuration["ContactsApi:TimeoutSeconds"], out var timeout) ? timeout : 30;
        _httpClient.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);
        
        _logger.LogInformation("ContactsService initialisé avec BaseUrl: {BaseUrl}, Timeout: {Timeout}s", _baseUrl, _timeoutSeconds);
    }

    public async Task<CompanyInfo?> GetCompanyInfoAsync(int companyId)
    {
        try
        {
            _logger.LogInformation("Récupération des informations de l'entreprise {CompanyId}", companyId);
            
            var response = await _httpClient.GetAsync($"{_baseUrl}/contacts/companies/{companyId}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ContactsApiResponse<CompanyInfo>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    _logger.LogInformation("Informations de l'entreprise récupérées avec succès: {CompanyName}", apiResponse.Data.Name);
                    return apiResponse.Data;
                }
            }

            _logger.LogWarning("Impossible de récupérer les informations de l'entreprise {CompanyId}. Status: {StatusCode}", companyId, response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des informations de l'entreprise {CompanyId}", companyId);
            return null;
        }
    }

    public async Task<CompanyInfo> CreateCompanyAsync(CreateCompanyDto companyDto)
    {
        try
        {
            _logger.LogInformation("Création d'une nouvelle entreprise: {CompanyName}", companyDto.Name);
            
            var json = JsonSerializer.Serialize(companyDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/contacts/companies", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ContactsApiResponse<CompanyInfo>>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    _logger.LogInformation("Entreprise créée avec succès: {CompanyName} (ID: {CompanyId})", apiResponse.Data.Name, apiResponse.Data.Id);
                    return apiResponse.Data;
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Erreur lors de la création de l'entreprise. Status: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
            throw new InvalidOperationException($"Erreur lors de la création de l'entreprise. Status: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de l'entreprise {CompanyName}", companyDto.Name);
            throw;
        }
    }

    private record ContactsApiResponse<T>
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public T? Data { get; init; }
    }
} 