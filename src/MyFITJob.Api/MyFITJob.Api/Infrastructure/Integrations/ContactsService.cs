using System.Text.Json;
using MyFITJob.Api.JobOffers.DTOs;

namespace MyFITJob.Api.Infrastructure.Integrations;

public class ContactsService : IContactsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ContactsService> _logger;
    private readonly string _baseUrl;
    private readonly JsonSerializerOptions _jsonOptions;

    public ContactsService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ContactsService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _baseUrl = configuration["ContactsApi:BaseUrl"];

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        };
        _logger.LogInformation("ContactsService initialisé avec BaseUrl: {BaseUrl}", _baseUrl);
    }

    public async Task<CompanyInfo?> GetCompanyInfoAsync(int companyId)
    {
        _logger.LogInformation("Récupération des informations de l'entreprise {CompanyId}", companyId);

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/companies/{companyId}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ContactsApiResponse<CompanyInfo>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse?.Success == true && apiResponse.Data != null)
            {
                _logger.LogInformation("Informations de l'entreprise récupérées avec succès: {CompanyName}",
                    apiResponse.Data.Name);
                return apiResponse.Data;
            }
        }

        _logger.LogWarning(
            "Impossible de récupérer les informations de l'entreprise {CompanyId}. Status: {StatusCode}", companyId,
            response.StatusCode);

        return null;
    }

    public async Task<CompanyInfo> CreateCompanyAsync(CreateCompanyDto companyDto)
    {
        try
        {
            _logger.LogInformation("Création d'une nouvelle entreprise: {CompanyName}", companyDto.Name);

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/companies", companyDto,
                _jsonOptions);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ContactsApiResponse<CompanyInfo>>(responseContent, _jsonOptions);

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    _logger.LogInformation("Entreprise créée avec succès: {CompanyName} (ID: {CompanyId})",
                        apiResponse.Data.Name, apiResponse.Data.Id);
                    return apiResponse.Data;
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Erreur lors de la création de l'entreprise. Status: {StatusCode}, Content: {Content}",
                response.StatusCode, errorContent);
            throw new InvalidOperationException(
                $"Erreur lors de la création de l'entreprise. Status: {response.StatusCode}");
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