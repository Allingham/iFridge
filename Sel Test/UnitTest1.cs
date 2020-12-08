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
            //Henter Listen og checker indholdet i hvor mange elementer der er i
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllProductsButton"));
            getAllButtonElement.Click();
            Thread.Sleep(2000);
            var objektListStart = driver.FindElements(By.Id("debugList"));
            Thread.Sleep(1000);
            var startresult = objektListStart.Count;


            //Indtaster alle værdierne og trykker på opret
            IWebElement inputBarcode = driver.FindElement(By.Id("barcodeInput"));
            inputBarcode.Clear();
            inputBarcode.SendKeys("1008");

            IWebElement InputName = driver.FindElement(By.Id("nameInput"));
            InputName.Clear();
            InputName.SendKeys("Pizza");

            var selectElement = driver.FindElement(By.TagName("select"));
            var selectObject = new SelectElement(selectElement);
            selectObject.SelectByIndex(2);

            IWebElement InputexpirationDate = driver.FindElement(By.Id("expirationDateInput"));
            InputexpirationDate.Clear();
            InputexpirationDate.SendKeys("2");

            IWebElement Inputweight = driver.FindElement(By.Id("weightInput"));
            Inputweight.Clear();
            Inputweight.SendKeys("100");


            IWebElement Inputpicture = driver.FindElement(By.Id("pictureInput"));
            Inputpicture.Clear();
            Inputpicture.SendKeys("");

            Thread.Sleep(2000);

            IWebElement AddButton = driver.FindElement(By.Id("add"));
            AddButton.Click();


            Thread.Sleep(5000);


            getAllButtonElement.Click();
            Thread.Sleep(2000);

            //Checker hvor mange elementer der er i listen efter man har oprettet
            var objektListEnd = driver.FindElements(By.Id("debugList"));
            Thread.Sleep(2000);
            var Endresult = objektListEnd.Count;
            Thread.Sleep(2000);

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
            //Finder vores select element
            var Category = driver.FindElement(By.TagName("select"));
            SelectElement selectElement = new SelectElement(Category);

            //Giver programmet tid til at finde select elementet
            Thread.Sleep(2000);

            //V�lger det element p� index 1es plads
            selectElement.SelectByIndex(1);

            Thread.Sleep(2000);

            //V�lger element p� index 1es plads
            var optionsElement = driver.FindElements(By.TagName("option"));
            var result = optionsElement[1];

            Thread.Sleep(2000);

            //Ser p� om det element p� index 1es plads er valgt
            Assert.IsTrue(result.Selected);


        }

        [TestMethod]
        public void ExpireNotificationTest()
        {
            //Henter Listen ved at trykke p� "Get List"-knappen.
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            //Vente tid for at sikrer at den har hentet data da vi skal lave test p� indholdet af listen.
            Thread.Sleep(3000);

            //Vi tager objekter fra listen og propper i en liste.
            var objektListExpDate = driver.FindElements(By.ClassName("expirationList"));
            var objektListWarning = driver.FindElements(By.ClassName("expirationStatus"));

            //Vi t�ller antallet af elementer i hver liste som opfylder krav
            int warningsCount = 0;
            int expDateValue = 0;

            //Her finder vi alle de elementer hvor advarsels billedet er vist istedet for teksten som burde ske hvor der er alarm for at
            //der er mindre end 3 dage til udl�bsdato.
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

            //Her finder vi alle de elementer hvor udl�bsdato er lig med 3 eller mindre.
            foreach (var Element in objektListExpDate)
            {
                try
                {
                    if (Element.Text == "3" || Element.Text == "2" || Element.Text == "1" || Element.Text == "0")
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
            //Endeligt ser vi p� om der er ligemange advarsels billeder vist som der er v�rdier under gr�nsev�rdien p� 3(da denne test blev lavet).
            Assert.AreEqual(warningsCount, expDateValue);
        }


        [TestMethod]
        public void getRecipe()
        {
            // text felt hvor man søger opskrift 
            //få en list af indgridienser der skal bruges til opskrift
            //indgridienser jeg mangler skal være røde eller have en notifikation ud for

            IWebElement recipeinput = driver.FindElement(By.Id("recipeInput"));
            recipeinput.Clear();
            recipeinput.SendKeys("pasta");
            IWebElement getRecipesButton = driver.FindElement(By.Id("getRecipes"));

            getRecipesButton.Click();
            Thread.Sleep(2000);
            IWebElement cell = driver.FindElement(By.ClassName("recipeList"));
            Assert.AreEqual("Pasta With Tuna", cell.Text);


            //tjekke om vi har indgridenten i vores køleskab

            cell.Click();

            Thread.Sleep(2000);
            IList<IWebElement> IngredientList = driver.FindElements(By.Id("IngredientList"));
            var textColor = IngredientList[0].GetAttribute("style");
            Assert.AreEqual("color: rgb(255, 0, 0);", textColor);

        }

        [TestMethod]
        public void TestOfCreateShoppingList()
        {
            var objektListStart = driver.FindElements(By.Id("TableRows2"));
            var startresult = objektListStart.Count;


            IWebElement InsertProductName = driver.FindElement(By.Id("itemName"));
            InsertProductName.SendKeys("Risengr�d p� Tube");

            IWebElement InsertShop = driver.FindElement(By.Id("itemPlace"));
            InsertShop.SendKeys("Fakta");

            IWebElement InsertProductPrice = driver.FindElement(By.Id("itemPrice"));
            InsertProductPrice.SendKeys("25");

            
            IWebElement CreateShoppingList = driver.FindElement(By.Id("addToShopping"));
            CreateShoppingList.Click();

            Thread.Sleep(2000);

            var objektListEnd = driver.FindElements(By.Id("TableRows2"));
            var Endresult = objektListEnd.Count;
            Assert.IsTrue(startresult > Endresult);

            Thread.Sleep(5000);

        }


        [TestCleanup]
        public void TestOfCleanUp()
        {
            driver.Close();
        }
    }
}
