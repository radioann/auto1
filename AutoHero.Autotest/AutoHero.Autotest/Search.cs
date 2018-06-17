using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Globalization;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutoHero.Autotest
{
    [TestClass]
    public class Search
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver(@"..\..\");// or better write path to chromedriver.exe to PATH variable.
            INavigation navigation = driver.Navigate();
            navigation.GoToUrl("https://www.autohero.com/de/search/");

            //Filtering by first registration 2015+ 
            IWebElement filterYear = driver.FindElement(By.XPath("//*[@data-qa-selector='filter-year']"));
            filterYear.Click();
            IWebElement yearRangeMin = driver.FindElement(By.Name("yearRange.min"));
            yearRangeMin.SendKeys("2015" + Keys.Enter);

            //Sorting by price descending
            IWebElement sort = driver.FindElement(By.Name("sort"));
            sort.SendKeys("Höchster Preis" + Keys.Enter);

            //Wait for content loading
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.TextToBePresentInElementValue(driver.FindElement(By.XPath("//div[@data-qa-selector='results-amount']")), "Lädt"));
            navigation.Refresh();

            //Verifying descending price sort order
            ReadOnlyCollection<IWebElement> price = driver.FindElements(By.XPath("//div[@data-qa-selector='price']"));
            decimal previous = decimal.Parse(price[0].Text.Split(' ')[0], CultureInfo.InvariantCulture);
            decimal current = 0m;
            for (int i = 1; i < price.Count; i++)
            {
                current = decimal.Parse(price[i].Text.Split(' ')[0], CultureInfo.InvariantCulture);
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }
    }
}
