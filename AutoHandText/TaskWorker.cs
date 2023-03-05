using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHandText
{
    internal class TaskWorker
    {
        private string taskURL;
        private IWebDriver driver;
        public string DirectoryName { get; private set; }
        public string FullDirectoryPath => Program.BaseFilePath + DirectoryName;

        public TaskWorker(string url, IWebDriver driver)
        {
            taskURL = url;
            this.driver = driver;
        }

        public async Task Work()
        {
            var taskHadler = new ClassroomTaskHandler(taskURL, driver, FullDirectoryPath);
            ClassroomTask task = taskHadler.GetClassroomTask();
            await Program.HandTextScraper.MakeImages(task.Content);
            Directory.CreateDirectory(FullDirectoryPath);
            //CopyFilesToTaskDirectory(task);
            var documentCreator = new DocumentCreator(DirectoryName, task.Name + ".docx");
            documentCreator.CreateDocument();
            foreach(var img in new DirectoryInfo(Program.BaseFilePath).EnumerateFiles()
                .Where(file => file.Extension == ".jpeg"))
            {
                img.Delete();
            }
        }

        //private void CopyFilesToTaskDirectory(ClassroomTask task)
        //{
        //    DirectoryName = task.Name;
        //    Directory.CreateDirectory(FullDirectoryPath);
        //    foreach(var file in new DirectoryInfo(FullDirectoryPath)
        //        .EnumerateFiles()
        //        .Where(f => f.Extension == ".jpeg"))
        //    {
        //        file.CopyTo(FullDirectoryPath + "//" + file.Name);
        //    }
        //}

        //private IWebDriver CreateDriver()
        //{
        //    var driverOptions = new ChromeOptions();
        //    driverOptions.AddArguments("--ignore-ssl-errors=yes", "--ignore-certificate-errors");
        //    driverOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
        //    driverOptions.AddUserProfilePreference("download.default_directory", FullDirectoryPath);
        //    Directory.CreateDirectory(FullDirectoryPath);
        //    var driver = new ChromeDriver(driverOptions);
        //    //driver.Manage().Window.Maximize();
        //    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        //    return driver;
        //}
    }
}
