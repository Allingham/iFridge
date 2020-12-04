using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PiConsumer;

namespace Sel_Test
{
    [TestClass]
    public class UnitTest1
    {
        // TODO Virker p� local, ikke p� azure da DB v�rdierne er �ndret.
        //private const string URL = "https://ifridgeapp.azurewebsites.net/";
        private const string URL = "http://localhost:3000/";
        ChromeOptions options = new ChromeOptions();
        IWebDriver driver = new ChromeDriver();


        [TestInitialize]
        public void Setup()
        {
            driver.Navigate().GoToUrl(URL);
        }

        [TestMethod]
        public void TestAdd()
        {
            //Henter Listen og checker indholdet i hvormange elementer der er i
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();
            var objektListStart = driver.FindElements(By.Id("ProductList"));
            var startresult = objektListStart.Count;


            //Indtaster alle v�rdierne og trykker p� opret
            IWebElement inputBarcode = driver.FindElement(By.Id("barcodeInput"));
            inputBarcode.Clear();
            inputBarcode.SendKeys("1000008");

            IWebElement InputName = driver.FindElement(By.Id("nameInput"));
            InputName.Clear();
            InputName.SendKeys("Vegansk Pizza");

            IWebElement InputCategori = driver.FindElement(By.Id("categoriInput"));
            InputCategori.Clear();
            InputCategori.SendKeys("Skrald");

            IWebElement InputexpirationDate = driver.FindElement(By.Id("expirationDateInput"));
            InputexpirationDate.Clear();
            InputexpirationDate.SendKeys("2");

            IWebElement Inputweight = driver.FindElement(By.Id("weightInput"));
            Inputweight.Clear();
            Inputweight.SendKeys("100");


            IWebElement Inputpicture = driver.FindElement(By.Id("pictureInput"));
            Inputpicture.Clear();
            Inputpicture.SendKeys("");

            Thread.Sleep(5000);

            IWebElement AddButton = driver.FindElement(By.Id("add"));
            AddButton.Click();

            Thread.Sleep(5000);


            //Checker hvormange elementer der i listen efter man har oprettet
            var objektListEnd = driver.FindElements(By.Id("ProductList"));
            var Endresult = objektListEnd.Count;
            Assert.IsTrue(startresult < Endresult);


        }

        [TestMethod]
        public void TestDelete()
        {
            ProductPoster.PostProductInstance(1);


            //Henter alle vores objekter i vores table.
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            Thread.Sleep(2000);


            //Sort listen s� den kan slette f�rste element i listen
            IWebElement sort = driver.FindElement(By.Id("barcodeButton"));
            sort.Click();

            //T�ller hvor mange objekter der er i vores table
            IList<IWebElement> objektListStart = driver.FindElements(By.Id("TableRows"));
            var startresult = objektListStart.Count;

            //S�tter vores delete knap op derefter klikker p� den f�rste delete knap vores table
            var deleteRowButton = driver.FindElements(By.Id("deleteButton"));
            deleteRowButton[0].Click();
            Thread.Sleep(1000);

            //Opdater vores table
            getAllButtonElement.Click();
            IList<IWebElement> objektListEnd = driver.FindElements(By.Id("TableRows"));
            var Endresult = objektListEnd.Count;

            //Nu ser vi om der er f�rrer objekter i vores table end f�r
            Assert.IsTrue(startresult == Endresult + 1);

            Thread.Sleep(5000);

        }


        [TestMethod]
        public void TestOfSort()
        {


            Thread.Sleep(3000);
            //S�tter get all inventory button op og klikker p� den
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            //s�tter alle sort buttons op
            IWebElement barcodeButtonElement = driver.FindElement(By.Id("barcodeButton"));

            Thread.Sleep(3000);
            IWebElement cell = driver.FindElement(By.ClassName("barcodeList"));


            //Klikker p� hver af knapperne
            Thread.Sleep(2000);
            //Tester om den sorter efter barcode
            barcodeButtonElement.Click();
            Assert.AreEqual("1", cell.Text);
            Thread.Sleep(2000);

            //Tester om den reverse sorter efter barcode
            barcodeButtonElement.Click();
            Assert.AreEqual("99999999", cell.Text);
            Thread.Sleep(1000);



        }
        [TestMethod]
        public void DropDownSubCategoryTest()
        {

            var Category = driver.FindElement(By.TagName("select"));
            SelectElement selectElement = new SelectElement(Category);

            Thread.Sleep(2000);

            selectElement.SelectByIndex(1);

            Thread.Sleep(2000);

            var optionsElement = driver.FindElements(By.TagName("option"));
            var result = optionsElement[1];

            Thread.Sleep(2000);

            Assert.IsTrue(result.Selected);


        }


        [TestCleanup]
        public void TestOfCleanUp()
        {
            driver.Close();
        }
    }
}
