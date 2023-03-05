using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Runtime.CompilerServices;

namespace AutoHandText
{
    internal record class AuthData(string login, string password);

    internal class Program
    {
        public const string BaseFilePath = "C:\\Users\\Matrix\\Desktop\\AutoHandText\\";
        public static HandTextScraper HandTextScraper { get; private set; }
        public static AuthData AuthData { get; private set; }

        public static async Task Main()
        {
            AuthData = GetAuthData();

            HandTextScraper = new HandTextScraper();
            Task.Run(() =>
            {
                HandTextScraper.Start(CreateDriver(new Proxy()
                {
                    Kind = ProxyKind.Manual,
                    IsAutoDetect = false,
                    SocksProxy = "<34.118.76.87:1080>",
                    SocksVersion = 5,
                }));
            });

            var driver = CreateDriver();
            var classroomScraper = new ClassroomScraper(driver);
            var urls = classroomScraper.GetURLs();

            foreach (var url in urls)
            {
                await new TaskWorker(url, driver).Work();
            }
        }

        public static IWebDriver CreateDriver(Proxy proxy = null)
        {
            var driverOptions = new ChromeOptions();
            driverOptions.AddArguments("--ignore-ssl-errors=yes", "--ignore-certificate-errors");
            driverOptions.AddUserProfilePreference("profile.default_content_setting_values.automatic_downloads", 1);
            driverOptions.AddUserProfilePreference("download.default_directory", BaseFilePath);
            if (proxy != null)
                driverOptions.Proxy = proxy;
            var driver = new ChromeDriver(driverOptions);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            return driver;
        }

        private static AuthData GetAuthData()
        {
            //Console.WriteLine("Login: ");
            //var login = Console.ReadLine();
            //Console.WriteLine("Password: ");
            //var password = Console.ReadLine();

            var lines = File.ReadAllLines("authdata.txt");
            var login = lines[0];
            var password = lines[1];
            return new AuthData(login, password);
        }
    }
}