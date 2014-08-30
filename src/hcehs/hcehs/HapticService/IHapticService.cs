using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HapCon.HapticService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHapticService" in both code and config file together.
    [ServiceContract]
    public interface IHapticService
    {
        [OperationContract]
        string GetMessage(string name);


        [OperationContract]
        void LoadConfigurations();
       
    }
}
