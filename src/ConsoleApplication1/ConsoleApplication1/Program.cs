using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var xdoc = XDocument.Load("XMLFile1.xml");
                var entries = from e in xdoc.Descendants("HapticDevices")
                          select new
                          {
                              HapticType = (string)e.Attribute("hapticType"),
                              WorkstationName = (string)e.Attribute("workstationName"), 
                              ListeningMode = (string)e.Attribute("listeningMode"),
                              IpAddress  = (string)e.Attribute("ipAddress")
                          };
                Console.WriteLine(entries.ElementAt(0));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error" + e);
            }
            
            
            Console.WriteLine("\n---XML parsed---");
            Console.ReadKey();



        }


    }
}
