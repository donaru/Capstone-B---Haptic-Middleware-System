using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HapticControlManager
{
    public partial class AddForm : Form
    {
        private string _workstationName;
        private string _hapticDevice;
        private string _listeningMode;
        private string _ipAddress;

        public AddForm()
        {
            InitializeComponent();
            
        }

        private void hapticDeviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            if (box.SelectedIndex == 0)
            {
                pictureBox.ImageLocation = "leap_motion_logo.jpg";
                _hapticDevice = "leapmotion";
                serialRadioButton.Select();
                tcpRadioButton.Enabled = false;
                ipAddressTextBox.Enabled = false;
                ipAddressTextBox.Text = "N/A";
                
            }
            if (box.SelectedIndex == 1)
            {
                pictureBox.ImageLocation = "kinect_logo.jpg";
                _hapticDevice = "kinect";
                serialRadioButton.Enabled = true;
                tcpRadioButton.Enabled = false;
                ipAddressTextBox.Enabled = false;
                ipAddressTextBox.Text = "N/A";
            }
/*
            if (box.SelectedIndex == 1)
            {
                pictureBox.ImageLocation = "softkinetic.jpg";
                _hapticDevice = "softkinetic";
                serialRadioButton.Select();
                tcpRadioButton.Enabled = false;
                ipAddressTextBox.Enabled = false;
                ipAddressTextBox.Text = "N/A";
                
            }*/
            if (box.SelectedIndex == 2)
            {
                pictureBox.ImageLocation = "mesa_imaging.jpg";
                _hapticDevice = "mesa";
                serialRadioButton.Enabled = true;
                tcpRadioButton.Enabled = true;
                ipAddressTextBox.Enabled = true;
            }


        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            _workstationName = workstationTextBox.Text;
            _ipAddress = ipAddressTextBox.Text;
            if (tcpRadioButton.Checked)
            {
                _listeningMode = "tcp";
            }
            if (serialRadioButton.Checked)
            {
                _listeningMode = "serial";
            }                

            this.Close();
        }


        public string getWorkstationName
        {
           get {return _workstationName;}
        }

        public string getHapticDevice
        {
            get { return _hapticDevice; }
        }

        public string getListeningMode
        {
            get { return _listeningMode; }
        }

        public string getIpAddress
        {
            get { return _ipAddress; }
        }
        








    }
}
