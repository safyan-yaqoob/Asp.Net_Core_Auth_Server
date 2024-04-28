using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Data.Repository;
using IdentityServer.Records;

namespace IdentityServer.Pages.Scopes
{
    public class EditModel : PageModel
    {
        private readonly ScopesRepository _repository;
        public EditModel(ScopesRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public EditScopeRecord InputModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var model = await _repository.GetScopeAsync(id);

            if (model == null)
            {
                return RedirectToPage("/Admin/Scopes/Index");
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
                await _repository.UpdateScopeAsync(InputModel);
                return RedirectToPage("/Admin/Scopes/Index");
            }

            return Page();
        }
    }
}
