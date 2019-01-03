using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ClientSenderConsole
{
    class Program
    {
        public static void StartChat(string message)
        {
            // Data buffer untuk data masuk. 
            byte[] bytes = new byte[1024];

            // melakukan koneksi remote ke server. 
            try
            {
                // menentukan endpoint remote untuk socket.
                // contoh digunakan port 11000.
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // membuat socket TCP/IP.
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // melakukan koneksi ke socket remote endpoint.
                // penanganan jika error. 
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    // encode data string menjadi byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(message + "<EOF>");

                    // mengirim data via socket.
                    int bytesSent = sender.Send(msg);

                    // menerima respon dari server.
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Terkirim pada {0}",Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    Console.WriteLine("Konfirmasi diterima pada " + DateTime.Now.ToString());
                    Console.WriteLine();

                    // release socket. sender.Shutdown(SocketShutdown.Both); sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}",
                    ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}",
                    e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static int Main(string[] args)
        {
            bool isRepeat = true;
            while (isRepeat)
            {
                Console.Write("Pesan : ");
                string message = Console.ReadLine(); StartChat(message);

                if (!String.IsNullOrEmpty(message))
                {
                    if (message.ToUpper().Equals("EXIT"))
                    {
                        isRepeat = false;
                    }
                }
            }
            return 0;
        }
    }
}
