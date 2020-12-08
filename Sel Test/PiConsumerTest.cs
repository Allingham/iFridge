using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FridgeModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PiConsumer;

namespace Sel_Test
{
    [TestClass]
    public class PiConsumerTest
    {

        [TestMethod]
        public void TestReceive()
        {
            //arrange
            int number = 999;

            new Task(() =>
            {
                UdpClient sender = new UdpClient(0);
                sender.EnableBroadcast = true;

                IPAddress anyIP = IPAddress.Broadcast;
                //IPEndPoint broadEndPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"),9000);
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, 9050);

                Thread.Sleep(2000);

                while (true){
                    number = 1;
                    
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(number.ToString());
                    sender.Send(sendBytes, sendBytes.Length, remoteEndPoint);

                    Thread.Sleep(100);

                    sendBytes = Encoding.ASCII.GetBytes("#");
                    sender.Send(sendBytes, sendBytes.Length, remoteEndPoint);
                }
            }).Start();

            IPAddress anyIpAddress = IPAddress.Any;
            IPEndPoint remoteEndPoint = new IPEndPoint(anyIpAddress, 9050);
            UdpClient server = new UdpClient(9050);

            //act
            int receivedNumber = 0;

            Assert.AreEqual(ProductPoster.UDPToBarcode(server,remoteEndPoint), 1);

            //receivedNumber = ProductPoster.ListenForNewProduct(server, remoteEndPoint);

            //assert

            //Assert.AreEqual(number, receivedNumber);

        }

        [TestMethod]
        public void TestPost()
        {
            //arrange
            int barcode = 10000001;

            //act + assert
            Assert.AreEqual(ProductPoster.PostProductInstance(barcode).StatusCode, HttpStatusCode.Created);




        }
    }
}
