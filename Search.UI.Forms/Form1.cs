using Newtonsoft.Json;
using SharedLibrary.Data.Domain;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Search.UI.Forms
{
    public partial class Form1 : Form
    {
        private TcpClient _client;

        public Form1()
        {
            InitializeComponent();

            Thread.Sleep(1000);
            _client = new TcpClient("localhost", 9999);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var networkStream = _client.GetStream();

            var bytes = Encoding.UTF8.GetBytes(textBox1.Text);
            networkStream.Write(bytes, 0, bytes.Length);

            bytes = new byte[10240];
            networkStream.Read(bytes, 0, bytes.Length);
            var message = Encoding.UTF8.GetString(bytes).Trim('\0');

            var results = JsonConvert.DeserializeObject<List<Result>>(message);

            networkStream.Close();
        }
    }
}
