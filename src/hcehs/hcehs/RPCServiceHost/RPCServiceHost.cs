using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using HapCon.HapticService;
using HapCon.Common;

namespace HapCon.RPCServiceHost
{
    public class RPCServiceHost
    {

        public static void Main()
        {

            using (ServiceHost host = new ServiceHost(typeof(HapticService.HapticService)))
            {
                
                host.Open();
                
                Console.WriteLine("Service started at " + DateTime.Now.ToString());

                /*
                //testing leap motion
                LeapMotion.LeapMotion leapmotion = new LeapMotion.LeapMotion();
                leapmotion.SetParameters("Leap Motion", "local", ListeningMode.UsbConnection);
                leapmotion.Initialise();
                while (true)
                {
                    float[] coordinates = leapmotion.getCoordinate();
                    if (coordinates != null)
                    {
                        Console.WriteLine("X: " + coordinates[0] + ", Y: " + coordinates[1]);
                    }
                    
                }*/

                Console.ReadLine();
            }
        }
    }
}
