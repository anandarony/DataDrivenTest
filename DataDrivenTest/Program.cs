using CsvHelper;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Globalization;
using DataDrivenTest;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;


class Program
{
    public static string DataCSVFile = System.IO.Directory.GetCurrentDirectory();
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();


        var testDataList = ReadCsvData(DataCSVFile + "\\Data\\Data.csv");
        ExtentReports extentReports = new ExtentReports();
        ExtentSparkReporter reportpath = new ExtentSparkReporter(@"C:\SQA Report Location\report" + DateTime.Now.ToString("_MMddyyyy_hhmmtt") + ".html");

        extentReports.AttachReporter(reportpath);
        ExtentTest testl = extentReports.CreateTest("Login Test", "This is our First Test Case");


        driver.Navigate().GoToUrl("https://practicetestautomation.com/practice-test-login/");
        testl.Log(Status.Info, "Open browser");
        Console.WriteLine("Open Browser");

        driver.Manage().Window.Maximize();
        Console.WriteLine("Browser Maximize");
        testl.Log(Status.Info, "Browser Maximize");


        foreach (var test in testDataList)
        {
            driver.FindElement(By.Id("username")).SendKeys(test.username);
            Console.WriteLine("Provide username: " +test.username);
            testl.Log(Status.Info, "Provide username: " +test.username);

            driver.FindElement(By.Id("password")).SendKeys(test.password);
            Console.WriteLine("Provide Password: "+test.password);
            testl.Log(Status.Info, "Provide Password: "+test.password);

            driver.FindElement(By.Id("submit")).Click();

            Console.WriteLine("Hit Submit button");
            testl.Log(Status.Info, "Hit Submuit button");
            try
            {
                driver.FindElement(By.CssSelector(".wp-block-button__link")).Click();
                Console.WriteLine("Login Successfully");
                testl.Log(Status.Pass, "Login Successfully");
                break;
            }
            catch
            {
                Console.WriteLine("Failed Login");
                testl.Log(Status.Fail, "login failed");
            }


        }


        driver.Quit();
        extentReports.Flush();
    }

    static List<TestData> ReadCsvData(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return new List<TestData>(csv.GetRecords<TestData>());
        }
    }

    private static void CreateReportDirectories()
    {
        string Reportpath = @"C:\SQA Report Location\";
        if (Directory.Exists(Reportpath))
        {
            Directory.CreateDirectory(Reportpath);


        }
    }
}




