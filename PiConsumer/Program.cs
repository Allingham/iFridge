using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using FridgeModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PiConsumer
{
    class Program
    {
        static void Main(string[] args)
        {

            IPAddress anyIpAddress = IPAddress.Any;
            IPEndPoint remoteEndPoint = new IPEndPoint(anyIpAddress, 9050);
            UdpClient server = new UdpClient(9050);

            while (true)
            {
                Console.WriteLine("Listening for new product");
                ProductPoster.PostProductInstance(ProductPoster.ListenForNewProduct(server,remoteEndPoint));
            }
        }
    }
}
