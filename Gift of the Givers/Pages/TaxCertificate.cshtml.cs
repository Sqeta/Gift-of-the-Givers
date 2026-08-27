using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers.Pages
{
    public class TaxCertificateModel : PageModel
    {
        public string DonorName { get; set; } = "Anonymous Donor";

        public string DonationType { get; set; } = "One-Time";

        public string Amount { get; set; } = "0.00";

        public string Currency { get; set; } = "ZAR";

        public string DonationDate { get; set; } = "";

        public string ReferenceNumber { get; set; } = "";

        public void OnGet()
        {
            DonorName = TempData["DonorName"]?.ToString() ?? "Anonymous Donor";

            DonationType = TempData["DonationType"]?.ToString() ?? "One-Time";

            Amount = TempData["Amount"]?.ToString() ?? "0.00";

            Currency = TempData["Currency"]?.ToString() ?? "ZAR";

            DonationDate = DateTime.Now.ToString("dd MMMM yyyy");

            ReferenceNumber =
                "GOTG-" +
                DateTime.Now.ToString("yyyyMMdd") +
                "-" +
                Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }
}