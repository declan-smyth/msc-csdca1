﻿using System;
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

        
        [TestInitialize()]
        public void TestSetup()
        {
            string default_browser = "Firefox";
            //
            // Chrome Options
            //
            var optionsCh = new ChromeOptions
            {
                AcceptInsecureCertificates = true,
                               
            };
            optionsCh.AddArguments(new List<string>() { "headless", "disable-gpu" });

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
                        driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
                        break;
                    case "Firefox":
                        driver = new FirefoxDriver(Environment.GetEnvironmentVariable("GeckoWebDriver"), optionsFf);
                        break;
                    default:
                        driver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), optionsCh);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error Occured that casued an exception:" + ex.Message);

                System.Console.WriteLine("ChromeWebDriver: " + Environment.GetEnvironmentVariable("ChromeWebDriver"));
                System.Console.WriteLine("GeckoWebDriver: " + Environment.GetEnvironmentVariable("GeckoWebDriver"));
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
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            Assert.IsTrue(driver.Title.Contains("Blood Pressure Calculator"),"verify the title of the page contains Blood Pressure");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_Launch_BloodPressurePage()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            Assert.IsTrue(driver.Title.Contains(""), "Verify title of the Blood Pressure Page");
        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_Launch_About()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/About"), "Validate that the page opens in the browser");

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
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/About"), "Validate that the page opens in the browser");

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
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/About"), "Validate that the page opens in the browser");

 
            var headerSelector = By.TagName("p");
            Assert.IsTrue(driver.FindElement(headerSelector).Text.Contains("Declan Smyth"), "Verift that the authors name is on the about page");

        }

        [DataRow(124, 79)]
        [DataTestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_BloodPressure_Check_Elevated_InputValues(int s, int d)
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), s.ToString());


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), d.ToString());


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("results"));
            System.Console.WriteLine(elementResult.Text);

            //
            // That the returned values to ensure it matches the expected value
            //
            var expected_result = "ELEVATED";
            Assert.IsTrue(elementResult.Text.Contains(expected_result));

        }


        [DataRow(130,79)]
        [DataRow(139,85)]
        [DataRow(140,79)]
        [DataRow(140,90)]
        [DataRow(180,79)]
        [DataTestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_BloodPressure_Check_HighPressure_InputValues(int s, int d)
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), s.ToString());


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), d.ToString());


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("results"));
            System.Console.WriteLine(elementResult.Text);

            //
            // That the returned values to ensure it matches the expected value
            //
            var expected_result = "HIGH BLOOD PRESSURE";
            Assert.IsTrue(elementResult.Text.Contains(expected_result));

        }


        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_BloodPressure_Check_HighPressure_WarningMessage()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), "130");


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "79");


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("alertmessage"));
            System.Console.WriteLine(elementResult.Text);

            //
            // Check the the message contains the word warning
            //
            Assert.IsTrue(elementResult.Text.Contains("Warning!"));

        }

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_BloodPressure_Check_HighPressure_MessageType()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), "130");


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "79");


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("alertmessage"));
            System.Console.WriteLine(elementResult.Text);
            System.Console.WriteLine(elementResult.GetAttribute("class").ToString());
            
            //
            // Check the the message contains the word warning
            //
            Assert.IsTrue(elementResult.GetAttribute("class").Contains("alert-warning"));

        }


        [DataRow(181, 122)]
        [DataRow(70, 40)]
        [DataTestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(1)]
        public void Test_BloodPressure_Check_MedicalAttention_Message(int s, int d)
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), s.ToString());


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), d.ToString());


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("alertmessage"));
            System.Console.WriteLine(elementResult.Text);

            //
            // Check the the message contains the word warning
            //
            var expected_result = "seek medical attendion";
            Assert.IsTrue(elementResult.Text.Contains(expected_result));
          }

        [DataRow(181, 122)]
        [DataRow(70, 40)]
        [DataTestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_BloodPressure_Check_MedicalAttention_MessageType(int s, int d)
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), s.ToString());


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), d.ToString());


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("alertmessage"));
            System.Console.WriteLine(elementResult.Text);

            //
            // Check the the message contains the word warning
            //
            Assert.IsTrue(elementResult.GetAttribute("class").Contains("alert-danger"));

        }


        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_BloodPressure_Check_ErrorMessage_Invalid_Diastolic_Value()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");

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
        [Priority(2)]
        public void Test_BloodPressure_Check_ErrorMessage_Invalid_Systolic_Value()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");
            

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

        [TestMethod]
        [TestCategory("AcceptanceTest")]
        [Priority(2)]
        public void Test_BloodPressue_Check_ErrorMessage_SystolicGreaterDiastolic()
        {
            //
            // Navigate to a page for testing
            //
            Assert.IsTrue(this.OpenWebPageInBrowser(this.appURL + "/bloodpressure"), "Validate that the page opens in the browser");


            //
            // Complete the Values for the Systolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Systolic"), "100");


            //
            // Complete the values for the Diastolic Input Field
            //
            this.InputFieldUpdates(By.Id("BP_Diastolic"), "120");


            //
            // Get the result from the element being tested
            //
            var elementResult = driver.FindElement(By.Id("validation-summary-errors"));


            //
            // That the returned values to ensure it matches the expected value
            //
            Assert.IsTrue(elementResult.Text.Contains("Systolic must be greater than Diastolic"), "Verify tha an error message is displayed if the Systolic is less than Diastolic");

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
        public Boolean OpenWebPageInBrowser(string pageToOpen)
        {
            Boolean result = false;

            if (driver != null)
            {
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                driver.Navigate().GoToUrl(pageToOpen);
                result = true;
            }

            return result;
        }


        #endregion

    }
}
