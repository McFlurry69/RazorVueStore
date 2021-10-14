using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shop.UI.Pages.Accounts
{
    public class Login : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public Login(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        
        [BindProperty] public LoginViewModel Input { get; set; }
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, false);

            if (result.Succeeded)
                return RedirectToPage("/Admin/Index");
            return Page();
        }
    }

    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}