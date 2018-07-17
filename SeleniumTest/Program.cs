using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlindsOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new FirefoxDriver();
            //Launching browser and navigating to blinds.com
            driver.Url = "http:www.blinds.com";

            //resizing window to full screen
            driver.Manage().Window.Maximize();

            //Lets wait for few sec to avoid synch issues
            driver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(3));

            //FInd the search box and type in the search term
            IWebElement searchBox = driver.FindElement(By.Name("q"));
            searchBox.SendKeys("Room darkening blinds");

            //Wait again for few secs and click the search  button
            driver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(3));
            driver.FindElement(By.Id("gcc-inline-search-submit")).Click();
            //searchBox.sendKeys(Keys.ENTER);

            //Wait again for few secs and click the price filter to sort from low to high
            driver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(2));
            driver.FindElement(By.LinkText("Price Low-High")).Click();

            //Wait again
            driver.Manage().Timeouts().ImplicitWait = (TimeSpan.FromSeconds(5));

            //Get the prices in the search results and store them in a List
            IList<IWebElement> prices = driver.FindElements(By.XPath("//div[@id='gcc-search-results-list']/div/article[1]/div/div/div/div/div/div/div/span[1]"));

            //This is how we verify prices are sorted
            //Craeting a list of Integer type
            List<int> displayPrices = new List<int>();

            //Declaring an Iterator 
            IEnumerator<IWebElement> itr = prices.GetEnumerator();

            //Iterating through the list and grabbing the prices as String
            while (itr.MoveNext())
            {
                String p = itr.Current.Text;
                Console.WriteLine(p);
                //Grabbing the substring of the amount to get rid of $ sign
                String amount = p.Substring(1);
                Console.WriteLine(amount);

                //Converting Strings into Double amount
                Double cost = Double.Parse(amount);

                //Casting double to int and adding to the displayPrices list
                int pr = (int)cost;
                displayPrices.Add(pr);
            }

            //Looping through the list to cpmpare int values to verify sort
            for (int i = 0; i < displayPrices.Count() - 1; i++)
            {
                if (displayPrices.IndexOf(i) <= displayPrices.IndexOf(i + 1))
                {
                    Console.WriteLine("Pass - The resluts are sorted from Low to High Price");
                }
                else
                    Console.WriteLine("Fail"); ;

            }

        }
    }
}