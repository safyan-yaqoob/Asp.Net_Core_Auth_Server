using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Data.Repository;
using IdentityServer.Records;

namespace IdentityServer.Pages.Clients
{
    public class IndexModel : PageModel
    {
        private readonly ClientAppRepository _repository;

        public IndexModel(ClientAppRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ClientsSummaryRecord> Clients { get; private set; } = default!;
        public string? Filter { get; set; }

        public async Task OnGetAsync(string? filter)
        {
            Filter = filter;
            Clients = await _repository.GetClientsAsync();
        }
    }
}
