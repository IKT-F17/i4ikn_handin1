// *** http://csharp.net-informations.com/communications/csharp-server-socket.htm ***

using System;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		const int PORT = 9000;
		const int BUFSIZE = 1000;
	    const string IP = "127.0.0.1";

        /// <summary>
        /// New file server.
        /// </summary>
		//private file_server ()
		//{
		//    var requestCount = 0;

  //          TcpListener serverSocket = new TcpListener(8888);
  //          TcpClient clientSocket = default(TcpClient);

		//    serverSocket.Start();
		//    Console.WriteLine(" >> Server Started");

		//    clientSocket = serverSocket.AcceptTcpClient();
		//    Console.WriteLine(" >> Accept connection from client");

		//    while ((true))
		//    {
		//        try
		//        {
		//            requestCount = requestCount + 1;
		//            NetworkStream networkStream = clientSocket.GetStream();
		//            byte[] bytesFrom = new byte[10025];
		//            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
		//            string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
		//            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
		//            Console.WriteLine(" >> Data from client - " + dataFromClient);
		//            string serverResponse = "Last Message from client" + dataFromClient;
		//            Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
		//            networkStream.Write(sendBytes, 0, sendBytes.Length);
		//            networkStream.Flush();
		//            Console.WriteLine(" >> " + serverResponse);
		//        }
		//        catch (Exception ex)
		//        {
		//            Console.WriteLine(ex.ToString());
		//        }
		//    }

		//    clientSocket.Close();
		//    serverSocket.Stop();
		//    Console.WriteLine(" >> exit");
		//    Console.ReadLine();
  //      }

		/// <summary>
		/// Sends the file.
		/// </summary>
		//private void sendFile (String fileName, long fileSize, NetworkStream io)
		//{
		//	// TO DO Your own code
		//}


        // MAIN:
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
            //new file_server();

	        var ipAddress = IPAddress.Parse(IP);

		    var serverSocket = new TcpListener(ipAddress, PORT);
		    var requestCount = 0;
		    serverSocket.Start();
		    Console.WriteLine(" >> Server Started");
		    var clientSocket = serverSocket.AcceptTcpClient();
		    Console.WriteLine(" >> Accept connection from client");
		    requestCount = 0;

		    while ((true))
		    {
		        try
		        {
		            requestCount = requestCount + 1;
		            NetworkStream networkStream = clientSocket.GetStream();
		            byte[] bytesFrom = new byte[10025];
		            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
		            string dataFromClient = Encoding.ASCII.GetString(bytesFrom);
		            dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
		            Console.WriteLine(" >> Data from client - " + dataFromClient);
		            string serverResponse = "Last Message from client" + dataFromClient;
		            Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
		            networkStream.Write(sendBytes, 0, sendBytes.Length);
		            networkStream.Flush();
		            Console.WriteLine(" >> " + serverResponse);
		        }
		        catch (Exception ex)
		        {
		            Console.WriteLine(ex.ToString());
		        }
		    }

		    clientSocket.Close();
		    serverSocket.Stop();
		    Console.WriteLine(" >> exit");
		    Console.ReadLine();
		}
	}
}