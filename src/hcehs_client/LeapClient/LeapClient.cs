using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LeapClient
{
    public class LeapClient
    {
        public void run() 
        {
            string message = "";
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect("127.0.0.1", 8001);
                // use the ipaddress as in the server program

                Console.WriteLine("Connected");
                //Console.Write("Enter the string to be transmitted : ");

                // String str = Console.ReadLine();
                
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();

                while (true)
                {


                    byte[] bb = new byte[100];
                    int k = stm.Read(bb, 0, 100);
                    
                    for (int i = 0; i < k; i++)
                    {
                        if (Convert.ToChar(bb[i]) != '!')
                            message = message + Convert.ToChar(bb[i]);

                    }
                    Console.WriteLine(message);
                    //Console.WriteLine();
                     
                }
                

                tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }
           // return message;
        }
        
    }
}
