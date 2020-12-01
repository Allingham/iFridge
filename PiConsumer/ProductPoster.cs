using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using FridgeModels;

namespace PiConsumer
{
    public static class ProductPoster
    {

        public static bool PostPorductInstance(int barcode){

            throw new NotImplementedException();

            ProductInstance newInstance = new ProductInstance(){
                Barcode = barcode,
                DateAdded = DateTime.Now,
                OwnerId = 0
            };

            

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
