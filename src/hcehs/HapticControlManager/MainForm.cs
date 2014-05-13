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
using HapCon.HapticService;
using HapCon.Common;
using HapCon.LeapMotion;
using System.IO;
using System.Xml;


namespace HapticControlManager
{
    
    public partial class mainForm : Form
    {
        ServiceHost host;
        private List<HapticDevice> _devices = new List<HapticDevice>();
        public struct HapticDevice
        {
            public string hapticName;
            public string computerName;
            public string listeningMode;
            public string ipAddress;
        }
        
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
            startButton.Enabled = false;
            stopButton.Enabled = true;
            addButton.Enabled = false;
            deleteButton.Enabled = false;

            ExportListViewlToXML(deviceListView, "", "configuration.xml");


            host = new ServiceHost(typeof(HapCon.HapticService.HapticService));
            host.Open();
            statusTextBox.Text = "Service started";
            statusTextBox.Update();

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
            addButton.Enabled = true;
            deleteButton.Enabled = true;
        }

        private void addButton_Click(object sender, EventArgs e)
        {

            AddForm addForm = new AddForm();
            addForm.ShowDialog();

            HapticDevice newDevice;
            newDevice.computerName = addForm.getWorkstationName;
            newDevice.hapticName = addForm.getHapticDevice;
            newDevice.listeningMode = addForm.getListeningMode;
            newDevice.ipAddress = addForm.getIpAddress;
            _devices.Add(newDevice);

            ListViewItem item = new ListViewItem(newDevice.hapticName);
            item.SubItems.Add(newDevice.computerName);
            item.SubItems.Add(newDevice.listeningMode);
            item.SubItems.Add(newDevice.ipAddress);
            deviceListView.Items.Add(item);


        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem eachItem in deviceListView.SelectedItems)
            {
                _devices.RemoveAt(eachItem.Index);
                deviceListView.Items.Remove(eachItem);
                
            }
        }


        public static void ExportListViewlToXML(ListView listview, String filePath, String fileName)
        {
            FileStream fileStream;
            StreamWriter streamWriter;
            XmlTextWriter xmlTextWriter;

            try
            {
                // overwrite even if it already exists
                fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

                streamWriter = new StreamWriter(fileStream);
                xmlTextWriter = new XmlTextWriter(streamWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();
                xmlTextWriter.WriteStartElement("HapticConfigurations");

                const int SUBITEM1_POS = 0;
                const int SUBITEM2_POS = 1;
                const int SUBITEM3_POS = 2;
                const int SUBITEM4_POS = 3;

                for (int i = 0; i < listview.Items.Count; i++)
                {
                    String currentSubItem1 = listview.Items[i].SubItems[SUBITEM1_POS].Text;
                    String currentSubItem2 = listview.Items[i].SubItems[SUBITEM2_POS].Text;
                    String currentSubItem3 = listview.Items[i].SubItems[SUBITEM3_POS].Text;
                    String currentSubItem4 = listview.Items[i].SubItems[SUBITEM4_POS].Text;

                    xmlTextWriter.WriteStartElement("HapticDevices");
                    xmlTextWriter.WriteAttributeString("hapticType", currentSubItem1.ToString());
                    xmlTextWriter.WriteAttributeString("workstationName", currentSubItem2.ToString());
                    xmlTextWriter.WriteAttributeString("listeningMode", currentSubItem3.ToString());
                    xmlTextWriter.WriteAttributeString("ipAddress", currentSubItem4.ToString());
                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndDocument();
                xmlTextWriter.Flush();
                xmlTextWriter.Close();

                //return true;
            }
            catch (IOException ex)
            {
                // do something about your error
                //return false;
            }
        }


 


    }
}
