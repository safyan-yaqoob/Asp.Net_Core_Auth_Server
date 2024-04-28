using OpenIddict.Abstractions;
using OpenIddict.EntityFrameworkCore.Models;
using IdentityServer.Records;

namespace IdentityServer.Data.Repository
{
    public class ScopesRepository
    {
        public readonly IOpenIddictScopeManager _scopesManager;
        public ScopesRepository(IOpenIddictScopeManager scopesManager)
        {
            _scopesManager = scopesManager;
        }

        public async Task AddScopeAsync(ScopeRecord scope)
        {
            var apiScope = await _scopesManager.FindByNameAsync(scope.Name);

            if (apiScope != null)
            {
                await _scopesManager.DeleteAsync(apiScope);
            }

            await _scopesManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                DisplayName = scope.DisplayName,
                Name = scope.Name,
                Resources =
                {
                    scope.Resources
                }
            });
        }

        public async Task<IEnumerable<ScopeSummaryRecord>> GetScopesAsync()
        {
            var scopes = new List<ScopeSummaryRecord>();
            var result = _scopesManager.ListAsync();
            await foreach (var item in result)
            {
                var scope = (OpenIddictEntityFrameworkCoreScope)item;
                scopes.Add(new ScopeSummaryRecord
                {
                    DisplayName = scope.DisplayName,
                    Name = scope.Name,
                    Id = scope.Id
                });
            }

            return scopes;
        }

        public async Task<EditScopeRecord> GetScopeAsync(string id)
        {
            var result = (OpenIddictEntityFrameworkCoreScope)await _scopesManager.FindByIdAsync(id);
            if (result != null)
            {
                return new EditScopeRecord()
                {
                    Id = result.Id,
                    Name = result.Name,
                    DisplayName = result.DisplayName,
                    Resources = result.Resources
                };
            }

            return null;
        }
        public async Task UpdateScopeAsync(EditScopeRecord scope)
        {
            var result = (OpenIddictEntityFrameworkCoreScope)await _scopesManager.FindByIdAsync(scope.Id);

            if (result != null)
            {
                result.Name = scope.Name;
                result.DisplayName = scope.DisplayName;
                result.Resources = scope.Resources;
                await _scopesManager.UpdateAsync(result);
            }
        }
    }
}
