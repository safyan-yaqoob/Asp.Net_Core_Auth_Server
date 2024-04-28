using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Data.Repository;
using IdentityServer.Records;

namespace IdentityServer.Pages.Clients
{
    public class AddClient : PageModel
    {
        private readonly ClientAppRepository _repository;

        public AddClient(ClientAppRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public ClientRecord InputModel { get; set; } = default!;

        public bool Created { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _repository.CreateClientAsync(InputModel);
                Created = true;
            }

            return Page();
        }
    }
}
