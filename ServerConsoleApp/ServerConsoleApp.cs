using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace server
{
    class UDPServer
    {

        UdpClient socket;
        IPEndPoint IPAddressEndPoint;


        private UDPServer()
        {
            Console.WriteLine("Starting UDP Listener");
            IPAddressEndPoint = new IPEndPoint(IPAddress.Any, 9000);
            socket = new UdpClient(IPAddressEndPoint);
            byte[] data = new byte[1000];
            byte[] dataTBS = new byte[1000];

            while (true)
            {
                Console.WriteLine("Waiting for client");
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 8000);
                data = socket.Receive(ref sender);

                string cmd = Encoding.ASCII.GetString(data, 0, data.Length);
                Console.WriteLine($"Server recieved {cmd} from client");

                switch (cmd)
                {
                    case "U":
                    case "u":
                        SendUptime(dataTBS, sender);
                        break;
                    case "L":
                    case "l":
                        SendAverageLoad(dataTBS, sender);
                        break;
                    default:
                        SendbadArgumentError(data, sender);
                        break;
                }
            }
        }

        public static void Main(string[] args)
        {
            new UDPServer();
        }

        private void SendUptime(byte[] dataTBS, IPEndPoint sender)
        {
            FileStream uptime = new FileStream("/proc/uptime", FileMode.Open, FileAccess.Read);
            int bytesRead = uptime.Read(dataTBS, 0, dataTBS.Length);
            socket.Send(dataTBS, dataTBS.Length, sender);
        }


        private void SendAverageLoad(byte[] dataTBS, IPEndPoint sender)
        {
            FileStream loadavg = new FileStream("/proc/loadavg", FileMode.Open, FileAccess.Read);
            int bytesRead = loadavg.Read(dataTBS, 0, dataTBS.Length);
            socket.Send(dataTBS, dataTBS.Length, sender);
        }

        private void SendbadArgumentError(byte[] data, IPEndPoint sender)
        {
            string badArgumentError = "Did not compute - Please use U, u, L or l as argument";
            data = Encoding.ASCII.GetBytes(badArgumentError);
            Console.WriteLine(badArgumentError);
            socket.Send(data, data.Length, sender);
        }

    }
}