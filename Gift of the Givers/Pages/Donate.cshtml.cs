
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class DonateModel : PageModel
    {
        [BindProperty]
        public string DonationType { get; set; } = string.Empty;

        [BindProperty]
        public decimal Amount { get; set; }

        [BindProperty]
        public string Currency { get; set; } = string.Empty;

        [BindProperty]
        public string DonorName { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public bool IsAnonymous { get; set; }

        // Checks whether the donation amount is valid.
        public static bool IsValidDonationAmount(decimal amount)
        {
            return amount >= 1;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Prevent donations below the minimum amount.
            if (!IsValidDonationAmount(Amount))
            {
                ModelState.AddModelError(
                    nameof(Amount),
                    "Donation amount must be at least R1."
                );

                return Page();
            }

            // Store donation details for the certificate page.
            TempData["DonationType"] = DonationType;
            TempData["Amount"] = Amount.ToString("0.00");
            TempData["Currency"] = Currency;

            TempData["DonorName"] = IsAnonymous
                ? "Anonymous Donor"
                : DonorName;

            TempData["Email"] = Email;

            // Continue to the tax certificate page.
            return RedirectToPage("/TaxCertificate");
        }
    }
}
