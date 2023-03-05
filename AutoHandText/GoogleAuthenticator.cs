using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHandText
{
    internal class GoogleAuthenticator
    {
        private static string authenticateURL = "https://accounts.google.com/";
        private IWebDriver driver;
        
        public GoogleAuthenticator(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void Authenticate()
        {
            driver.Navigate().GoToUrl(authenticateURL);
            driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[1]/div/form/span/section/div/div/div[1]/div/div[1]/div/div[1]/input"))
                .SendKeys(Program.AuthData.login);
            driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[2]/div/div[1]/div/div/button"))
                .Click();
            driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[1]/div/form/span/section[2]/div/div/div[1]/div[1]/div/div/div/div/div[1]/div/div[1]/input"))
                .SendKeys(Program.AuthData.password);
            ClickButtonAfterDOMUpdate(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[2]/div/div[1]/div/div/button"));
            new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                .Until(ExpectedConditions.UrlMatches("https://myaccount.google.com/"));
        }

        private void ClickButtonAfterDOMUpdate(By locator)
        {
            try
            {
                driver.FindElement(locator).Click();
            }
            catch (StaleElementReferenceException e)
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(locator));
                driver.FindElement(locator).Click();
            }
        }
    }
}
