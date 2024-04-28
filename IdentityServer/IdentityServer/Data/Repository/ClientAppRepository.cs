using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using IdentityServer.Records;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace IdentityServer.Data.Repository
{
    public class ClientAppRepository
    {
        public readonly IOpenIddictApplicationManager _iddictManager;
        public readonly ApplicationDbContext _context;
        public ClientAppRepository(IOpenIddictApplicationManager iddictManager,
            ApplicationDbContext context)
        {
            _iddictManager = iddictManager;
            _context = context;
        }

        public async Task CreateClientAsync(ClientRecord client)
        {
            var application = new OpenIddictApplicationDescriptor
            {
                ClientId = client.ClientId,
                ClientSecret = Guid.NewGuid().ToString(),
                ConsentType = ConsentTypes.Explicit,
                DisplayName = client.DisplayName,
                RedirectUris =
                {
                    new Uri(client.RedirectUri.ToString())
                },
                PostLogoutRedirectUris =
                {
                    new Uri(client.PostLogoutRedirectUris.ToString())
                },
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                   $"{Permissions.Prefixes.Scope}api1"
                }
            };

            await _iddictManager.CreateAsync(application);
        }

        public async Task<IEnumerable<ClientsSummaryRecord>> GetClientsAsync()
        {
            var clients = new List<ClientsSummaryRecord>();
            var result = _iddictManager.ListAsync();
            await foreach (var item in result)
            {
                var clientApp = (OpenIddictEntityFrameworkCoreApplication)item;
                clients.Add(new ClientsSummaryRecord
                {
                    ClientId = clientApp.ClientId,
                    ClientSecret = clientApp.ClientSecret,
                    DisplayName = clientApp.DisplayName,
                    Id = clientApp.Id
                });
            }

            return clients;
        }

        public async Task<EditClientRecord> GetClientAsync(string id)
        {
            var result = (OpenIddictEntityFrameworkCoreApplication)await _iddictManager.FindByIdAsync(id);

            return new EditClientRecord()
            {
                Id = result.Id,
                RedirectUri = result.RedirectUris.ToString(),
                PostLogoutRedirectUris = result.PostLogoutRedirectUris.ToString(),
                ClientId = result.ClientId,
                ClientSecret = result.ClientSecret,
                DisplayName = result.DisplayName,
            };
        }
        public async Task UpdateClientAsync(EditClientRecord client)
        {
            var result = (OpenIddictEntityFrameworkCoreApplication)await _iddictManager.FindByIdAsync(client.Id);

            result.ClientId = client.ClientId;
            result.ClientSecret = client.ClientSecret;
            result.DisplayName = client.DisplayName;
            result.RedirectUris = client.RedirectUri.ToString();
            result.PostLogoutRedirectUris = client.PostLogoutRedirectUris.ToString();

            await _iddictManager.UpdateAsync(result);
        }
    }
}
