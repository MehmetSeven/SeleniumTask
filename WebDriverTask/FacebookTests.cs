using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebDriverTask
{
    [TestClass]
    public class FacebookTests
    {
        private IWebDriver driver;
        [TestInitialize]
        public void TestStart()
        {
            driver = new FirefoxDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://www.facebook.com");
        }

        [TestCleanup]
        public void TestEnd()
        {
            driver.Quit();
        }
        [TestMethod]
        public void FacebookLogin()
        {
            driver.FindElement(By.Id("email")).SendKeys("test_gczgocu_user@tfbnw.net");
            driver.FindElement(By.Id("pass")).SendKeys("testuser");
            driver.FindElement(By.Id("loginbutton")).Click();
        }
    }
}
