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
        // TODO Virker på local, ikke på azure da DB værdierne er ændret.
        private const string URL = "https://ifridgeapp.azurewebsites.net/";
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
            //Henter alle vores objekter i vores table.
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            Thread.Sleep(2000);

            //Tæller hvor mange objekter der er i vores table
            IList<IWebElement> objektListStart = driver.FindElements(By.Id("TableRows"));
            var startresult = objektListStart.Count;

            //Sætter vores delete knap op derefter klikker på den første delete knap vores table
            var deleteRowButton = driver.FindElements(By.Id("deleteButton"));
            deleteRowButton[0].Click();
            Thread.Sleep(1000);

            //Opdater vores table
            getAllButtonElement.Click();
            IList<IWebElement> objektListEnd = driver.FindElements(By.Id("TableRows"));
            var Endresult = objektListEnd.Count;

            //Nu ser vi om der er færrer objekter i vores table end før
            Assert.IsTrue(startresult == Endresult + 1);

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

        public void DropDownSubCategoryTest()
        {
            //Her får vi fat på Dropdown elementet (gennem id fra HTML side)
            IWebElement selectSubCategoryDropdown = driver.FindElement(By.Id("subCategoryDropdown"));

            //her får vi fat i vores dropdown muligheder
            SelectElement selectSubCategory = new SelectElement(selectSubCategoryDropdown);

            //her får jeg fat i alle muligheder og smider dem i en liste
            IList<IWebElement> elements = selectSubCategory.Options;

            //her tæller jeg hvor mange muligheder subCategory der i i listen
            int elementsSize = elements.Count;

            //her får jeg fat i vores table med alle subcategory elementer fra vores DB
            var objektListEnd = driver.FindElements(By.Id("SubCategoryList"));
            //Her tæller jeg listen
            var subCategoryCount = objektListEnd.Count;

            //selve testen der sammenligner de 2 antal fra table og fra category options
            Assert.AreEqual(elementsSize, subCategoryCount);


            //Her får jeg fat på et det første element inden i cubCategoryListen
            var cell = driver.FindElements(By.Id("subCategoryColumn")).First();
            //her tjekker jeg at valuen af første element er hvad jeg forventer
            Assert.AreEqual("Mælk", cell.Text);

        }

        [TestMethod]
        public void ExpireNotificationTest()
        {
            //Henter Listen ved at trykke på "Get List"-knappen.
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            //Vente tid for at sikrer at den har hentet data da vi skal lave test på indholdet af listen.
            Thread.Sleep(3000);

            //Vi tager objekter fra listen og propper i en liste.
            var objektListExpDate = driver.FindElements(By.ClassName("expirationList"));
            var objektListWarning = driver.FindElements(By.ClassName("expirationStatus"));

            //Vi tæller antallet af elementer i hver liste som opfylder krav
            int warningsCount = 0;
            int expDateValue = 0;

            //Her finder vi alle de elementer hvor advarsels billedet er vist istedet for teksten som burde ske hvor der er alarm for at
            //der er mindre end 3 dage til udløbsdato.
            foreach (var Element in objektListWarning)
            {
                try
                {
                    
                    if (Element.Text != "Varen er stadig god")
                    {
                        warningsCount++;
                    }
                }
                catch (System.FormatException e)
                {
                    Console.WriteLine(e);
                    warningsCount++;
                }
            }

            //Her finder vi alle de elementer hvor udløbsdato er lig med 3 eller mindre.
            foreach (var Element in objektListExpDate)
            {
                try
                {
                    if (Element.Text =="3" || Element.Text == "2" || Element.Text == "1" || Element.Text == "0")
                    {
                        expDateValue++;
                    }
                }
                catch (System.FormatException e)
                {
                    Console.WriteLine(e);
                    expDateValue++;
                }
            }
            //Endeligt ser vi på om der er ligemange advarsels billeder vist som der er værdier under grænseværdien på 3(da denne test blev lavet).
            Assert.AreEqual(warningsCount, expDateValue);
        }

        [TestCleanup]
        public void TestOfCleanUp()
        {
            driver.Close();
        }
    }
}
