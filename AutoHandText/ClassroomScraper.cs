using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AutoHandText
{
    internal class ClassroomScraper
    {
        private static string classroomURL = "https://classroom.google.com";
        private static string taskKeyword = "Лекція";
        private IWebDriver driver;

        public ClassroomScraper(IWebDriver driver)
        {
            this.driver = driver;
        }

        public List<string> GetURLs()
        {
            new GoogleAuthenticator(driver).Authenticate();
            GoToTasks();
            return ParseTasksList();
        }

        private void GoToTasks()
        {
            //new WebDriverWait(driver, TimeSpan.FromSeconds(5))
            //    .Until(ExpectedConditions.UrlMatches("https://myaccount.google.com/"));
            driver.Navigate().GoToUrl(classroomURL);
            driver.FindElement(By.CssSelector(".WpHeLc.VfPpkd-mRLv6.VfPpkd-RLmnJb")).Click();
            driver.FindElement(By.CssSelector(".VfPpkd-Bz112c-LgbsSe.yHy1rc.eT1oJ.mN1ivc.oxacD.rKc6P")).Click();
            driver.FindElement(By.XPath("/html/body/c-wiz[2]/div/div/div[5]/div[2]/div[1]/div/div[2]/div/div/button"))
                .Click();
        }

        //private void Authenticate()
        //{
        //    driver.Navigate().GoToUrl(authenticateURL);
        //    driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[1]/div/form/span/section/div/div/div[1]/div/div[1]/div/div[1]/input"))
        //        .SendKeys(login);
        //    driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[2]/div/div[1]/div/div/button"))
        //        .Click();
        //    driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[1]/div/form/span/section[2]/div/div/div[1]/div[1]/div/div/div/div/div[1]/div/div[1]/input"))
        //        .SendKeys(password);
        //    ClickButtonAfterDOMUpdate(By.XPath("/html/body/div[1]/div[1]/div[2]/div/c-wiz/div/div[2]/div/div[2]/div/div[1]/div/div/button"));
        //}

        //private void ClickButtonAfterDOMUpdate(By locator)
        //{
        //    try
        //    {
        //        driver.FindElement(locator).Click();
        //    }
        //    catch (StaleElementReferenceException e)
        //    {
        //        new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(ExpectedConditions.ElementIsVisible(locator));
        //        driver.FindElement(locator).Click();
        //    }
        //}

        private List<string> ParseTasksList()
        {
            var tasksList = driver.FindElement(By.ClassName("e2urcc")).FindElements(By.TagName("li"));
            return tasksList
                .Where(li => li.FindElement(By.CssSelector(".asQXV.oDLUVd.YVvGBb")).Text.ContainsSubstring(taskKeyword))
                .Select(li => li.FindElement(By.ClassName("nUg0Te")).GetAttribute("href"))
                .ToList();
        }
    }

    public static class StringExtensions
    {
        public static bool ContainsSubstring(this string str, string substring)
        {
            if (substring.Length > str.Length)
                return false;
            int hashSub = substring.Sum(ch => ch.GetHashCode());
            int curStrHash = 0;
            int curStrHashLength = 0;
            for (int i = 0; i < str.Length; i++)
            {
                curStrHash += str[i].GetHashCode();
                curStrHashLength++;
                if(curStrHashLength > substring.Length)
                {
                    curStrHashLength--;
                    curStrHash -= str[i - curStrHashLength].GetHashCode();
                }
                if(curStrHash == hashSub && str.Substring(i - curStrHashLength + 1, curStrHashLength).Equals(substring))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
