using NUnit.Framework;
using NUnit.Framework.Constraints;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CCI.Selenium.Technical.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class LoginTest
    {
        private ChromeDriver? driver;
        private readonly By _usernameInputXpath = By.XPath("//input[contains(@id, 'user-name')]");
        private readonly By _passwordInputXpath = By.XPath("//input[contains(@id, 'password')]");
        private readonly By _submitButtonXpath = By.XPath("//input[contains(@class, 'submit-button')]");
        private readonly By _loginErrorMessagesXpath = By.XPath("//h3[contains(@data-test, 'error')]");
        private readonly By _shoppingCartQtyXpath = By.XPath("//*[@id=\"shopping_cart_container\"]/a/span");
        private readonly By _checkoutButtonXpath = By.XPath("//*[@id=\"checkout\"]");
        private readonly By _firstNameInputXpath = By.XPath("//input[contains(@id, 'first-name')]");
        private readonly By _lastNameInputXpath = By.XPath("//input[contains(@id, 'last-name')]");
        private readonly By _postalCodeInputXpath = By.XPath("//input[contains(@id, 'postal-code')]");
        private readonly By _finishButtonXpath = By.XPath("//*[@id=\"finish\"]");
        private readonly By _addToCartXpath = By.XPath("//button[contains(@id,'add-to-cart-sauce-labs-backpack')]");
        private readonly By _removeFromCartXpath = By.XPath("//button[contains(@id,'remove')]");
        private readonly By _productSortContainerXpath = By.XPath("//*[@id=\"header_container\"]/div[2]/div/span/select"); //*[@id="header_container"]/div[2]/div/span/select

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
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

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
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

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
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

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
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);

            string productsPageUrl = "https://www.saucedemo.com/inventory.html";
            string currentUrl = driver.Url;

            Assert.That(currentUrl, Is.EqualTo(productsPageUrl));
        }

        [Test]
        public void GivenValidUserNameAndPassword_AddThenCheckout()
        {
            string username = "standard_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);

            IWebElement addToCart = driver.FindElement(_addToCartXpath);
            addToCart.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);

            IWebElement viewCart = driver.FindElement(_shoppingCartQtyXpath);
            viewCart.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);
            if (driver.Url != "https://www.saucedemo.com/cart.html") throw new Exception("This is not the cart page");

            IWebElement checkoutButton = driver.FindElement(_checkoutButtonXpath);
            checkoutButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);
            if (driver.Url != "https://www.saucedemo.com/checkout-step-one.html") throw new Exception("This is not the checkout step one page");

            string firstName = "Derpy";
            string lastName = "Hooves";
            string postalCode = "97201";
            IWebElement firstNameInput = driver.FindElement(_firstNameInputXpath);
            IWebElement lastNameInput = driver.FindElement(_lastNameInputXpath);
            IWebElement postalCodeInput = driver.FindElement(_postalCodeInputXpath);

            firstNameInput.SendKeys(firstName);
            lastNameInput.SendKeys(lastName);
            postalCodeInput.SendKeys(postalCode);

            IWebElement continueButton = driver.FindElement(_submitButtonXpath);
            continueButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);
            if (driver.Url != "https://www.saucedemo.com/checkout-step-two.html") throw new Exception("This is not the checkout step two page");

            IWebElement finishButton = driver.FindElement(_finishButtonXpath);
            finishButton.Click();

            string checkoutCompleteUrl = "https://www.saucedemo.com/checkout-complete.html";
            string currentUrl = driver.Url;

            Assert.That(currentUrl, Is.EqualTo(checkoutCompleteUrl));
        }

        [Test]
        public void GivenValidUserNameAndPassword_ThenAddAndRemoveFromCart()
        {
            int cartQty = 0;
            int postCartQty = 0;
            string username = "standard_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);

            IWebElement addToCart = driver.FindElement(_addToCartXpath);
            addToCart.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);

            IWebElement shoppingCartQty = driver.FindElement(_shoppingCartQtyXpath);
            cartQty = int.Parse(shoppingCartQty.Text);

            IWebElement removeFromCart = driver.FindElement(_removeFromCartXpath);
            removeFromCart.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);

            try
            {
                IWebElement postShoppingCartQty = driver.FindElement(_shoppingCartQtyXpath);
                postCartQty = int.Parse(postShoppingCartQty.Text);
            }
            catch (NoSuchElementException) { postCartQty = 0; }

            Assert.That((cartQty - postCartQty) == 1);
        }

        [Test]
        public void GivenValidUserNameAndPassword_ChangeFilterOnProductPage()
        {
            string username = "standard_user";
            string password = "secret_sauce";
            IWebElement usernameInput = driver.FindElement(_usernameInputXpath);
            IWebElement passwordInput = driver.FindElement(_passwordInputXpath);
            IWebElement loginButton = driver.FindElement(_submitButtonXpath);

            usernameInput.SendKeys(username);
            passwordInput.SendKeys(password);
            loginButton.Click();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);

            SelectElement productSort = new SelectElement(driver.FindElement(_productSortContainerXpath));
            productSort.SelectByValue("lohi");
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(5);
            productSort = new SelectElement(driver.FindElement(_productSortContainerXpath));
            productSort.SelectByValue("hilo");




        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
