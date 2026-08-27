using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                ErrorMessage = "Incorrect email address or password.";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                Password,
                RememberMe,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ErrorMessage = "Incorrect email address or password.";
                return Page();
            }

            if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                return RedirectToPage("/EmployeeDashboard");
            }

            return RedirectToPage("/Index");
        }
    }
}