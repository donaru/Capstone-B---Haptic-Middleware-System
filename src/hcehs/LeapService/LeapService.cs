using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace HapCon.LeapService
{
    public class LeapService
    {
        public void run()
        {
            try
            {
                LeapMotion.LeapMotion leapmotion = new LeapMotion.LeapMotion();
                leapmotion.SetParameters("Leap Motion", "local", ListeningMode.UsbConnection);
                leapmotion.Initialise();

                IPAddress ipAd = IPAddress.Parse("127.0.0.1");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 8001);

                /* Start Listeneting at the specified port */
                myList.Start();

                Console.WriteLine("The server is running at port 8001...");
                Console.WriteLine("The local End point is  :" +
                                  myList.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");

                Socket s = myList.AcceptSocket();
                Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

                String str = "";
               

                ASCIIEncoding asen = new ASCIIEncoding();
                float[] lastValue = new float[2];
                float[] coordinates = new float[2];

                while (true)
                {
                    try
                    {
                        
                        
                        coordinates = leapmotion.getCoordinate();
                        //Console.WriteLine("X: " + coordinates[0] + ", Y: " + coordinates[1]);
                        str = lastValue[0] + "," + lastValue[1] + "!";
                        
                        if (coordinates != null)
                        {
                            str = coordinates[0] + "," + coordinates[1] + "!";

                            //Console.WriteLine("X: " + coordinates[0] + ", Y: " + coordinates[1]);
                            
                            //Console.WriteLine("Sent: " + str);
                            
                        }
                        s.Send(asen.GetBytes(str));


                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine("Error recieved:" + e);
                    }

                }


                /* clean up */
                s.Close();
                myList.Stop();
                Console.ReadLine();




                //tcpclnt.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }

        }
    }
}
