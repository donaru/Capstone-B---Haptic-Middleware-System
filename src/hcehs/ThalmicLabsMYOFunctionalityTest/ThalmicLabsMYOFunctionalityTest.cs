using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using HapCon.ThalmicLabsMYO;
using MyoSharp.Device;
using MyoSharp.Poses;
using MyoSharp.ConsoleSample.Internal;

namespace HapCon.ThalmicLabsMYOFunctionalityTest
{
    class ThalmicLabsMYOFunctionalityTest
    {
        static CommonGestures previousgesture;
        static void Main(string[] args)
        {
            
            ThalmicLabsMYO.ThalmicLabsMYO myo = new ThalmicLabsMYO.ThalmicLabsMYO();
            myo.SetParameters("MYO", "local", ListeningMode.UsbConnection);
            myo.Initialise();
            Console.WriteLine("Listening");
            while(true)
            {
                CommonGestures newgesture = myo.getGesture();
                if (newgesture != previousgesture)
                {
                    switch (myo.getGesture())
                    {
                        case CommonGestures.SwipeLeft:
                            Console.WriteLine("Swipe Left");
                            break;
                        case CommonGestures.SwipeRight:
                            Console.WriteLine("Swipe Right");
                            break;
                        case CommonGestures.Unknown:
                            Console.WriteLine("Rest");
                            break;


                    }
                    previousgesture = newgesture;
                }
            }

        }
    }
}
