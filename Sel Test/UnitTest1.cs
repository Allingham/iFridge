using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sel_Test
{
    [TestClass]
    public class UnitTest1
    {
        // TODO Virker på local, ikke på azure da DB værdierne er ændret.
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


            //Indtaster alle værdierne og trykker på opret
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

            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();
            var objektListStart = driver.FindElements(By.Id("ProductList"));
            var startresult = objektListStart.Count;



            IWebElement deteteButton = driver.FindElement(By.Id("deleteButton"));
            deteteButton.Click();





            var objektListEnd = driver.FindElements(By.Id("ProductList"));
            var Endresult = objektListEnd.Count;
            Assert.IsTrue(startresult > Endresult);

            Thread.Sleep(5000);

        }


        [TestMethod]
        public void TestOfSort()
        {
            Thread.Sleep(3000);
            //Sætter get all inventory button op og klikker på den
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            //sætter alle sort buttons op
            IWebElement barcodeButtonElement = driver.FindElement(By.Id("barcodeButton"));
            IWebElement wareNameButtonElement = driver.FindElement(By.Id("wareNameButton"));
            IWebElement expirationDatButtonElement = driver.FindElement(By.Id("expirationDateButton"));
            IWebElement categoryButtonElement = driver.FindElement(By.Id("categoryButton"));
            IWebElement weightButtonElement = driver.FindElement(By.Id("weightButton"));



            Thread.Sleep(3000);
            IWebElement cell = driver.FindElement(By.ClassName("barcodeNumber"));


            //Klikker på hver af knapperne
            Thread.Sleep(2000);
            //Tester om den sorter efter barcode
            barcodeButtonElement.Click();
            Assert.AreEqual("1", cell.Text);
            Thread.Sleep(2000);
            //Tester om den reverse sorter efter barcode
            barcodeButtonElement.Click();
            Assert.AreEqual("12345678", cell.Text);
            Thread.Sleep(1000);

        }



        [TestCleanup]
        public void TestOfCleanUp()
        {
            driver.Close();
        }
    }
}
