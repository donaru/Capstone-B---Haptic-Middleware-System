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
                      System.Threading.Thread.Sleep(450);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error recieved:" + e);
                }

            }
        }
    }
}
