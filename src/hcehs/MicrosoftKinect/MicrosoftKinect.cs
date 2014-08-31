using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using Microsoft.Kinect;

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
            Console.WriteLine("Initialized Kinect");
            try
            {
                sensor_SkeletonStream();
                this.sensor.Start();
                Console.WriteLine("Connected Kinect");
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
            
            return CommonGestures.Okay;
        }

        public float[] getCoordinate()
        {
            float[] coordinate = new float[2];
            coordinate[0] = xCoordinate;
            coordinate[1] = yCoordinate;
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
                    return;
                }
                if (firstSkeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                {
                    //this.MapJointsWithUIElement(fir)
                    distance = firstSkeleton.Joints[JointType.HandRight].Position.Z;
                    xCoordinate = firstSkeleton.Joints[JointType.HandRight].Position.X;
                    yCoordinate = firstSkeleton.Joints[JointType.HandRight].Position.Y;


                        
                }
                /*
                if (firstSkeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
                {
                    //this.MapJointsWithUIElement(fir)
                   textBox2.Text = (firstSkeleton.Joints[JointType.HandLeft].Position.Z).ToString();


                }*/

            }
        }

 
    

    }
}
