using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;

namespace HapCon.LeapMotion
{
    public class LeapMotion : ILeapMotion
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public string GetListeningMode { get; set; }
        public LeapMotion()
        {

        }

        public void SetParameters(string name, string computerName, string listeningMode)
        {
            this.Name = name;
            throw new NotImplementedException();
        }
        public void Initialise()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public string getGesture()
        {
            throw new NotImplementedException();
        }

        public int getXCoordinate()
        {
            throw new NotImplementedException();
        }

        public int getYCoordinate()
        {
            throw new NotImplementedException();
        }
    }
}
