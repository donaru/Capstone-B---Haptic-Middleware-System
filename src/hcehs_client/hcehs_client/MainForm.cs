using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hcehs_client
{
    public partial class MainForm : Form
    {
        HapticService.HapticServiceClient client = new HapticService.HapticServiceClient("NetTcpBinding_IHapticService");
        public MainForm()
        {
            InitializeComponent();
            workstationNameTextBox.Text = "test";
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {
                //HapticService.HapticServiceClient client = new HapticService.HapticServiceClient("NetTcpBinding_IHapticService");
                statusTextBox.Text = client.GetMessage("World!");
            }
            catch (Exception ex)
            {
                statusTextBox.Text = "";
            }

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
               // HapticService.HapticServiceClient client = new HapticService.HapticServiceClient("NetTcpBinding_IHapticService");


                    string coordinates; 
                    try
                    {
                        coordinates = client.GetCoordinate(workstationNameTextBox.Text);

                        /*
                        //float[] coordinates = client.GetCoordinate(workstationNameTextBox.Text);
                        var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                        var width = screen.Width;
                        var height = screen.Height;
                        float x = coordinates[0] * width;
                        float y = coordinates[1] * height;
                        int xCoordinate = (int)Math.Round(x);
                        int yCoordinate = (int)Math.Round(y);*/
                        statusTextBox.Text = coordinates;
                        //Cursor.Position = new Point(xCoordinate, height - yCoordinate);


                    }
                    catch (Exception err)
                    {
                        statusTextBox.Text = "Error";
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
    }
}
