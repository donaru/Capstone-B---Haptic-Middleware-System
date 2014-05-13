using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using Leap;

namespace HapCon.LeapMotion
{
    public class LeapMotion : ILeapMotion
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public ListeningMode GetListeningMode { get; set; }

        LeapListener _listener = new LeapListener();
        Controller _controller = new Controller();
        public LeapMotion()
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
                throw new InvalidOperationException("Error: unable to set parameters to Leap device :" + e);
            }


        }
        public void Initialise()
        {
            try
            {
                _controller.AddListener(_listener);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error: unable to initialize Leap device :" + e);
            }

        }

        public void Shutdown()
        {
            _controller.RemoveListener(_listener);
            _controller.Dispose();
        }

        public string getGesture()
        {
            Frame frame = _controller.Frame();
            GestureList gestures = frame.Gestures();
            for (int i = 0; i < gestures.Count; i++)
            {
                Gesture gesture = gestures[i];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPECIRCLE:
                        CircleGesture circle = new CircleGesture(gesture);

                        // Calculate clock direction using the angle between circle normal and pointable
                        String clockwiseness;
                        if (circle.Pointable.Direction.AngleTo(circle.Normal) <= Math.PI / 4)
                        {
                            //Clockwise if angle is less than 90 degrees
                            clockwiseness = "clockwise";
                        }
                        else
                        {
                            clockwiseness = "counterclockwise";
                        }

                        float sweptAngle = 0;

                        // Calculate angle swept since last frame
                        if (circle.State != Gesture.GestureState.STATESTART)
                        {
                            CircleGesture previousUpdate = new CircleGesture(_controller.Frame(1).Gesture(circle.Id));
                            sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                        }

                        return("Circle id: " + circle.Id
                                       + ", " + circle.State
                                       + ", progress: " + circle.Progress
                                       + ", radius: " + circle.Radius
                                       + ", angle: " + sweptAngle
                                       + ", " + clockwiseness);
                        
                    case Gesture.GestureType.TYPESWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        return("Swipe id: " + swipe.Id
                                       + ", " + swipe.State
                                       + ", position: " + swipe.Position
                                       + ", direction: " + swipe.Direction
                                       + ", speed: " + swipe.Speed);
                        
                    case Gesture.GestureType.TYPEKEYTAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        return("Tap id: " + keytap.Id
                                       + ", " + keytap.State
                                       + ", position: " + keytap.Position
                                       + ", direction: " + keytap.Direction);
                        
                    case Gesture.GestureType.TYPESCREENTAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        return("Tap id: " + screentap.Id
                                       + ", " + screentap.State
                                       + ", position: " + screentap.Position
                                       + ", direction: " + screentap.Direction);
                        
                    default:
                        return("Unknown gesture type.");
                        
                }
                
            }
            return ("Unknown gesture type.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>A normalized coordinate to be multiplied with the actual width and height of the screen</returns>
        public float[] getCoordinate()
        {
            float[] coordinates = new float[2];
            Frame frame = _controller.Frame();
            if (!frame.Hands.IsEmpty)
            {
                // Get the first hand
                Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                FingerList fingers = hand.Fingers;
                if (!fingers.IsEmpty)
                {
                    // Calculate the hand's average finger tip position
                    Vector avgPos = Vector.Zero;
                    foreach (Finger finger in fingers)
                    {
                        avgPos += finger.TipPosition;
                    }
                    avgPos /= fingers.Count;

                    InteractionBox iBox = _controller.Frame().InteractionBox;
                    Vector normalizedPosition = iBox.NormalizePoint(avgPos);

                    coordinates[0] = normalizedPosition.x;
                    coordinates[1] = normalizedPosition.y;

                    return coordinates;
                }
                
            }
            return null;
        }
    }
}
