using System.ComponentModel.DataAnnotations;
using Gift_of_the_Givers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class VolunteerModel : PageModel
    {
        private readonly PrototypeStore _store;

        public VolunteerModel(PrototypeStore store)
        {
            _store = store;
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
        [Required(ErrorMessage = "Please enter your contact number.")]
        public string PhoneNumber { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please select your main skill.")]
        public string Skills { get; set; } = string.Empty;


        [BindProperty]
        [Required(ErrorMessage = "Please select your availability.")]
        public string Availability { get; set; } = string.Empty;


        [BindProperty]
        public string? AdditionalInformation { get; set; }


        [BindProperty]
        [Range(
            typeof(bool),
            "true",
            "true",
            ErrorMessage = "Please confirm the volunteer declaration."
        )]
        public bool TermsAccepted { get; set; }


        public bool Submitted { get; set; }


        public void OnGet()
        {
            Submitted = false;
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Submitted = false;

                return Page();
            }


            _store.Volunteers.Add(
                new VolunteerSignup
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    PhoneNumber = PhoneNumber,
                    Skills = Skills,
                    Availability = Availability,
                    AdditionalInformation = AdditionalInformation,
                    DateSubmitted = DateTime.Now
                }
            );


            Submitted = true;

            ModelState.Clear();


            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Skills = string.Empty;
            Availability = string.Empty;
            AdditionalInformation = string.Empty;
            TermsAccepted = false;


            return Page();
        }
    }
}