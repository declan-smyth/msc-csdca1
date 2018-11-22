using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;


namespace BPCalculator.AcceptanceTest.BloodPressure
{
    [TestClass]
    public class BloodPressureCategoryAcceptance
    {
        private IWebDriver driver;
        private string appURL;
        private TestContext testContextInstance;
        private string browser;

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
            //
            // Setup the URL to Target, this can be based on the build settings
            // Default: Dev Test Slot
            //
            try
            {
                if (this.TestContext.Properties["webAppUri"] != null)
                {
                    this.appURL = this.TestContext.Properties["webAppUri"].ToString();
                }
                else
                {
                    this.appURL = "https://bpcalculator-dev-as-dev.azurewebsites.net";
                }
            }
            catch
            {
                this.appURL = "https://bpcalculator-dev-as-dev.azurewebsites.net";
            }

            //
            // Get the browser to use from the Test Context, otherwise use Chrome as the default
            //
            try
            {
                this.browser = this.TestContext.Properties["browser"].ToString() != null ? this.TestContext.Properties["browser"].ToString() : "Firefox";

            }
            catch 
            {

                this.browser = "Firefox";
            }
            //
            // Initalize the driver based on the browser selected in the build
            // Note: 
            //      You need to pass the location of the driver exeutables on the system
            //      In Azure DevOps the drives are located via environment variables
            //
            switch (browser)
            {
                case "Chrome":
                    var optionsCh = new ChromeOptions
                    {
                        AcceptInsecureCertificates = true
                    };

                    driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
                    break;
                case "Firefox":

                    var optionsFf = new FirefoxOptions
                    {
                        AcceptInsecureCertificates = true
                    };

                    driver = new FirefoxDriver(Environment.GetEnvironmentVariable("GekoWebDriver"), optionsFf);
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
            try
            {
                driver.Quit();
            }
            catch
            {
                // To Nothing here as we are cleaing up the test
            }
        }


        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_Launch_WebSite()
        {

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(this.appURL);
                        
            Assert.IsTrue(driver.Title.Contains("Blood Pressure Calculator"),"verify the title of the page contains Blood Pressure");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_Launch_BloodPressurePage()
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(this.appURL + "/bloodpressure");

            Assert.IsTrue(driver.Title.Contains(""), "Verify title of the Blood Pressure Page");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_Launch_About()
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(this.appURL + "/About");

           

            Assert.IsTrue(driver.Title.Contains("About"), "Verify title of the About Page");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_About_Check_HesderOnPage()
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(this.appURL + "/About");
            
            var headerSelector = By.TagName("h3");
            Assert.IsTrue(driver.FindElement(headerSelector).Text.Contains("Continuous Assessment 1"), "Verift that the heading on the page contains CA 1");

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_About_Check_AuthorName()
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(this.appURL + "/About");

            var headerSelector = By.TagName("p");
            Assert.IsTrue(driver.FindElement(headerSelector).Text.Contains("Declan Smyth"), "Verift that the authors name is on the about page");

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_BloodPressure_CheckLinks()
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(this.appURL + "/bloodpressure");

            var elementSystolic = driver.FindElement(By.Id("BP_Systolic"));
            //elementSystolic.Clear();
            elementSystolic.Click();
            elementSystolic.SendKeys("120");

            var elementDisastolic = driver.FindElement(By.Id("BP_Diastolic"));
            //elementDisastolic.Clear();
            elementDisastolic.Click();
            elementDisastolic.SendKeys("50");
                       
            var elementResult = driver.FindElement(By.ClassName("form-group"));

            Assert.Equals("Elevated Blood Pressue", elementResult.Text);

        }

    }
}
