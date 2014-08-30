using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using HapCon.Common;
using HapCon.LeapMotion;
using HapCon.LeapService;
using System.Threading;
using System.IO;

namespace HapCon.HapticService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HapticService" in both code and config file together.
    public class HapticService : IHapticService
    {
        private LinkedList<HapticDevice> _devices = new LinkedList<HapticDevice>();


        private struct HapticDevice
        {
            public string hapticType;
            public string workstationName;
            public string listeningMode;
            public string ipAddress;
        }
        public string GetMessage(string name)
        {
            LoadConfigurations();

            string data = "Not connected";
            bool result = LoadWorkstation(name);
            if (result)
            {
                data = "connected";
            }
            
            return data;
        }




        private bool LoadWorkstation(string workstationName)
        {
            for (int i = 0; i < _devices.Count;i++)
            {
                if (_devices.ElementAt(i).workstationName == workstationName)
                {
                    LeapService.LeapService service = new LeapService.LeapService();
                    var task = new Thread(service.run);
                    task.Start();
                    Console.WriteLine("Workstation found!!");
                    return true;
 
                }
            }
            return false;

        }
 
        public void LoadConfigurations() 
        {
            try
            {
                var xdoc = XDocument.Load("configuration.xml");
                var entries = from e in xdoc.Descendants("HapticDevices")
                              select new
                              {
                                  HapticType = (string)e.Attribute("hapticType"),
                                  WorkstationName = (string)e.Attribute("workstationName"),
                                  ListeningMode = (string)e.Attribute("listeningMode"),
                                  IpAddress = (string)e.Attribute("ipAddress")
                              };

                for (int i = 0; i < entries.Count(); i++ )
                {
                    HapticDevice device = new HapticDevice();
                    device.hapticType = entries.ElementAt(i).HapticType;
                    device.workstationName = entries.ElementAt(i).WorkstationName;
                    device.listeningMode = entries.ElementAt(i).ListeningMode;
                    device.ipAddress = entries.ElementAt(i).IpAddress;
                    _devices.AddLast(device);
                }
                    
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in loading configurations: " + e);
            }
        }


    }
}
