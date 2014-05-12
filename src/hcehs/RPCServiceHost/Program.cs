using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace RPCServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(HapticService.HapticService)))
            {
                host.Open();
                Console.WriteLine("Service started at " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
    }
}
