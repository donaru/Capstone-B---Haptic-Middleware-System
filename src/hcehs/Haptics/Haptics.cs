using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using HapCon.LeapMotion;
using HapCon.MicrosoftKinect;
using HapCon.ThalmicLabsMYO;

namespace HapCon.Haptics
{
    public class Haptics : IHaptics
    {
        public enum HapticDevices
        {
            LeapMotion,
            Kinect,
            MYO,
            Unknown
        }

        LeapMotion.LeapMotion leapmotion = new LeapMotion.LeapMotion();

        MicrosoftKinect.MicrosoftKinect kinect = new MicrosoftKinect.MicrosoftKinect();

        ThalmicLabsMYO.ThalmicLabsMYO myo = new ThalmicLabsMYO.ThalmicLabsMYO();

        HapticDevices currentDevice = HapticDevices.Unknown;

        static float oldkinect_distance = 0;
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public ListeningMode GetListeningMode { get; set; }

        public Haptics()
        {     
        }

        // Set parameters are not used as 
        public void SetParameters(string name, string computerName, ListeningMode listeningMode)
        { }

        public void Initialise()
        {
            //Leap Motion
            leapmotion.SetParameters("Leap Motion", "local", ListeningMode.UsbConnection);
            leapmotion.Initialise();
            // Microsoft Kinect
            kinect.SetParameters("Kinect", "local", ListeningMode.UsbConnection);
            kinect.Initialise();
            // MYO
            myo.SetParameters("MYO", "local", ListeningMode.BluetoothConnection);
            myo.Initialise();
        }
        public void Shutdown()
        {
            leapmotion.Shutdown();
            kinect.Shutdown();
            myo.Shutdown();
        }

        public CommonGestures getGesture()
        {
            CommonGestures leapmotion_gesture = leapmotion.getGesture();
            CommonGestures kinect_gesture = kinect.getGesture();
            CommonGestures myo_gesture = myo.getGesture();

            switch (currentDevice)
            {
                case HapticDevices.MYO:
                    return myo_gesture;

                case HapticDevices.LeapMotion:
                    if (leapmotion_gesture == CommonGestures.Unknown) //If unable to locate any gesture from leap, just double check on MYO
                        return myo_gesture;
                    else
                        return leapmotion_gesture;

                case HapticDevices.Kinect:
                    if (kinect_gesture == CommonGestures.Unknown)
                        return myo_gesture;
                    else
                        return kinect_gesture;

            }
         
            return CommonGestures.Unknown;
        }
        public float[] getCoordinate()
        {
            float[] coordinates = new float[3];
            coordinates[0] = 0;
            coordinates[1] = 0;
            coordinates[2] = 0;
            switch(currentDevice)
            {
                case HapticDevices.LeapMotion:
                    coordinates = leapmotion.getCoordinate();
                    if (coordinates != null)
                    {
                        coordinates[0] = coordinates[0] / 10;
                        coordinates[1] = coordinates[1] / 10;
                        coordinates[2] = coordinates[2] / 10;
                    }
                    return coordinates;
                case HapticDevices.Kinect:
                    coordinates = kinect.getCoordinate();
                    if (coordinates != null)
                    {
                        coordinates[0] = coordinates[0] * 100;
                        coordinates[1] = coordinates[1] * 100;
                        coordinates[2] = coordinates[2] * 100;
                    }
                    return coordinates;
            }
            return coordinates;
        }

        public float[] eulerAngle()
        {
            return myo.getCoordinate();
        }

        public float getDistance()
        {
            float[] coordinates = new float[3];
            float leapmotion_distance = 0;
            float kinect_distance = kinect.getDistance();
            coordinates = leapmotion.getCoordinate();
            if (coordinates != null)
                leapmotion_distance = (float)Math.Sqrt(Math.Pow(coordinates[0], 2) + Math.Pow(coordinates[1], 2) + Math.Pow(coordinates[2], 2));
            if ((leapmotion_distance > 0))
            {
                currentDevice = HapticDevices.LeapMotion;
                return leapmotion_distance / 10;
            }   
            if ((leapmotion_distance == 0) && (kinect_distance > 0))
            {
                currentDevice = HapticDevices.Kinect;
                if (kinect_distance == oldkinect_distance)
                {
                    currentDevice = HapticDevices.MYO;
                    return 0;
                } else
                {
                    oldkinect_distance = kinect_distance;
                    return kinect_distance*100;
                }
                
            }
            else
            {
                currentDevice = HapticDevices.MYO;
                return 0;
            }
                
        }

        public HapticDevices deviceSelected()
        {
            return currentDevice;
        }

    }
}
