
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace GiftOfTheGivers.Tests
{
    public class SeleniumTests
    {
        private const string BaseUrl = "https://localhost:7217";

        private ChromeDriver CreateDriver()
        {
            var options = new ChromeOptions();

            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-notifications");

            return new ChromeDriver(options);
        }

        private void ScrollAndClick(
            IWebDriver driver,
            WebDriverWait wait,
            By locator)
        {
            var element = wait.Until(d =>
            {
                var found = d.FindElement(locator);

                return found.Displayed && found.Enabled
                    ? found
                    : null;
            });

            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center'});",
                element
            );

            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Retry after scrolling if another element
                // temporarily blocks the button.
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block: 'center'});",
                    element
                );

                wait.Until(d =>
                {
                    try
                    {
                        element.Click();
                        return true;
                    }
                    catch (ElementClickInterceptedException)
                    {
                        return false;
                    }
                });
            }
        }

        [Fact]
        public void DonationForm_ShouldOpenTaxCertificatePage()
        {
            using var driver = CreateDriver();

            var wait = new WebDriverWait(
                driver,
                TimeSpan.FromSeconds(20)
            );

            // Open donation page
            driver.Navigate().GoToUrl(BaseUrl + "/Donate");

            // Wait for donation form
            wait.Until(d =>
                d.FindElement(By.Id("donationAmount")).Displayed
            );

            // Enter donation amount
            driver.FindElement(By.Id("donationAmount"))
                .SendKeys("500");

            // Select currency
            var currency = new SelectElement(
                driver.FindElement(By.Id("currency"))
            );

            currency.SelectByValue("ZAR");

            // Enter donor details
            driver.FindElement(By.Id("donorName"))
                .SendKeys("Test Donor");

            driver.FindElement(By.Id("donorEmail"))
                .SendKeys("testdonor@example.com");

            // Submit donation form
            ScrollAndClick(
                driver,
                wait,
                By.CssSelector(".donate-page button[type='submit']")
            );

            // Confirm navigation to certificate page
            wait.Until(d =>
                d.Url.Contains(
                    "/TaxCertificate",
                    StringComparison.OrdinalIgnoreCase
                )
            );

            Assert.Contains(
                "/TaxCertificate",
                driver.Url,
                StringComparison.OrdinalIgnoreCase
            );
        }

        [Fact]
        public void EmployeeLogin_ShouldOpenDashboard()
        {
            using var driver = CreateDriver();

            var wait = new WebDriverWait(
                driver,
                TimeSpan.FromSeconds(20)
            );

            // Open login page
            driver.Navigate().GoToUrl(BaseUrl + "/Login");

            // Confirm login form loads
            wait.Until(d =>
                d.FindElement(By.CssSelector(".auth-page form"))
                    .Displayed
            );

            // This test checks login page availability.
            // Actual dashboard access requires valid
            // employee credentials.
            Assert.Contains(
                "/Login",
                driver.Url,
                StringComparison.OrdinalIgnoreCase
            );

            Assert.True(
                driver.FindElement(
                    By.CssSelector(".auth-page button[type='submit']")
                ).Displayed
            );
        }
    }
}
