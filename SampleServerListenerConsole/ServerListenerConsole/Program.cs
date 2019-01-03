using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ServerListenerConsole
{
    class Program
    {
        // data masuk dari client.
        public static string data = null;

        public static void StartListening()
        {
            // data buffer untuk data masuk.
            byte[] bytes = new Byte[1024];

            // menentukan lokal endpoint untuk socket.
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            // membuat socket TCP/IP.
            
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); //new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Bind socket ke local endpoint dan
            // mendengarkan data yang masuk. 
            try
            {
                Console.WriteLine("Hostname : " + ipHostInfo.HostName);
                Console.WriteLine("IP Addr : " + localEndPoint.Address);
                Console.WriteLine("Port : " + localEndPoint.Port);

                listener.Bind(localEndPoint);
                listener.Listen(10);
                while (true)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Menunggu pesan baru...");
                    // Program dihentikan saat menunggu koneksi yang masuk. 
                    Socket handler = listener.Accept();
                    data = null;
                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    // menampilkan data pada layar.
                    data = data.Substring(0, data.Length - 5);
                    Console.WriteLine(DateTime.Now.ToString() + " : {0}", data);

                    // mengirimkan balasan ke client. 
                    data = DateTime.Now.ToString();
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        static int Main(string[] args)
        {
            StartListening();
            return 0;
        }
    }
}
