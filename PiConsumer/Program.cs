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

            bool debugPost = true;

            if(debugPost == false) { 

                IPAddress anyIpAddress = IPAddress.Any;
                IPEndPoint remoteEndPoint = new IPEndPoint(anyIpAddress, 9050);
                UdpClient server = new UdpClient(9050);

                while (true)
                {

                    int barcode = ProductPoster.UDPToBarcode(server, remoteEndPoint);

                    Console.WriteLine(barcode);

                    ProductPoster.PostProductInstance(barcode);
                }
            }
            else
            {
                ProductPoster.PostProductInstance(10000007);
                ProductPoster.PostProductInstance(69696969);
                ProductPoster.PostProductInstance(10000005);
                ProductPoster.PostProductInstance(10000004);
                ProductPoster.PostProductInstance(10000003);
                ProductPoster.PostProductInstance(69);
            }
        }
    }
}
