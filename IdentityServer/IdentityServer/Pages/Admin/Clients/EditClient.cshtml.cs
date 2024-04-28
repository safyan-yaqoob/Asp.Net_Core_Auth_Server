using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Data.Repository;
using IdentityServer.Records;

namespace IdentityServer.Pages.Clients
{
    public class EditClient : PageModel
    {
        private readonly ClientAppRepository _repository;
        public EditClient(ClientAppRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public EditClientRecord InputModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var model = await _repository.GetClientAsync(id);

            if (model == null)
            {
                return RedirectToPage("/Admin/Clients/Index");
            }
            else
            {
                InputModel = model;
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateClientAsync(InputModel);
                return RedirectToPage("/Admin/Clients/Index");
            }

            return Page();
        }
    }
}
