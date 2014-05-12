using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace HapticService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HapticService" in both code and config file together.
    public class HapticService : IHapticService
    {

        public string GetMessage(string name)
        {
            //test data


            return "Hello " + name;
        }
    }
}
