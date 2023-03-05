using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutoHandText
{
    internal class HandTextScraper
    {
        private const string baseURL = "http://hand-text.ru/";
        private IWebDriver driver;

        public void Start(IWebDriver driver)
        {
            this.driver = driver;
            driver.Navigate().GoToUrl(baseURL);
        }

        public async Task MakeImages(string text)
        {
            driver.FindElement(By.ClassName("drop-list__front")).Click();
            var fontOptions = driver.FindElements(By.ClassName("drop-list__back-item"));
            fontOptions[0].Click();
            var textBox = driver.FindElement(By.ClassName("input"));
            textBox.Clear();
            foreach(var symbol in text)
            {
                textBox.SendKeys(symbol.ToString());
                Thread.Sleep(1);
            }
            //textBox.SendKeys(text);
            new WebDriverWait(driver, TimeSpan.FromSeconds(15))
                .Until(ExpectedConditions.TextToBePresentInElementValue(textBox, text));
            driver.FindElement(By.ClassName("button__converter")).Click();
            driver.FindElement(By.ClassName("button__watermark")).Click();
        }
    }
}
