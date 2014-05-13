using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapCon.Common
{

    public interface IHardware
    {
        string Name { get; set; }
        void Initialise();
        void Shutdown();

        void SetParameters(string name, string computerName, string listeningMode);
        string SetComputerName { get; set; }
        string GetListeningMode { get; set; }
        string getGesture();
        int getXCoordinate();
        int getYCoordinate();
    }
}

