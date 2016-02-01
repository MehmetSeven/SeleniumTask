using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace WebDriverTask
{
    [TestClass]
    public class N11Tests
    {
        private IWebDriver driver;

        [TestInitialize]
        public void TestStart()
        {
            driver = new FirefoxDriver();
            //Açılan pencerenin ekranı kaplaması için
            driver.Manage().Window.Maximize();
            // N11 e git komutu
            driver.Navigate().GoToUrl("http://www.n11.com");
        }

        [TestCleanup]
        public void TestEnd()
        {
            driver.Quit();
        }

        [TestMethod]
        public void TestN11()
        {
            N11MainPageTest();
            N11Login();
            SearchSamsungAndVerify();
            SelectSecondPage();
            SelectThirdItem();
            //2 tıklama arası bekletmek için
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

            IWebElement addFavoritesToProduct = AddItemToFavoritesAndVerify();
            RemoveFromFavoritesAndVerify(addFavoritesToProduct);


        }

        public void N11MainPageTest()
        {
            //Anasayfa kontrolu 2 farklı yol olacak şekilde ayarlandı
            //string title = driver.Title;
            //Assert.AreEqual("n11.com - Alışverişin Uğurlu Adresi", title);
            var pageName = driver.FindElement(By.Id("ga-pagename"));
            var innerHtml = pageName.GetAttribute("innerHTML");

            //Anasayfa kontrolunun yapıldığı kısım
            Assert.IsTrue(innerHtml.Contains("anasayfa"));
        }
        public void N11Login()
        {
            //Uye Girişi sayfasına yonlendırmenın yapıldığı kısım
            driver.FindElement(By.ClassName("btnSignIn")).Click();

            //Uye girişi için mail adresi ve şifrenin yazdırıldığı ve uye girişinin tamamlandığı kısım
            driver.FindElement(By.Id("email")).SendKeys("mehmetswn@hotmail.com");
            driver.FindElement(By.Id("password")).SendKeys("test1user");
            driver.FindElement(By.Id("loginButton")).Click();
        }
        public void SearchSamsungAndVerify()
        {

            N11SearchText("samsung");
            //Samsung için sonuc bulunduğunun kontrol edildiği kısım
            var searchResult = driver.FindElement(By.Id("itemCount"));
            var searchResultValue = searchResult.GetAttribute("Value");
            Assert.AreNotEqual("0", searchResultValue);
        }

        public void SelectSecondPage()
        {
            N11PageSelection(2);
            Assert.IsTrue(driver.Url.Contains("pg=2"));
        }

        public void SelectThirdItem()
        {
            SelectSearchReasultItem(3);
        }
        public IWebElement AddItemToFavoritesAndVerify()
        {
            //Favorilere eklenmesi
            IWebElement addFavElement = driver.FindElement(By.Id("addToFavourites"));
            addFavElement.Click();
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

            //Favori urun sayfasinda urunu kontrol etmek için aşağıdaki değişkenlere ihtiyac vardır
            IWebElement productAnchor = driver.FindElement(By.ClassName("proName"));
            var productName = productAnchor.GetAttribute("innerHTML").Replace(@"\r", "").Replace(@"\n", "").Trim();
            //Favorilerin olduğu kısıma giriş
            var goToFavPageAnchor = driver.FindElement(By.XPath("//*[@class='myAccountMenu hOpenMenu']/div/a[2]"));
            var link = goToFavPageAnchor.GetAttribute("href");
            driver.Navigate().GoToUrl(link);
            IWebElement product = SearchInFavorites(productName);
            Assert.IsTrue(product != null);
            return product;
        }

        private void RemoveFromFavoritesAndVerify(IWebElement itemToRemove)
        {
            //Favorilerden silindiği ve kontrol edildiği kısım
            string productName = itemToRemove.GetAttribute("innerHTML");
            N11RemoveFromFavorites(itemToRemove);
            Assert.IsTrue(SearchInFavorites(productName) == null);
        }
        public IWebElement SearchInFavorites(string itemName)
        {
            //favorilerdeki ürünün önceki sayfadaki ürün mü kontrolu
            List<IWebElement> favList = driver.FindElements(By.XPath("//*[@class='productTitle']/p/a")).ToList();
            return favList.Find(x => x.GetAttribute("innerHTML") == itemName);
        }

        public void N11RemoveFromFavorites(IWebElement itemToRemove)
        {
            //Favoriler sayfasından silinme işleminin yapıldığı kısım
            IWebElement productRemoveAnchor = itemToRemove.FindElement(By.XPath("../../../td[1]/a"));
            productRemoveAnchor.Click();
        }
        public void N11SearchText(string searcText)
        {
            //Arama yaptırılan kısım
            IWebElement searchInput = driver.FindElement(By.Id("searchData"));
            searchInput.SendKeys(searcText);
            searchInput.SendKeys(Keys.Enter);
        }
        public void N11PageSelection(int pageNumber)
        {
            //2.sayfaya geçilen ve onaylamanın olduğu kısım
            IWebElement pageNumberResult = driver.FindElement(By.XPath("//*[@class='pagination']/a[" + pageNumber.ToString() + "]"));
            pageNumberResult.Click();
        }

        public void SelectSearchReasultItem(int itemNumber)
        {
            //Listelenen urunlerden 3.urunun içine girilmesi
            IWebElement selectedProduct = driver.FindElement(By.XPath("//*[@id='view']/ul/li[" + itemNumber.ToString() + "]/div/div/a"));
            selectedProduct.Click();
        }
    }
}
