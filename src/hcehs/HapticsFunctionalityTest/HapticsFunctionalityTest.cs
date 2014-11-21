using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using HapCon.Haptics;

namespace HapCon.HapticsFunctionalityTest
{
    class HapticsFunctionalityTest
    {
  
        static void Main(string[] args)
        {
            CommonGestures oldgesture = CommonGestures.Unknown;
            float[] coordinate = new float[3];
            float previous_distance = 0;

            Haptics.Haptics controller = new Haptics.Haptics();
            controller.Initialise();

            while(true)
            {
                // ********************** Get Distance
                
                float newdistance = controller.getDistance();
                
                /*
                if (newdistance != previous_distance)
                {
                    Console.WriteLine(newdistance + "cm");
                    previous_distance = newdistance;
                }*/
                 
                
                // ******************************** Get current device
                
                switch(controller.deviceSelected())
                {
                    case Haptics.Haptics.HapticDevices.LeapMotion:
                        Console.WriteLine("Leap Motion within user range");
                        break;
                    case Haptics.Haptics.HapticDevices.Kinect:
                        Console.WriteLine("Kinect within user range");
                        break;
                    case Haptics.Haptics.HapticDevices.MYO:
                        Console.WriteLine("Grey area detected. MYO is being used as backup");
                        break;
                    case Haptics.Haptics.HapticDevices.Unknown:
                        Console.WriteLine("User not detected");
                        break;
                }

                // **************************************** Get Coordinate
                /*
                coordinate = controller.getCoordinate();
                if (coordinate != null)
                    Console.WriteLine("x:" + coordinate[0] + "y:" + coordinate[1] + "z:" + coordinate[2]);
                */


                // Get Velocity

                // Get Gesture
                CommonGestures gesture = controller.getGesture();
                if (gesture != oldgesture)
                {
                    switch (gesture)
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
                        case CommonGestures.Unknown:
                            Console.WriteLine("Unknown");
                            break;    
                            
                    }
                    
                    oldgesture = gesture;
                }
            }

        }
    }
}
