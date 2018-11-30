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
using System.Collections.Generic;

namespace BPCalculator.AcceptanceTest.BloodPressure
{
    [TestClass]
    public class BloodPressureCategoryAcceptance
    {
        protected IWebDriver driver;
        private string appURL;
        public TestContext TestContext { get; set; }
        private string browser;

        
        /// <summary>
        /// 
        /// </summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}

        [TestInitialize()]
        public void TestSetup()
        {
            string default_browser = "Chrome";
            //
            // Chrome Options
            //
            var optionsCh = new ChromeOptions
            {
                AcceptInsecureCertificates = true,
                               
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
                this.browser = TestContext.Properties["browser"].ToString() != null ? this.TestContext.Properties["browser"].ToString() : default_browser;

            }
            catch 
            {

                this.browser = default_browser;
            }
            //
            // Initalize the driver based on the browser selected in the build
            // Note: 
            //      You need to pass the location of the driver exeutables on the system
            //      In Azure DevOps the drives are located via environment variables
            //
            try
            {
                switch (browser)
                {
                    case "Chrome":
                        optionsCh.AddArguments(new List<string>() { "headless", "disable-gpu" });
                        driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
                        break;
                    case "Firefox":
                        driver = new FirefoxDriver(Environment.GetEnvironmentVariable("GeckoWebDriver"), optionsFf);
                        break;
                    case "IE":
                        driver = new InternetExplorerDriver(Environment.GetEnvironmentVariable("IEWebDriver"));
                        break;
                    default:
                        optionsCh.AddArguments(new List<string>() { "headless", "disable-gpu" });
                        driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
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
            this.InputFieldUpdates(By.Id("BP_Systolic"), "120");


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "50");
   

            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Name("result"));
            System.Console.WriteLine(elementResult.Text);

            //
            // That the returned values to ensure it matches the expected value
            //
            Assert.IsTrue(elementResult.Text.Contains("Elevated Blood Pressure"));

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_BloodPressure_Check_ErrorMessage_Invalid_Diastolic_Value()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/bloodpressure");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), "100");


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "300");


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Name("errormessage-diastolic"));
            

            //
            // That the returned values to ensure it matches the expected value
            //
            Assert.IsTrue(elementResult.Text.Contains("Invalid Diastolic Value") ,"Verify that an Invalid value error condition is captured");

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_BloodPressure_Check_ErrorMessage_Invalid_Systolic_Value()
        {
            //
            // Navigate to a page for testing
            //
            this.OpenWebPageInBrowser(this.appURL + "/bloodpressure");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), "300");


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "400");


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Name("errormessage-systolic"));


            //
            // That the returned values to ensure it matches the expected value
            //
            Assert.IsTrue(elementResult.Text.Contains("Invalid Systolic Value"), "Verify that an invlaid value error condition is captured");

        }


        #region HelperFunctions

        /// <summary>
        /// Performs a click that will handle stale elements exceptions
        /// 
        /// It will handle a Stale Element Reference Execetion that
        /// can occur where th
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
        /// This method will perform a SendKey on an element to provide input
        /// 
        /// It will handle a Stale Element Reference Execetion that
        /// can occur where the DOM is changed due to dynamic execution of javascript
        /// on the page
        /// <param name="by">Reference to Element</param>
        /// <param name="keys">Key value to send to element</param>
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
        /// This method will perform a Clear action on an element
        /// 
        /// It will handle a Stale Element Reference Execetion that
        /// can occur where the DOM is changed due to dynamic execution of javascript
        /// on the page
        /// </summary>
        /// <param name="by">Element</param>
        /// <returns>True - Success; False - Error Condition</returns>
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
        /// Send a Tab Key to move from an element
        /// </summary>
        /// <param name="by">Reference to an element</param>
        /// <returns></returns>
        public Boolean DoTab(By by)
        {
            Boolean result = false;

            int attempts = 0;

            while (attempts < 2)
            {
                try
                {
                    driver.FindElement(by).SendKeys(Keys.Tab);
                    result = true;
                    break;
                }
                catch (StaleElementReferenceException) { }

            }
            return result;
        }

        /// <summary>
        /// Helper method to update the input fields on the form
        /// The method will perform the followng:
        /// - Click into the field
        /// - Clear the Field
        /// - Send Value to field
        /// - Click out
        /// </summary>
        /// <param name="by">Reference to the input element</param>
        /// <param name="input">Input value</param>
        public void InputFieldUpdates(By by, String input)
        {
            DoClick(by);
            DoClear(by);
            DoClick(by);

            if (!input.Equals(string.Empty))
            {
                DoSendKey(by, input);
            }

            DoTab(by);
        }

        /// <summary>
        /// Helper methond to Open a Web Page in a Browser with the following attributes
        /// - Browser Window is Maximised
        /// - Timeout is set to 30 Seconds
        /// </summary>
        /// <param name="pageToOpen">Url to open</param>
        public void OpenWebPageInBrowser(string pageToOpen)
        {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            driver.Navigate().GoToUrl(pageToOpen);
        }


        #endregion

    }
}
