using System;
using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace Play.Identity.Service.Settings;

public class IdentityServerSettings
{
    public IReadOnlyCollection<ApiScope> ApiScopes { get; init; }
    public IReadOnlyCollection<ApiResource> ApiResources { get; init; }
    public IReadOnlyCollection<Client> Clients { get; init; }
    public IReadOnlyCollection<IdentityResource> IdentityResources { get; set; } =
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", ["role"])
        ];
}
