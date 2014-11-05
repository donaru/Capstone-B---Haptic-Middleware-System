using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using MyoSharp.Device;
using MyoSharp.Poses;
using MyoSharp.ConsoleSample.Internal;

namespace HapCon.ThalmicLabsMYO
{
    public class ThalmicLabsMYO : IThalmicLabsMYO
    {
        
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public ListeningMode GetListeningMode { get; set; }

        public ThalmicLabsMYO()
        {
        }
        public void Initialise()
        {
            // create a hub that will manage Myo devices for us
            using (var hub = Hub.Create())
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Short);
                    e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged += Myo_PoseChanged;
                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.OrientationDataAcquired -= Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                };

                // wait on user input
                ConsoleHelper.UserInputLoop(hub);
            }
        
        }
        public void Shutdown()
        {
            using (var hub = Hub.Create())
            {
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Shutting down MYO", e.Myo.Arm);
                    e.Myo.OrientationDataAcquired -= Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                };
            }
        }

        public void SetParameters(string name, string computerName, ListeningMode listeningMode)
        {
            try
            {
                this.Name = name;
                this.SetComputerName = computerName;
                this.GetListeningMode = listeningMode;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error: unable to set parameters to Thalmic Labs MYO :" + e);
            }
        }

        public CommonGestures getGesture()
        {
            /*
             *          SwipeLeft,
                        SwipeRight,
                        CircleClockwise,
                        CircleAntiClockwise,
                        Okay,
                        Unknown
             * */
            return CommonGestures.SwipeLeft;
        }
        public float[] getCoordinate()
        {
            float[] coordinate = new float[3];
            return coordinate;
        }

        private static void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            var pi = (float)System.Math.PI;
            
            // convert the values to a 0-9 scale (for easier digestion/understanding)
            var roll = (int)((e.Roll + pi) / (pi * 2.0f) * 10);
            var pitch = (int)((e.Pitch + pi) / (pi * 2.0f) * 10);
            var yaw = (int)((e.Yaw + pi) / (pi * 2.0f) * 10);

            Console.Clear();
            Console.WriteLine(@"Roll: {0}", roll);
            Console.WriteLine(@"Pitch: {0}", pitch);
            Console.WriteLine(@"Yaw: {0}", yaw);
        }

        private static void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);
        }
    }
}
