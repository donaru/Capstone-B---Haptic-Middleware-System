using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using HapticService;
using HapCon.Common;
using HapCon.LeapMotion;


namespace HapticControlManager
{
    
    public partial class mainForm : Form
    {
        ServiceHost host; 
        public mainForm()
        {
            InitializeComponent();
            stopButton.Enabled = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            host = new ServiceHost(typeof(HapticService.HapticService));
            host.Open();
            statusTextBox.Text = "Service started";
            statusTextBox.Update();
            startButton.Enabled = false;
            stopButton.Enabled = true;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            host.Close();
            statusTextBox.Text = "Service stopped";
            statusTextBox.Update();
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm();
            addForm.Show();
        }

 


    }
}
