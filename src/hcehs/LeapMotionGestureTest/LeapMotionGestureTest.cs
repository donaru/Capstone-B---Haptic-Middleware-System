using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using HapCon.LeapMotion;

namespace HapCon.LeapMotionGestureTest
{
    class LeapMotionGestureTest
    {
        static void Main(string[] args)
        {
            float[] coordinates = new float[3];
            float[] oldcoordinates = new float[3];
            int deltax,deltay,deltaz;
            int velocity;
            LeapMotion.LeapMotion leapmotion = new LeapMotion.LeapMotion();
            leapmotion.SetParameters("Leap Motion", "local", ListeningMode.UsbConnection);
            leapmotion.Initialise();
            while (true)
            {
                try
                {
                    
                    CommonGestures gesture = leapmotion.getGesture();
                    switch(gesture)
                    {
                        case CommonGestures.CircleClockwise:
                            Console.WriteLine("Circle Clockwise");
                            break;
                        case CommonGestures.CircleAntiClockwise:
                            Console.WriteLine("Circle Anti Clockwise");
                            break;
                        case CommonGestures.SwipeLeft:
                            Console.WriteLine("Swipe Left");
                            break;
                        case CommonGestures.SwipeRight:
                            Console.WriteLine("Swipe Right");
                            break;
                        case CommonGestures.Okay:
                            Console.WriteLine("Okay");
                            break;
                        //case CommonGestures.Unknown:
                        //    Console.WriteLine("Unknown");
                        //    break;    

                    }

                    // Calculating velocity
                    oldcoordinates = coordinates;
                    coordinates = leapmotion.getCoordinate();
                    deltax = Convert.ToInt32(oldcoordinates[0]-coordinates[0]);
                    deltay = Convert.ToInt32(oldcoordinates[1]-coordinates[1]);
                    deltaz = Convert.ToInt32(oldcoordinates[2]-coordinates[2]);
                    velocity = (int) (Math.Sqrt(deltax ^ 2 + deltay ^ 2 + deltaz ^ 2) / 0.2);
                    //Console.WriteLine("x: " + coordinates[0] + " y:" + coordinates[1] + " z:" + coordinates[2]);
                    Console.WriteLine("velocity:" +  velocity + "millimeters/second" );
                      
                       
                      System.Threading.Thread.Sleep(200);

                }
                catch (Exception e)
                {
                    //Console.WriteLine("Error recieved:" + e);
                }
            }
        }
    }
}
