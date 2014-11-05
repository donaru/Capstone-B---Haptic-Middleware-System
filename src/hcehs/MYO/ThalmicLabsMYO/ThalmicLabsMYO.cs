using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;
using MyoSharp.Device;
using MyoSharp.Poses;

namespace HapCon.ThalmicLabsMYO
{
    public class ThalmicLabsMYO : IThalmicLabsMYO
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string SetComputerName { get; set; }
        public ListeningMode GetListeningMode { get; set; }

        public ThalmicLabsMYO()
        {


        }
        public void Initialise()
        { }
        public void Shutdown()
        { }

        public void SetParameters(string name, string computerName, ListeningMode listeningMode)
        { }

        public CommonGestures getGesture()
        {
            /*
             *          SwipeLeft,
                        SwipeRight,
                        CircleClockwise,
                        CircleAntiClockwise,
                        Okay,
                        Unknown
             * */
            return CommonGestures.SwipeLeft;
        }
        public float[] getCoordinate()
        {
            float[] coordinate = new float[3];
            return coordinate;
        }
    }
}
