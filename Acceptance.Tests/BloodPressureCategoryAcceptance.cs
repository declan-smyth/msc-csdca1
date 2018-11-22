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
            // Chrome Options
            //
            var optionsCh = new ChromeOptions
            {
                AcceptInsecureCertificates = true
            };

            //
            // Firefox Options
            //
            var optionsFf = new FirefoxOptions
            {
                AcceptInsecureCertificates = true
            };

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
                this.browser = TestContext.Properties["browser"].ToString() != null ? this.TestContext.Properties["browser"].ToString() : "Firefox";

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
                    driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
                    break;
                case "Firefox":

                    driver = new FirefoxDriver(Environment.GetEnvironmentVariable("GekoWebDriver"), optionsFf);
                    break;
                case "IE":
                    driver = new InternetExplorerDriver(Environment.GetEnvironmentVariable("IEWebDriver"));
                    break;
                default:
                    driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
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
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/bloodpressure");

            Assert.IsTrue(driver.Title.Contains("Blood Pressure Calculator"),"verify the title of the page contains Blood Pressure");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_Launch_BloodPressurePage()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/bloodpressure");
          
            Assert.IsTrue(driver.Title.Contains(""), "Verify title of the Blood Pressure Page");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_Launch_About()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/About");

            Assert.IsTrue(driver.Title.Contains("About"), "Verify title of the About Page");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_About_Check_HesderOnPage()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/About");

            var headerSelector = By.TagName("h3");
            Assert.IsTrue(driver.FindElement(headerSelector).Text.Contains("Continuous Assessment 1"), "Verift that the heading on the page contains CA 1");

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_About_Check_AuthorName()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/About");

            var headerSelector = By.TagName("p");
            Assert.IsTrue(driver.FindElement(headerSelector).Text.Contains("Declan Smyth"), "Verift that the authors name is on the about page");

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_BloodPressure_Check_InputValues()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/bloodpressure");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), 120);


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "50");
   


            var elementResult = driver.FindElement(By.ClassName("form-group"));
            System.Console.WriteLine(elementResult.Text);

            Assert.Equals("Elevated Blood Pressure", elementResult.Text);

        }



        #region HelperFunctions

        /// <summary>
        /// Performs a click that will handle stale elements exceptions
        /// </summary>
        /// <param name="by">Element that is being used</param>
        /// <returns>boolean</returns>
        public Boolean DoClick (By by)
        {
            Boolean result = false;

            int attempts = 0;

            while (attempts < 2)
            {
                try
                {
                    driver.FindElement(by).Click();
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException) {      }

            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="by"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Boolean DoSendKey(By by, string keys)
        {
            Boolean result = false;

            int attempts = 0;

            while (attempts < 2)
            {
                try
                {
                    driver.FindElement(by).SendKeys (keys);
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException) { }

            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public Boolean DoClear(By by)
        {
            Boolean result = false;

            int attempts = 0;

            while (attempts < 2)
            {
                try
                {
                    driver.FindElement(by).Clear();
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException) { }

            }
            return result;
        }

        /// <summary>
        /// Helper method to update the input fieds on a form
        /// </summary>
        /// <param name="by"></param>
        /// <param name="input"></param>
        public void InputFieldUpdates(By by, String input)
        {
            DoClick(by);
            DoClear(by);
            DoClick(by);

            if (!input.Equals(string.Empty))
            {
                DoSendKey(by, input);
            }

            DoClick(by);
        }

        public void OpenWebPageInBrowser(string pageToOpen)
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(pageToOpen);
        }


        #endregion

    }
}
