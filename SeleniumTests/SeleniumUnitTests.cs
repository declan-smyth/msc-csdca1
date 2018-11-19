using System;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

// xUnit Test Framework
using Xunit;

// Selenium Test Framework
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;



//using OpenQA.Selenium.PhantomJS;

namespace BPCalculator.AcceptanceTest.BloodPressure
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
                

                // Setup Chrome Driver 
               //webDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                

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
               // webDriver.Quit();
            
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while stopping the Web Driver..." + e);
            }
        }

        [Fact]
        public void TestSystolicMaxValue()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");

            if (webDriver == null)
            {
                // Setup Chrome Driver 
                webDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }

            webDriver.Manage().Window.Maximize();

            webDriver.Navigate().GoToUrl(@"https://en.wikipedia.org/wiki/Main_Page");

            Assert.Equal("Wikipedia, the free encyclopedia", webDriver.Title);
           
        }

        [Fact]
        public void TempTest()
        {
            //using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            //{
            //    driver.Navigate().GoToUrl(@"https://automatetheplanet.com/multiple-files-page-objects-item-templates/");
            //    var link = driver.FindElement(By.PartialLinkText("TFS Test API"));
            //    var jsToBeExecuted = $"window.scroll(0, {link.Location.Y});";
            //    ((IJavaScriptExecutor)driver).ExecuteScript(jsToBeExecuted);
            //    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            //    var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("TFS Test API")));
            //    clickableElement.Click();
            //}
        }
    }
}
