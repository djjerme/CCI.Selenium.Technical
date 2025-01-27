using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CCI.Selenium.Technical.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class LoginTest
    {
        private ChromeDriver? driver;
        private readonly By _usernameInputXpath = By.XPath("//input[contains(@id, 'user-name')]");
        private readonly By _passwordInputXpath = By.XPath("//input[contains(@id, 'password')]");
        private readonly By _loginButtonXpath = By.XPath("//input[contains(@class, 'submit-button')]");
        private readonly By _loginErrorMessagesXpath = By.XPath("//h3[contains(@data-test, 'error')]");

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.saucedemo.com/");
        }

        [Test]
        public void GivenLockedUser_ThanLockedOutErrorMessage()
        {
            string username = "locked_out_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();

            IWebElement loginErrorMessages = driver.FindElement(_loginErrorMessagesXpath);
            string errorMessage = loginErrorMessages.Text;

            Assert.Multiple(() =>
            {
                Assert.That(errorMessage, Does.Contain("Epic sadface: Sorry, this user has been locked out."));
                Assert.That(loginErrorMessages.Displayed, Is.True);
            });
        }

        [Test]
        public void GivenInvalidPassword_ThanUsernameAndPasswordDoNotMatchErrorMessage()
        {
            string username = "standard_user";
            string password = "invalid_password";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();

            IWebElement loginErrorMessages = driver.FindElement(_loginErrorMessagesXpath);
            string errorMessage = loginErrorMessages.Text;

            Assert.Multiple(() =>
            {
                Assert.That(errorMessage, Does.Contain("Epic sadface: Username and password do not match any user in this service"));
                Assert.That(loginErrorMessages.Displayed, Is.True);
            });
        }

        [Test]
        public void GivenEmptyUsernameAndPassword_ThanUsernameIsRequiredErrorMessage()
        {
            string username = "";
            string password = "";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();

            IWebElement loginErrorMessages = driver.FindElement(_loginErrorMessagesXpath);
            string errorMessage = loginErrorMessages.Text;

            Assert.Multiple(() =>
            {
                Assert.That(errorMessage, Does.Contain("Epic sadface: Username is required"));
                Assert.That(loginErrorMessages.Displayed, Is.True);
            });
        }

        [Test]
        public void GivenValidUsernameAndPassword_ThanNavigateToProductsPage()
        {
            string username = "standard_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_loginButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);

            string productsPageUrl = "https://www.saucedemo.com/inventory.html";
            string currentUrl = driver.Url;

            Assert.That(currentUrl, Is.EqualTo(productsPageUrl));
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
