using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HapCon.Common;

namespace HapCon.LeapMotion
{
    public interface ILeapMotion : IHardware
    {
        string ConnectionString { get; set; }
    }
}
