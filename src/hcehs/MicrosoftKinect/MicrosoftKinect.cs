using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using Microsoft.Kinect;
using GesturePak;
using System.Timers;
using System.Diagnostics;


namespace HapCon.MicrosoftKinect
{
    public class MicrosoftKinect : IMicrosoftKinect
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public ListeningMode GetListeningMode { get; set; }

        KinectSensor sensor;
        Skeleton[] totalSkeleton;
        static float xCoordinate;
        static float yCoordinate;
        static float distance;

        // Matcher
        private GestureMatcher matcher;

        // Path to gesture. Point this to any GesturePak created gesture
       // private string gesturefile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\GesturePak\\Flap.xml";
        private string gesturefile  = "Okay.xml";
        private string gesturefile1 = "SwipeLeft.xml";
        private string gesturefile2 = "SwipeRight.xml";
        private string gesturefile3 = "CircleClockwise.xml";
        private string gesturefile4 = "CircleAntiClockwise.xml";


        public bool gestureFound = false;
        public string gestureName;
        public bool regGesture;
       

        public MicrosoftKinect()
        {


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
                throw new InvalidOperationException("Error: unable to set parameters to Microsoft Kinect :" + e);
            }
        }


        public void Initialise()
        {

            // Make sure we have a Kinect
            if (KinectSensor.KinectSensors.Count == 0)
            {
                throw new InvalidOperationException("Please plug in your Kinect and try again");
            }

            // Make sure we have a gesture file
            if (System.IO.File.Exists(gesturefile) == false)
            {
                throw new InvalidOperationException("Please modify this code to point to an existing gesture file.");
            }

            Console.WriteLine("Initialized Kinect");
            try
            {
                sensor_SkeletonStream();
               
                this.sensor.Start();
                this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
                Console.WriteLine("Connected Kinect");

                // Create your gesture objects (however many you want to test)
                Gesture g1 = new Gesture(gesturefile);
                Gesture g2 = new Gesture(gesturefile1);
                Gesture g3 = new Gesture(gesturefile2);
                Gesture g4 = new Gesture(gesturefile3);
                Gesture g5 = new Gesture(gesturefile4);

                // Add it to a gestures collection
                List<Gesture> gestures = new List<Gesture>();
                gestures.Add(g1);
                gestures.Add(g2);
                gestures.Add(g3);
                gestures.Add(g4);
                gestures.Add(g5);


                // Create a new matcher from the Kinect sensor and the gestures
                matcher = new GestureMatcher(KinectSensor.KinectSensors[0], gestures);

                // hook up events
                matcher.StartedRecognizing += matcher_StartedRecognizing;
                matcher.DoneRecognizing += matcher_DoneRecognizing;
                matcher.Tracking += matcher_Tracking;
                matcher.NotTracking += matcher_NotTracking;
                matcher.PoseMatch += matcher_PoseMatch;
                matcher.GestureMatch += matcher_GestureMatch;

                // Start recognizing your gestures!
                matcher.StartRecognizing();
                regGesture = true;

            } catch (Exception e)
            {
                throw new InvalidOperationException("Error: unable to initialise Microsoft Kinect :" + e);
            }
        }


        public void Shutdown()
        {
            try
            {
                this.sensor.Stop();
            } 
            catch (Exception e)
            {
                throw new InvalidOperationException("Error: unable to shutdown Microsoft Kinect :" + e);
            }
        }
      
        
       
        public CommonGestures getGesture()
        {
            if (gestureFound)
            {
                gestureFound = false;
                if(gestureName == "Flap")
                    return CommonGestures.Okay;
            }
            return CommonGestures.Unknown;
        }

         
        public float[] getCoordinate()
        {
            float[] coordinate = new float[3];
            coordinate[0] = xCoordinate;
            coordinate[1] = yCoordinate;
            coordinate[2] = distance;
            return coordinate;
        }

        public float getDistance()
        {
            try
            {
                return distance;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error: unable return distance :" + e);
            }
            
        }

        private void sensor_SkeletonStream()
        {
            this.sensor = KinectSensor.KinectSensors.Where(item => item.Status == KinectStatus.Connected).FirstOrDefault();
            if (!this.sensor.SkeletonStream.IsEnabled)
            {
                this.sensor.SkeletonStream.Enable();
                this.sensor.SkeletonFrameReady += sensor_SkeletonFrameReady;
            }

        }
        private void sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            totalSkeleton = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                {
                    return;
                }
                skeletonFrame.CopySkeletonDataTo(totalSkeleton);
                Skeleton firstSkeleton = (from trackskeleton in totalSkeleton where trackskeleton.TrackingState == SkeletonTrackingState.Tracked select trackskeleton).FirstOrDefault();
                if (firstSkeleton == null)
                {
                    //matcher.DoneRecognizing(); 
                    regGesture = false;
                    return;
                }
                if (firstSkeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                {
                    //this.MapJointsWithUIElement(fir)
                    distance = firstSkeleton.Joints[JointType.HandRight].Position.Z;
                    xCoordinate = firstSkeleton.Joints[JointType.HandRight].Position.X;
                    yCoordinate = firstSkeleton.Joints[JointType.HandRight].Position.Y;
                    
                    if(!regGesture)
                    {
                        matcher.StartRecognizing();
                        regGesture = true;
                        
                    }
                        
                }
                /*
                if (firstSkeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
                {
                    //this.MapJointsWithUIElement(fir)
                   textBox2.Text = (firstSkeleton.Joints[JointType.HandLeft].Position.Z).ToString();


                }*/

            }
        }

        private void matcher_StartedRecognizing()
        {

            // This tells us the matcher is recognizing
            Console.WriteLine("Watching...");

        }

        private void matcher_DoneRecognizing()
        {
            // This tells us the matcher is NOT recognizing
            Console.WriteLine("Not Watching");
        }

        private void matcher_Tracking(Pose pose, float delta)
        {
            // The window goes red when tracking
            Console.WriteLine("Tracking ...");
        }

        private void matcher_NotTracking()
        {
            // The window goes white when not tracking
            Console.WriteLine("Not Tracking");


        }

        private void matcher_PoseMatch(MatchingPose match, Pose pose)
        {
            //  We have matched a pose. 
            //  match.Pose is the pose from the gesture
            //  pose is the current frame (real time)
        }

        private void matcher_GestureMatch(Gesture gesture)
        {
            // We got a match!
            Console.WriteLine("Found: " + gesture.Name);
            gestureName = gesture.Name;
            gestureFound = true;
        }
 
    

    }
}
