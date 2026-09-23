using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Functions
{
    public class GenerateTaxCertificate
    {
        private readonly ILogger<GenerateTaxCertificate> _logger;

        public GenerateTaxCertificate(
            ILogger<GenerateTaxCertificate> logger)
        {
            _logger = logger;
        }

        [Function("GenerateTaxCertificate")]
        public IActionResult Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "get",
                "post")] HttpRequest req)
        {
            _logger.LogInformation(
                "Generating a dummy tax certificate.");

            string donorName =
                req.Query["donorName"].FirstOrDefault() ?? "Anonymous Donor";

            string amount =
                req.Query["amount"].FirstOrDefault() ?? "0.00";

            string currency =
                req.Query["currency"].FirstOrDefault() ?? "ZAR";

            string certificateNumber =
                $"GOTG-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

            var certificate = new
            {
                CertificateNumber = certificateNumber,
                Organisation = "Gift of the Givers Foundation",
                DonorName = donorName,
                Amount = amount,
                Currency = currency,
                IssuedDate = DateTime.Now.ToString("yyyy-MM-dd"),
                Message = "Dummy tax certificate generated successfully."
            };

            return new OkObjectResult(certificate);
        }
    }
}