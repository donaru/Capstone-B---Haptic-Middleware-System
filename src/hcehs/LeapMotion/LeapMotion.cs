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
        public LeapMotion()
        {

        }
        public string ConnectionString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Initialise()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void SetComputerName(string name)
        {
            throw new NotImplementedException();
        }

        public string GetListeningMode()
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
