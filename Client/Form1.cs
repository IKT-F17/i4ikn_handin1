using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public partial class Form1 : Form
    {
        const int PORT = 9000;
        const int BUFSIZE = 1000;

        TcpClient clientSocket = new TcpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Msg("Client Started");
            clientSocket.Connect("127.0.0.1", 8888);
            label1.Text = "Client Socket Program - Server Connected ...";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            var outStream = Encoding.ASCII.GetBytes(textBox2.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            var inStream = new byte[10025];
            serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
            var returndata = Encoding.ASCII.GetString(inStream);
            Msg(returndata);
            textBox2.Text = "";
            textBox2.Focus();
        }

        public void Msg(string msg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + msg;
        }
    }
}