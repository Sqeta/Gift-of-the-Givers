using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class DonateModel : PageModel
    {
        [BindProperty]
        public string DonationType { get; set; }

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public string Currency { get; set; }

        [BindProperty]
        public string DonorName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public bool IsAnonymous { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            TempData["DonationType"] = DonationType;
            TempData["Amount"] = Amount.ToString("0.00");
            TempData["Currency"] = Currency;
            TempData["DonorName"] = IsAnonymous ? "Anonymous Donor" : DonorName;
            TempData["Email"] = Email;

            return RedirectToPage("/TaxCertificate");
        }
    }
}