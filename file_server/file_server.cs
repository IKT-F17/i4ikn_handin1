using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		const int PORT = 9000;
		const int BUFSIZE = 1000;

		private file_server ()
		{
		    var localAddress = IPAddress.Parse("0.0.0.0");

            Console.WriteLine("CHOOSE IP ADDRESS FOR THE SERVER...");
		    Console.WriteLine("1. Default Local Host IP (127.0.0.1)");
		    Console.WriteLine("2. Default Remote Host IP (10.0.0.1)");
		    Console.WriteLine("3. Enter Custom IP Address.\n");
		    Console.Write("Your choice: ");
		    var choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    localAddress = IPAddress.Parse("127.0.0.1");
                    Console.WriteLine("Server IP address is now: 127.0.0.1\n");
                    break;

                case 2:
                    localAddress = IPAddress.Parse("10.0.0.1");
                    Console.WriteLine("Server IP address is now: 10.0.0.1\n");
                    break;

                case 3:
                    Console.Write("Type custom IP address here: ");
                    while (!IPAddress.TryParse(Console.ReadLine(), out localAddress))
                    {
                        Console.WriteLine("Invalid IP address, please try again.\n");
                        Console.Write("Type custom IP address here: ");
                    }
                    Console.WriteLine("Server IP Address is now: " + localAddress);
                    break;
            }

            try
		    {
		        var server = new TcpListener(localAddress, PORT);

		        while (true)
		        {
                    server.Start();
                    Console.WriteLine("Server: Started!");

                    Console.WriteLine("Server: Waiting for new connection...");
		            var client = server.AcceptTcpClient();
		            Console.WriteLine("Server: Client connected!");

                    // Modtager besked fra Client:
		            var stream = client.GetStream();

                    // Reads data from the client and prints it to console:
		            var fileName = LIB.readTextTCP(stream);
                    Console.WriteLine($"Stream received: {fileName}");

                    // Checks to see if file exists and returns its size:
		            var fileSize = LIB.check_File_Exists(fileName);

		            LIB.writeTextTCP(stream, fileSize.ToString());

		            if (fileSize != 0)
		                SendFile(fileName, fileSize, stream);
		            else
		                Console.WriteLine("File doesn't exist!");

                    client.Close();
                    server.Stop();
		        }
		    }

		    catch (SocketException e)
		    {
		        Console.WriteLine($"Socket exception: {e}");
		        Console.ReadKey();
                // Does server.Stop() run? Or does this potentially create memory leak???
		    }
        }

        private void SendFile (string fileName, long fileSize, NetworkStream io)
		{
		    var totalAmountSend = 0;

		    var bytes = new byte[BUFSIZE];
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

		    while (totalAmountSend < (int)fileSize)
		    {
		        var bytesRead = fs.Read(bytes, 0, BUFSIZE);

		        io.Write(bytes, 0, bytesRead);
		        totalAmountSend += bytesRead;

		        Console.WriteLine(bytesRead);
		    }

            fs.Close();
		}

        
	    public static void Main ()
		{
			Console.WriteLine ("Server: Starts...\n");
			new file_server();
		}
    }
}
