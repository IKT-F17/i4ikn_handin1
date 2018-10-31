using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class UDPClient
{
    public static void Main()
    {
        bool isSending = true;
        UdpClient listener = new UdpClient(9000);
        IPEndPoint UDPServerEndPoint = new IPEndPoint(IPAddress.Parse("192.168.216.255"), 9000);
        string receivedData;
        byte[] receiveByteArray;
        byte[] dataRequest = new byte[10];

        try
        {
            while (isSending)
            {
                Console.WriteLine("Enter 'U' to view Servers Uptime \n" +
                                  "Or Enter 'L' to view Servers Average Load");
                dataRequest = Encoding.ASCII.GetBytes(Console.ReadLine());
                listener.Send(dataRequest, dataRequest.Length, UDPServerEndPoint);
                //isSending = false;
                isSending = !isSending;

                while (!isSending)
                {
                    Console.Clear();
                    Console.WriteLine("Waiting for broadcast");

                    receiveByteArray = listener.Receive(ref UDPServerEndPoint);
                    Console.WriteLine($"Received a broadcast from {UDPServerEndPoint.ToString()}");
                    receivedData = Encoding.ASCII.GetString(receiveByteArray, 0, receiveByteArray.Length);
                    Console.WriteLine($"Data received: \n{receivedData}\n\n");
                    //isSending = true;
                    isSending = !isSending;
                }
            }
        }

        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        listener.Close();
        return;
    }
}
