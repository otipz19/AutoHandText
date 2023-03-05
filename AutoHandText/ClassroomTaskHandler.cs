using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace AutoHandText
{
    internal class ClassroomTask
    {
        public readonly string Name;
        public readonly string Content;

        public ClassroomTask(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }

    internal class ClassroomTaskHandler
    {
        private string url;
        private IWebDriver driver;
        private string directoryPath;

        public ClassroomTaskHandler(string url, IWebDriver driver, string directoryPath)
        {
            this.url = url;
            this.driver = driver;
            this.directoryPath = directoryPath;
        }

        public ClassroomTask GetClassroomTask()
        {
            driver.Navigate().GoToUrl(url);
            string name = driver.FindElement(By.CssSelector("div.N5dSp"))
                .FindElement(By.TagName("span"))
                .Text;
            string urlToDoc = driver.FindElement(By.CssSelector("div.sVNOQ"))
                .FindElement(By.CssSelector("a.vwNuXe"))
                .GetAttribute("href");
            driver.Navigate().GoToUrl(urlToDoc);    
            driver.FindElement(By.XPath("/html/body/div[4]/div[4]/div/div[3]/div[2]/div[2]/div[3]"))
                .Click();
            return new ClassroomTask(name, GetContentFromDoc());
        }

        private string GetContentFromDoc()
        {
            var dir = new DirectoryInfo(Program.BaseFilePath);
            //Waits for file download
            //while(dir.EnumerateFiles().Count() == 0)
            //{
            //    Thread.Sleep(100);
            //}
            Thread.Sleep(10000);
            var filePath = dir.EnumerateFiles()
                .Where(f => f.Extension == ".docx")
                .Select(f => f.FullName)
                .First();
            var doc = DocX.Load(filePath);
            var returnValue = string.Join(' ', doc.Paragraphs.Select(p => p.Text.Replace('\\', ' ')));
            File.Delete(filePath);
            return returnValue;
        }
    }
}
