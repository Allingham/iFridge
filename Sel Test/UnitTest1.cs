using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using FridgeModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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

            IList<Product> listBefore = getList().Result;
         


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


            Thread.Sleep(200);

            IWebElement AddButton = driver.FindElement(By.Id("add"));
            AddButton.Click();


            Thread.Sleep(500);

            IList<Product> listAfter = getList().Result;

            Thread.Sleep(500);

            DeleteProduct(1008);    

            Assert.AreEqual(listBefore.Count +1, listAfter.Count);


        }

        private async Task<IList<Product>> getList()
        {
            using (HttpClient client = new HttpClient())
            {
                string ProductEndPoint = "https://ifridgeapi.azurewebsites.net/api/Products";
                string content = await client.GetStringAsync(ProductEndPoint);
                IList<Product> list = JsonConvert.DeserializeObject<IList<Product>>(content);

                return list;
            }
        }

        private async void DeleteProduct(int id) {
            using (HttpClient client = new HttpClient())
            {
                string ProductEndPoint = "https://ifridgeapi.azurewebsites.net/api/Products";
                await client.DeleteAsync(ProductEndPoint + "/" + id);

            }
        }

        [TestMethod]
        public void TestDelete()
        {
            ProductPoster.PostProductInstance(1);
            Thread.Sleep(1000);

            //Henter alle vores objekter i vores table.
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            Thread.Sleep(200);

            //Sort listen s� den kan slette f�rste element i listen
            IWebElement sort = driver.FindElement(By.Id("dateAdded"));
            sort.Click();
            Thread.Sleep(200);
            sort.Click();

            Thread.Sleep(200);

            //T�ller hvor mange objekter der er i vores table
            IList<IWebElement> objektListStart = driver.FindElements(By.Id("TableRows"));
            var startresult = objektListStart.Count;

            Thread.Sleep(200);

            //S�tter vores delete knap op derefter klikker p� den f�rste delete knap vores table
            var deleteRowButton = driver.FindElement(By.Id("deleteButton"));
            deleteRowButton.Click();
            Thread.Sleep(500);

            //Opdater vores table
            
            IList<IWebElement> objektListEnd = driver.FindElements(By.Id("TableRows"));
            var Endresult = objektListEnd.Count;

            //Nu ser vi om der er f�rrer objekter i vores table end f�r
            Assert.AreEqual(startresult, Endresult + 1);

           

        }


        [TestMethod]
        public void TestOfSort()
        {


            Thread.Sleep(2000);
            //S�tter get all inventory button op og klikker p� den
            IWebElement getAllButtonElement = driver.FindElement(By.Id("getAllButton"));
            getAllButtonElement.Click();

            //s�tter alle sort buttons op
            IWebElement productNameButtonElement = driver.FindElement(By.Id("productName"));

            Thread.Sleep(200);
            IWebElement cell = driver.FindElement(By.ClassName("nameList"));


            //Klikker p� hver af knapperne
            Thread.Sleep(2000);
            //Tester om den sorter efter barcode
            productNameButtonElement.Click();
            Assert.AreEqual("Aalborg Akvavit", cell.Text);

            Thread.Sleep(200);

            //Tester om den reverse sorter efter barcode
            productNameButtonElement.Click();
            Assert.AreEqual("Økologisk Zucchini", cell.Text);
           



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

                    if (Element.Text != "THE ITEM IS STILL GOOD")
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
                double parsedInt;
                double.TryParse(Element.Text, NumberStyles.AllowDecimalPoint, CultureInfo.CreateSpecificCulture("en-US"), out parsedInt);

                if (parsedInt <= 3.0)
                {
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
            var objektListStart = driver.FindElements(By.Id("ShoppingListRows"));
            var startresult = objektListStart.Count;

            IWebElement getRecipes = driver.FindElement(By.Id("getRecipes"));
            getRecipes.Click();

            Thread.Sleep(2000);

            IWebElement InsertAmount = driver.FindElement(By.Id("itemAmount"));
            InsertAmount.SendKeys("2");
            
            IWebElement CreateShoppingList = driver.FindElement(By.Id("addToShopping"));
            CreateShoppingList.Click();

         

            var objektListEnd = driver.FindElements(By.Id("ShoppingListRows"));
            var Endresult = objektListEnd.Count;
            Assert.AreEqual(startresult + 1, Endresult);

            
        }


        [TestCleanup]
        public void TestOfCleanUp()
        {
            driver.Close();
        }
    }
}
