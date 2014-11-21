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
            float previousDistance = 0;
            float newDistance;

            float[] coordinates = new float[3];
            float[] oldcoordinates = new float[3];
            double deltax, deltay, deltaz;
            double velocity;

            while(true)
            {
               
                newDistance = kinect.getDistance();
                if (newDistance != 0 || newDistance != previousDistance)
                {
                    Console.WriteLine(newDistance);
                    previousDistance = newDistance;
                }
                
                //System.Threading.Thread.Sleep(1000);


                


                if (kinect.getGesture() == CommonGestures.SwipeLeft)
                {
                    Console.WriteLine("Swipe Left detected");
                    System.Threading.Thread.Sleep(10000); ;
                }

                // Calculating velocity
                oldcoordinates = coordinates;
                coordinates = kinect.getCoordinate();
                deltax = oldcoordinates[0] - coordinates[0];
                deltay = oldcoordinates[1] - coordinates[1];
                deltaz = oldcoordinates[2] - coordinates[2];

                velocity = (Math.Sqrt(Math.Pow(deltax, 2) + Math.Pow(deltay, 2) + Math.Pow(deltaz, 2)) / 0.2);
                //Console.WriteLine("x: " + coordinates[0] + " y:" + coordinates[1] + " z:" + coordinates[2]);
                Console.WriteLine("velocity:" + velocity + "m/second");


                System.Threading.Thread.Sleep(200);
            }
            

        }
    }
}
