using Duende.IdentityServer.Models;

namespace MyFITJob.Identity.Settings;

public class IdentityServerSettings
{
    public IReadOnlyCollection<ApiScope>  ApiScopes { get; init; }
    public IReadOnlyCollection<ApiResource>  ApiResources { get; init; }
    public IReadOnlyCollection<Client>  Clients { get; init; } 
    public IReadOnlyCollection<IdentityResource> IdentityResources { get; set; } = new IdentityResource[]
    {
        new IdentityResources.OpenId(), // Créer un nouveau scope OpenId 
        new IdentityResources.Profile(),
    };
} 