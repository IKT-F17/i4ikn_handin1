using System;
using System.IO;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		const int PORT = 9000;
		const int BUFSIZE = 1000;

		private file_client (string[] args)
		{
		    Console.WriteLine("Trying to connect...");

            // Opretter forbindelse til Client:
            var client = new TcpClient();
		    client.Connect(args[0], PORT);

            // Aktivere stream på den åbne forbindelsen:
		    var stream = client.GetStream();

            // Kontakter serveren og angiver hvilken fil:
		    LIB.writeTextTCP(stream, args[1]);
		    ReceiveFile(args[1], stream);

            stream.Close();
            client.Close();
		}

		private void ReceiveFile (string fileName, NetworkStream io)
		{
		    var fileSize = (int) LIB.getFileSizeTCP(io);

            if (fileSize != 0)
            {
                var totalAmountReceived = 0;

                var bytes = new byte[BUFSIZE];
                var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                while (totalAmountReceived < fileSize)
                {
                                                    // Kunne man ikke også bare have brugt BUFSIZE her?
                    var bytesRead = io.Read(bytes, 0, bytes.Length);

                    fs.Write(bytes, 0, bytesRead);
                    totalAmountReceived += bytesRead;

                    Array.Clear(bytes, 0, BUFSIZE);
                }

                fs.Close();
                //io.Close();   // Det behøver man vel ikke?
            }
            else
            {
                Console.WriteLine("No file found!");
            }
		}


		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
