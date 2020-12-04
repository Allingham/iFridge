using System;
using System.Net;
using System.Net.Sockets;

namespace PiConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress anyIpAddress = IPAddress.Any;
            IPEndPoint remoteEndPoint = new IPEndPoint(anyIpAddress, 9050);
            UdpClient server = new UdpClient(9050);

            Console.WriteLine(ProductPoster.UDPToBarcode(server, remoteEndPoint));

        }
    }
}
