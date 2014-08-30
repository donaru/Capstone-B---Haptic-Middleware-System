using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HapCon.Common
{
    public enum ListeningMode
    {
        TcpConnection,
        HttpConnection,
        UsbConnection
    }

    
    public enum CommonGestures
    {
        SwipeLeft,
        SwipeRight,
        CircleClockwise,
        CircleAntiClockwise,
        Okay,
        Unknown
    }
     


    public interface IHardware
    {
        string Name { get; set; }
        void Initialise();
        void Shutdown();

        void SetParameters(string name, string computerName, ListeningMode listeningMode);
        string SetComputerName { get; set; }
        ListeningMode GetListeningMode { get; set; }
        CommonGestures getGesture();
        float[] getCoordinate();
        
    }
}

