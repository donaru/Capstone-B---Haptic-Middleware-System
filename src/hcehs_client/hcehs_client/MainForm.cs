using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeapClient;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace hcehs_client
{
    public partial class MainForm : Form
    {
        HapticService.HapticServiceClient client = new HapticService.HapticServiceClient("NetTcpBinding_IHapticService");
        private Thread _task;
        public MainForm()
        {
            InitializeComponent();
            workstationNameTextBox.Text = "test";
        }

        private void sendButton_Click(object sender, EventArgs e)
        {


        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {

                statusTextBox.Text = client.GetMessage(workstationNameTextBox.Text);
                if (statusTextBox.Text == "connected")
                {
                    _task = new Thread(run);
                    _task.Start();
                }

            }
            catch (Exception ex)
            {
                statusTextBox.Text = "";
            }
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void run()
        {
            
            try
            {
                TcpClient tcpclnt = new TcpClient();
                tcpclnt.Connect("127.0.0.1", 8001);
                NetworkStream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                float[] coordinates = new float[2];

                Socket s = tcpclnt.Client;
                



                while (true)
                {
                    string message = "";
                    byte[] bb = new byte[100];
                    int k = s.Receive(bb);
                    
                    //Console.WriteLine("Size of stream = " + k);

                    for (int i = 0; i < k; i++)
                    {
                        if (Convert.ToChar(bb[i]) == '!')
                        {
                            break;
                        }
                        message = message + Convert.ToChar(bb[i]);

                    }
                    //Console.WriteLine( message);

                    var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                    var width = screen.Width;
                    var height = screen.Height;

                    string [] split = message.Split(new Char [] {','});
                    try
                    {
                        coordinates[0] = float.Parse(split[0],System.Globalization.CultureInfo.InvariantCulture);
                        coordinates[1] = float.Parse(split[1], System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch (Exception axe)
                    {
                        //Console.WriteLine("Error:" + axe);
                    }
                    
                    float x = coordinates[0] * width;
                    float y = coordinates[1] * height;
                    int xCoordinate = (int)Math.Round(x);
                    int yCoordinate = (int)Math.Round(y);
                    
                    if (coordinates[0] >= 0 && coordinates[0] <= 1 )
                    {
                        if (coordinates[1] >= 0 && coordinates[1] <= 1 )
                        {
                            //Cursor.Position = new Point(xCoordinate, height - yCoordinate);
                            Console.WriteLine(xCoordinate+ "," + (height - yCoordinate));
                        }

                    }

                }
                tcpclnt.Close();
            }

            catch (Exception e)
            {
                //Console.WriteLine("Error:" + e.StackTrace);
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                _task.Abort();
            }
            catch (Exception)
            {
                Console.WriteLine("catched abort exception");
            }
            statusTextBox.Text = "disconnected";
        }
    }
}
