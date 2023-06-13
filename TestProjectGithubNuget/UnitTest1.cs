using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenQA.Selenium.Support.UI;

namespace TakeAlotAppleTV
{
    [TestFixture]
    public class TestProjectGithubNuget
    {
        private static IWebDriver? driver;
        private string searchTerm = "Apple TV";



        private static WebDriverWait wait;




        [OneTimeSetUp]
        public void SetUpTest()
        {
            EdgeOptions options = new EdgeOptions();
            options.BrowserVersion = "114.0.1823.43";
            
            driver = new EdgeDriver (options);

            new DriverManager().SetUpDriver(new EdgeConfig());



            driver = new EdgeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }



        [Test]
        public void SearchForAppleTVResultsTest()
        {
            driver.Navigate().GoToUrl("https://www.takealot.com");
            driver.Manage().Window.Maximize();



            IWebElement searchInput = driver.FindElement(By.CssSelector("[name='search']"));
            searchInput.SendKeys(searchTerm);
            searchInput.SendKeys(Keys.Enter);



            try
            {
                wait.Until(d => driver.FindElement(By.CssSelector(".product-anchor")).Displayed);
            }
            catch (TimeoutException)
            {
                TestContext.WriteLine("elements took too long to load");
            }

            IList<string> productsAvailable = driver.FindElements(By.CssSelector(".product-anchor"))
            .Select(item => item.GetAttribute("title").ToLower())
            .ToList();



            IList<string> results = productsAvailable.Where(item => item.Contains(searchTerm.ToLower()))
            .ToList();
            TestContext.WriteLine("Available results: ");
            foreach (string product in productsAvailable)
            {
                TestContext.WriteLine(product);
            }
            TestContext.WriteLine($"Results containing {searchTerm}: ");
            foreach (string product in results)
            {
                TestContext.WriteLine(product);
            }



            Assert.AreEqual(results, productsAvailable);



        }



        [OneTimeTearDown]
        public void ReleaseDriver()
        {
            driver.Quit();
        }
    }
}