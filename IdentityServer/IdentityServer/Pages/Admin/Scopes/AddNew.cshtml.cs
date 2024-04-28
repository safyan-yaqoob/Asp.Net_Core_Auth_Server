using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IdentityServer.Data.Repository;
using IdentityServer.Records;

namespace IdentityServer.Pages.Scopes
{
    public class AddNewModel : PageModel
    {
        private readonly ScopesRepository _repository;

        public AddNewModel(ScopesRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public ScopeRecord InputModel { get; set; } = default!;

        public bool Created { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _repository.AddScopeAsync(InputModel);
                Created = true;
            }

            return Page();
        }
    }
}
