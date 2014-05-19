using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leap;

namespace mouseSample
{
    public partial class SimpleMouseController : Form
    {
        SampleListener listener = new SampleListener();
        Controller controller = new Controller();
        Boolean mouseOverride = false;
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        public SimpleMouseController()
        {
            InitializeComponent();
            //textBox1.Text = "device connected";
            backgroundWorker1.DoWork +=  new DoWorkEventHandler(actualWork);
            //mandatory. Otherwise will throw an exception when calling ReportProgress method  
            backgroundWorker1.WorkerReportsProgress = true;

            //mandatory. Otherwise we would get an InvalidOperationException when trying to cancel the operation  
            backgroundWorker1.WorkerSupportsCancellation = true;  
            // Have the sample listener receive events from the controller
             
            
           // Cursor.Position = new Point(
            //            int rxx = int.Parse(textBox_Status.Text);
            //Cursor.Position = new Point(Cursor.Position.X + rxx, 10);
        }

        private void actualWork(object sender, DoWorkEventArgs e)
        {
            mouseOverride = true;
            controller.AddListener(listener);

            do
            {
                try
                {
                    Frame frame = controller.Frame();
                    if (!frame.Hands.IsEmpty)
                    {
                        // Get the first hand
                        handDetect(frame);
                        gestureDetect(frame);

                        if (this.backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                catch (AccessViolationException)
                {

                }



                
            } while (mouseOverride != false);  
         
        }

        private void handDetect(Frame frame)
        {
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

                InteractionBox iBox = controller.Frame().InteractionBox;
                Vector normalizedPosition = iBox.NormalizePoint(avgPos);
                var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                var width = screen.Width;
                var height = screen.Height;
                float x = normalizedPosition.x * width;
                float y = normalizedPosition.y * height;


                int xCoordinate = (int)Math.Round(x);
                int yCoordinate = (int)Math.Round(y);

                Cursor.Position = new Point(xCoordinate, height - yCoordinate);
            }
        }
        private void gestureDetect(Frame frame)
        {
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
                            CircleGesture previousUpdate = new CircleGesture(controller.Frame(1).Gesture(circle.Id));
                            sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                        }

                        Console.WriteLine("Circle id: " + circle.Id
                                       + ", " + circle.State
                                       + ", progress: " + circle.Progress
                                       + ", radius: " + circle.Radius
                                       + ", angle: " + sweptAngle
                                       + ", " + clockwiseness);
                        break;
                    case Gesture.GestureType.TYPESWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        Console.WriteLine("Swipe id: " + swipe.Id
                                       + ", " + swipe.State
                                       + ", position: " + swipe.Position
                                       + ", direction: " + swipe.Direction
                                       + ", speed: " + swipe.Speed);
                        break;
                    case Gesture.GestureType.TYPEKEYTAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        Console.WriteLine("Tap id: " + keytap.Id
                                       + ", " + keytap.State
                                       + ", position: " + keytap.Position
                                       + ", direction: " + keytap.Direction);

                        // ----------- stops on key tap
                    if (backgroundWorker1.WorkerSupportsCancellation == true)
                    {
                        // Cancel the asynchronous operation.
                        //textBox_Status.Text = "Stopped";
                        //mouseOverride = false;
                        backgroundWorker1.CancelAsync();
                    }

                        //---------------
                        

                        break;
                    case Gesture.GestureType.TYPESCREENTAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        Console.WriteLine("Tap id: " + screentap.Id
                                       + ", " + screentap.State
                                       + ", position: " + screentap.Position
                                       + ", direction: " + screentap.Direction);
                        break;
                    default:
                        Console.WriteLine("Unknown gesture type.");
                        break;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox_Status.Text = "Started";
            
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_Stop_Click(object sender, EventArgs e)
        {

            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
            textBox_Status.Text = "Stopped";
            mouseOverride = false;
        }

        private void button_Quit_Click(object sender, EventArgs e)
        {
            // Remove the sample listener when done
            controller.RemoveListener(listener);
            controller.Dispose();
            Application.Exit();
        }




    }

    class SampleListener : Listener
    {
        private Object thisLock = new Object();

        private void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
                
            }
        }


        public override void OnInit(Controller controller)
        {
            SafeWriteLine("Initialized");
        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Connected");
            controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            SafeWriteLine("Disconnected");
        }
         
        public override void OnExit(Controller controller)
        {
            SafeWriteLine("Exited");
        }

        public override void OnFrame(Controller controller)
        {
            /*// Get the most recent frame and report some basic information
            Frame frame = controller.Frame();

            SafeWriteLine("Frame id: " + frame.Id
                        + ", timestamp: " + frame.Timestamp
                        + ", hands: " + frame.Hands.Count
                        + ", fingers: " + frame.Fingers.Count
                        + ", tools: " + frame.Tools.Count
                        + ", gestures: " + frame.Gestures().Count);

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
                    SafeWriteLine("Hand has " + fingers.Count
                                + " fingers, average finger tip position: " + avgPos);

                }

                // Get the hand's sphere radius and palm position
                SafeWriteLine("Hand sphere radius: " + hand.SphereRadius.ToString("n2")
                            + " mm, palm position: " + hand.PalmPosition);

                // Get the hand's normal vector and direction
                Vector normal = hand.PalmNormal;
                Vector direction = hand.Direction;

                // Calculate the hand's pitch, roll, and yaw angles
                SafeWriteLine("Hand pitch: " + direction.Pitch * 180.0f / (float)Math.PI + " degrees, "
                            + "roll: " + normal.Roll * 180.0f / (float)Math.PI + " degrees, "
                            + "yaw: " + direction.Yaw * 180.0f / (float)Math.PI + " degrees");
            }

            // Get gestures
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
                            CircleGesture previousUpdate = new CircleGesture(controller.Frame(1).Gesture(circle.Id));
                            sweptAngle = (circle.Progress - previousUpdate.Progress) * 360;
                        }

                        SafeWriteLine("Circle id: " + circle.Id
                                       + ", " + circle.State
                                       + ", progress: " + circle.Progress
                                       + ", radius: " + circle.Radius
                                       + ", angle: " + sweptAngle
                                       + ", " + clockwiseness);
                        break;
                    case Gesture.GestureType.TYPESWIPE:
                        SwipeGesture swipe = new SwipeGesture(gesture);
                        SafeWriteLine("Swipe id: " + swipe.Id
                                       + ", " + swipe.State
                                       + ", position: " + swipe.Position
                                       + ", direction: " + swipe.Direction
                                       + ", speed: " + swipe.Speed);
                        break;
                    case Gesture.GestureType.TYPEKEYTAP:
                        KeyTapGesture keytap = new KeyTapGesture(gesture);
                        SafeWriteLine("Tap id: " + keytap.Id
                                       + ", " + keytap.State
                                       + ", position: " + keytap.Position
                                       + ", direction: " + keytap.Direction);
                        break;
                    case Gesture.GestureType.TYPESCREENTAP:
                        ScreenTapGesture screentap = new ScreenTapGesture(gesture);
                        SafeWriteLine("Tap id: " + screentap.Id
                                       + ", " + screentap.State
                                       + ", position: " + screentap.Position
                                       + ", direction: " + screentap.Direction);
                        break;
                    default:
                        SafeWriteLine("Unknown gesture type.");
                        break;
                }
            }

            if (!frame.Hands.IsEmpty || !frame.Gestures().IsEmpty)
            {
                SafeWriteLine("");
            }
             * */
        }
    }
}
