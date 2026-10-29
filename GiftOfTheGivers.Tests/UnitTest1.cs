
using GiftOfTheGivers.Helpers;
using Gift_of_the_Givers.Pages;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class UnitTest1
    {
        // TEST 1: Tax Certificate Number Formatting
        [Fact]
        public void GenerateCertificateNumber_ShouldUseCorrectFormat()
        {
            // Arrange
            int donationId = 123;

            // Act
            string certificateNumber =
                TaxCertificateHelper.GenerateCertificateNumber(donationId);

            // Assert
            Assert.Equal("GOTG-TAX-000123", certificateNumber);
        }

        // TEST 2: Reject Invalid Donation Amount
        [Fact]
        public void DonationAmount_BelowMinimum_ShouldBeRejected()
        {
            // Arrange
            decimal amount = 0;

            // Act
            bool result =
                DonateModel.IsValidDonationAmount(amount);

            // Assert
            Assert.False(result);
        }

        // TEST 3: Accept Valid Donation Amount
        [Fact]
        public void DonationAmount_AboveMinimum_ShouldBeAccepted()
        {
            // Arrange
            decimal amount = 500;

            // Act
            bool result =
                DonateModel.IsValidDonationAmount(amount);

            // Assert
            Assert.True(result);
        }

        // TEST 4: Calculate Donation Total
        [Fact]
        public void CalculateDonationTotal_ShouldReturnCorrectTotal()
        {
            // Arrange
            decimal amount = 200m;
            int numberOfDonations = 3;
            decimal expectedTotal = 600m;

            // Act
            decimal actualTotal =
                TaxCertificateHelper.CalculateDonationTotal(
                    amount,
                    numberOfDonations
                );

            // Assert
            Assert.Equal(expectedTotal, actualTotal);
        }
    }
}
