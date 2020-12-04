using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Sel_Test
{
    [TestClass]
    public class UnitTest1
    {
        // TODO Virker p� local, ikke p� azure da DB v�rdierne er �ndret.
        private const string URL = "https://ifridgeapp.azurewebsites.net/";
        //private const string URL = "http://localhost:3000/";
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

            //IWebElement InputCategori = driver.FindElement(By.Id("categoriInput"));
            //InputCategori.Clear();
            //InputCategori.SendKeys("Skrald");

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
            //Henter alle vores objekter i vores table.
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            Thread.Sleep(2000);

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
            IWebElement cell = driver.FindElement(By.ClassName("barcodeNumber"));


            //Klikker p� hver af knapperne
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

        [TestMethod]
        public void DropDownSubCategoryTest()
        {
            //Her f�r vi fat p� Dropdown elementet (gennem id fra HTML side)
            IWebElement selectSubCategoryDropdown = driver.FindElement(By.Id("subCategoryDropdown"));

            //her f�r vi fat i vores dropdown muligheder
            SelectElement selectSubCategory = new SelectElement(selectSubCategoryDropdown);

            //her f�r jeg fat i alle muligheder og smider dem i en liste
            IList<IWebElement> elements = selectSubCategory.Options;

            //her t�ller jeg hvor mange muligheder subCategory der i i listen
            int elementsSize = elements.Count;

            //her f�r jeg fat i vores table med alle subcategory elementer fra vores DB
            var objektListEnd = driver.FindElements(By.Id("SubCategoryList"));
            //Her t�ller jeg listen
            var subCategoryCount = objektListEnd.Count;

            //selve testen der sammenligner de 2 antal fra table og fra category options
            Assert.AreEqual(elementsSize, subCategoryCount);


            //Her f�r jeg fat p� et det f�rste element inden i cubCategoryListen
            var cell = driver.FindElements(By.Id("subCategoryColumn")).First();
            //her tjekker jeg at valuen af f�rste element er hvad jeg forventer
            Assert.AreEqual("M�lk", cell.Text);

        }


        [TestCleanup]
        public void TestOfCleanUp()
        {
            driver.Close();
        }
    }
}
