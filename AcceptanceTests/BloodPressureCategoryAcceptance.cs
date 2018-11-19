using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;


namespace BPCalculator.AcceptanceTest.BloodPressure
{
    [TestClass]
    public class BloodPressureCategoryAcceptance
    {
        private IWebDriver driver;
        private string appURL;
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize()]
        public void TestSetup()
        {
            appURL = "https://bpcalculator-dev-as-dev.azurewebsites.net";

            string browser = "Firefox";
            switch (browser)
            {
                case "Chrome":
                    driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"));
                    break;
                case "Firefox":
                    driver = new FirefoxDriver(Environment.GetEnvironmentVariable("GekoWebDriver"));
                    break;
                case "IE":
                    driver = new InternetExplorerDriver(Environment.GetEnvironmentVariable("IEWebDriver"));
                    break;
                default:
                    driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"));
                    break;
            }

        }

        [TestCleanup()]
        public void TestCleanup()
        {
            driver.Quit();
        }


        [TestMethod]
        [TestCategory("Chrome")]
        public void TheBingSearchTest()
        {
            driver.Navigate().GoToUrl(appURL + "/");
            driver.FindElement(By.Id("sb_form_q")).SendKeys("Azure Pipelines");
            driver.FindElement(By.Id("sb_form_go")).Click();
            driver.FindElement(By.XPath("//ol[@id='b_results']/li/h2/a/strong[3]")).Click();

            Assert.IsTrue(driver.Title.Contains("Azure Pipelines"), "Verified title of the page");
        }
    }
}
