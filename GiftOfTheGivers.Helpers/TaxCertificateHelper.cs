
using System;

namespace GiftOfTheGivers.Helpers
{
    public static class TaxCertificateHelper
    {
        // Generates a unique-looking reference number
        // using the supplied donation ID.
        public static string GenerateCertificateNumber(int donationId)
        {
            return $"GOTG-TAX-{donationId:D6}";
        }

        // Calculates the total value of multiple donations.
        public static decimal CalculateDonationTotal(
            decimal amount,
            int numberOfDonations)
        {
            if (amount < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Donation amount must be at least 1."
                );
            }

            if (numberOfDonations < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(numberOfDonations),
                    "Number of donations must be at least 1."
                );
            }

            return amount * numberOfDonations;
        }
    }
}
