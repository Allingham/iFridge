using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using FridgeModels;
using Newtonsoft.Json;

namespace PiConsumer
{
    public static class ProductPoster
    {

        public static HttpResponseMessage PostProductInstance(int barcode){

            //string ProductInstanceEndPoint = "https://ifridgeapi.azurewebsites.net/api/ProductInsta";
            string ProductInstanceEndPoint = "https://ifridgeapi.azurewebsites.net/api/ProductInstances";

            ProductInstance newInstance = new ProductInstance()
            {
                Barcode = barcode,
                DateAdded = DateTime.Now,
                OwnerId = 0
            };
            
            using (HttpClient client = new HttpClient())
            {
                string ProductInstanceString = JsonConvert.SerializeObject(newInstance);

                Console.WriteLine(ProductInstanceString);

                StringContent content = new StringContent(ProductInstanceString, Encoding.UTF8, "application/json");



                HttpResponseMessage response = client.PostAsync(ProductInstanceEndPoint, content).Result;

                Console.WriteLine(response);

                return response;
            }


        }

        public static int ListenForNewProduct(UdpClient listener, IPEndPoint remoteEndPoint){
            //Byte[] receivedBytes = server.Receive(ref remoteEndPoint);
            Byte[] receivedBytes = listener.Receive(ref remoteEndPoint);


            string receivedData = Encoding.ASCII.GetString(receivedBytes);

            Console.WriteLine(receivedData);

            int recievedInt;

            Int32.TryParse(receivedData, out recievedInt);

            if (recievedInt == null) throw new ArgumentOutOfRangeException();

            return recievedInt;
        }

    }
}
