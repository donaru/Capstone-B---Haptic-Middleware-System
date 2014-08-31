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
    
            while(true)
            {
                Console.WriteLine(kinect.getDistance());
            }
            

        }
    }
}
