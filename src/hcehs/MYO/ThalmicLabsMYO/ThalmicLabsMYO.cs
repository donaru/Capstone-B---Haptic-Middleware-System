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
        static CommonGestures gesture = CommonGestures.Unknown;
        private static int[] circle;
        
        private static int circlepointer;
        static int oldyaw = 0;
        static int newroll;
        static int newpitch;
        static int newyaw;


        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public ListeningMode GetListeningMode { get; set; }

        public ThalmicLabsMYO()
        {
        }
        public void Initialise()
        {
            circle = new int[4];
            circlepointer = 0;
            for (int i = 0; i < 4; i++)
                circle[i] = 0;
            // create a hub that will manage Myo devices for us
            using (var hub = Hub.Create())
            {
                // listen for when the Myo connects
                hub.MyoConnected += (sender, e) =>
                {
                    Console.WriteLine("Myo {0} has connected!", e.Myo.Handle);
                    e.Myo.Vibrate(VibrationType.Medium);
                    
                    e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged += Myo_PoseChanged;
                    // for every Myo that connects, listen for special sequences
                    var clockwise_sequence = PoseSequence.Create(
                        e.Myo,
                        Pose.Fist,
                        Pose.WaveOut);
                    clockwise_sequence.PoseSequenceCompleted += CLKSequence_PoseSequenceCompleted;
                    var anticlockwise_sequence = PoseSequence.Create(
                        e.Myo,
                        Pose.Fist,
                        Pose.WaveIn);
                    anticlockwise_sequence.PoseSequenceCompleted += ACLKSequence_PoseSequenceCompleted;



                };

                // listen for when the Myo disconnects
                hub.MyoDisconnected += (sender, e) =>
                {
                    Console.WriteLine("Oh no! It looks like {0} arm Myo has disconnected!", e.Myo.Arm);
                    e.Myo.OrientationDataAcquired -= Myo_OrientationDataAcquired;
                    e.Myo.PoseChanged -= Myo_PoseChanged;
                };

                // wait on user input - Loop which is used to handle and manage different MYO devices
               // ConsoleHelper.UserInputLoop(hub);
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
             *          SwipeLeft, - DONE -
                        SwipeRight, - DONE -
                        CircleClockwise,
                        CircleAntiClockwise,
                        Okay, - DONE -
                        Unknown - DONE -
             * */
            return gesture;
        }
        public float[] getCoordinate()
        {
            float[] coordinate = new float[3];
            coordinate[0] = (float)newroll;
            coordinate[1] = (float)newpitch;
            coordinate[2] = (float)newyaw;
            return coordinate;
        }

        private static void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            var pi = (float)System.Math.PI;
            
            // convert the values to a 0-9 scale (for easier digestion/understanding)
            var roll = (int)((e.Roll + pi) / (pi * 2.0f) * 10);
            var pitch = (int)((e.Pitch + pi) / (pi * 2.0f) * 10);
            var yaw = (int)((e.Yaw + pi) / (pi * 2.0f) * 10);

            /* ---- PROTOTYPE CONSOLE OUTPUT
            Console.Clear();
            Console.WriteLine(@"Roll: {0}", roll);
            Console.WriteLine(@"Pitch: {0}", pitch);
            Console.WriteLine(@"Yaw: {0}", yaw);
            */

            // Update coordinate values for output
            newroll = roll;
            newpitch = pitch;
            newyaw = yaw;

            // *** Better logic will need to be implemented for circle gesture detection
            
            if (oldyaw != yaw)
            {
                if (circlepointer == 3)
                {   // 6 5 7 8
                    //Console.WriteLine(circle[0] + " " + circle[1] + " " + circle[2] + " " + circle[3]);
                    if ((circle[0] > circle[1]) && (circle[1] < circle[2]) && (circle[2] > circle[3]))
                    {
                       // Console.WriteLine("Clock-wise detected");
                       // gesture = CommonGestures.CircleClockwise;
                        circlepointer = 0;
                    }
                  
                    if ((circle[0] > circle[1]) && (circle[1] < circle[2]) && (circle[2] > circle[3]))
                    {
                       // Console.WriteLine("Anti Clock-wise detected");
                       // gesture = CommonGestures.CircleAntiClockwise;
                        circlepointer = 0;
                    }
                    else
                    {
                        
                        circle[0] = circle[1];
                        circle[1] = circle[2];
                        circle[2] = circle[3];
                        
                    }

                    //circle = null;
                    //circlepointer = 0;
                }
                try
                {
                    circle[circlepointer] = yaw;
                    if (circlepointer < 3) 
                        circlepointer++;
       
                    oldyaw = yaw;
                }
                catch (Exception j)
                {

                }
             

            }
            

        }

        private static void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            // ---- PROTOTYPE CONSOLE OUTPUT
            // Console.WriteLine("{0} arm Myo detected {1} pose!", e.Myo.Arm, e.Myo.Pose);

           
            if (e.Myo.Pose == Pose.WaveIn)
            {
                if (e.Myo.Arm == Arm.Right)
                    gesture = CommonGestures.SwipeLeft;
                else
                    gesture = CommonGestures.SwipeRight;
            }

            if (e.Myo.Pose == Pose.WaveOut)
            {
                if (e.Myo.Arm == Arm.Right)
                    gesture = CommonGestures.SwipeRight;
                else
                    gesture = CommonGestures.SwipeLeft;
            }

            if (e.Myo.Pose == Pose.FingersSpread)
                gesture = CommonGestures.Okay;

            if (e.Myo.Pose == Pose.Rest)
                gesture = CommonGestures.Unknown;


        }

        private static void CLKSequence_PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has performed a pose sequence!", e.Myo.Arm);
            e.Myo.Vibrate(VibrationType.Short);
            e.Myo.Vibrate(VibrationType.Short);
            gesture = CommonGestures.CircleClockwise;
        }
        private static void ACLKSequence_PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            Console.WriteLine("{0} arm Myo has performed a pose sequence!", e.Myo.Arm);
            e.Myo.Vibrate(VibrationType.Short);
            e.Myo.Vibrate(VibrationType.Short);
            e.Myo.Vibrate(VibrationType.Short);
            gesture = CommonGestures.CircleAntiClockwise;
        }


    }
}
