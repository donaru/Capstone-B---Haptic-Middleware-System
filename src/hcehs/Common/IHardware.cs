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
        void SetComputerName(string name);
        string GetListeningMode();
        string getGesture();
        int getXCoordinate();
        int getYCoordinate();
    }
}

