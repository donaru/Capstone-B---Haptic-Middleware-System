using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using HapCon.MicrosoftKinect;
using Microsoft.Kinect;

namespace HapCon.MicrosoftKinectFunctionalityTest
{
    class MicrosoftKinectFunctionalityTest
    {
        static void Main(string[] args)
        {
            MicrosoftKinect.MicrosoftKinect kinect = new MicrosoftKinect.MicrosoftKinect();
            kinect.SetParameters("Kinect", "local", ListeningMode.UsbConnection);
            kinect.Initialise();
           // float previousDistance = 0;
           // float newDistance;
            while(true)
            {
                /*
                newDistance = kinect.getDistance();
                if (newDistance != 0 || newDistance != previousDistance)
                {
                    Console.WriteLine(newDistance);
                    previousDistance = newDistance;
                }

                System.Threading.Thread.Sleep(1000);*/

                if (kinect.getGesture() == CommonGestures.Okay)
                {
                    Console.WriteLine("Okay");
                }
            }
            

        }
    }
}
