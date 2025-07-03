using Duende.IdentityServer.Models;

namespace MyFITJob.Identity.Settings;

public class IdentityServerSettings
{
    public IReadOnlyCollection<ApiScope>  ApiScopes { get; set; } = Array.Empty<ApiScope>();
    public IReadOnlyCollection<Client>  Clients { get; init; } 
    public IReadOnlyCollection<IdentityResource> IdentityResources { get; set; } = new IdentityResource[]
    {
        new IdentityResources.OpenId(), // Créer un nouveau scope OpenId 
        new IdentityResources.Profile(),
    };
} 