using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		private file_server ()
		{
		    var localAddress = IPAddress.Parse("127.0.0.1");

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
// Does server.Stop() run? Or does this potentially create memory leak???
            }
        }

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
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

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server: Starts...");
			new file_server();
		}
	}
}
