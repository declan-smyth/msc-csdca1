using System;
using System.Security.Permissions;

// xUnit Test Framework
using Xunit;

// Selenium Test Framework
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.PhantomJS;



namespace SeleniumTests
{
    public class SeleniumUnitTests : IDisposable
    {
        public String webAppUri = "";

        public IWebDriver webDriver;

        public SeleniumUnitTests()
        {
            try
            {
                // Setup the browser to use during the testing
                ChromeOptions options = new ChromeOptions();
                //options.AddArgument("--headless");
                webDriver = new ChromeDriver(options);

                // Get the WebappURI

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while starting the Web Driver..." + e);
            } 

        }

        /// <summary>
        /// Implement the Dispose method for cleardown of the browser instance
        /// </summary>
        public void Dispose()
        {
            try
            {
                webDriver.Quit();
            
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while stopping the Web Driver..." + e);
            }
        }

        [Fact]
        public void TestSystolicMaxValue()
        {
            
            // Navigate to the page
            webDriver.Navigate().GoToUrl("www.google.ie");
            
            IWebElement query = webDriver.FindElement(By.Name("q"));
            query.SendKeys("Cheese");
            System.Console.WriteLine("Page title is: " + webDriver.Title);
            webDriver.Quit();




            // Get the Systolic Element
            //IWebElement systolicElement = webDriver.FindElement(By.Id("BP.Systolic"));

            //Get the Distoloic Element
            //IWebElement diastolicElement = webDriver.FindElement(By.ClassName(""))

            // Send 10 to Systolic Element
            // systolicElement.SendKeys("60");


        }
    }
}
