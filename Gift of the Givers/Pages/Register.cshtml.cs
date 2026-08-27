using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare(
            "Password",
            ErrorMessage = "Passwords do not match."
        )]
        public string ConfirmPassword { get; set; } = string.Empty;


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

            var existingUser =
                await _userManager.FindByEmailAsync(Email);

            if (existingUser != null)
            {
                ErrorMessage =
                    "An account with this email address already exists.";

                return Page();
            }

            var user = new IdentityUser
            {
                UserName = Email,
                Email = Email
            };

            var result =
                await _userManager.CreateAsync(
                    user,
                    Password
                );

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(
                    user,
                    "Donor"
                );

                TempData["SuccessMessage"] =
                    "Your donor account was created successfully. Please log in.";

                return RedirectToPage("/Login");
            }

            ErrorMessage =
                string.Join(
                    " ",
                    result.Errors.Select(
                        error => error.Description
                    )
                );

            return Page();
        }
    }
}   