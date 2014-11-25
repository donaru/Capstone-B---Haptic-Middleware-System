using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;
using HapCon.Haptics;
using HapCon.Common;
using System.Threading;

namespace AutoItSandbox
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Haptics controller = new Haptics();
            bool capture = false;
            bool procedurenote = false;

            AutoItX3 _autoit = new AutoItX3();
            //_autoit.Run("notepad.exe");

           // _autoit.WinWaitActive("ProVation MD");
           /*
            * Prototype to log into ProVation using AutoIt
            */
            //initialise framework
            controller.Initialise();
            CommonGestures x = CommonGestures.Unknown;
            while(true)
            {
                x = controller.getGesture();
                if (x != CommonGestures.Unknown)
                    Console.WriteLine(x);
                Console.WriteLine(controller.getDistance());
                if (x == CommonGestures.SwipeRight)
                    break;
            }
            // PROVATION LOGIN
            _autoit.WinActivate("ProVation MD");
            //_autoit.MouseClick("left",490,23);
            _autoit.MouseClick("left", 1394, 641);
            _autoit.Send("backdoor");
            _autoit.Send("{TAB}");
            _autoit.Send("hoyLake");
            _autoit.Send("{ENTER}");
            while (true)
            {
                x = controller.getGesture();
                if (x != CommonGestures.Unknown)
                    Console.WriteLine(x);
                Console.WriteLine(controller.getDistance());
                if (x == CommonGestures.SwipeRight)
                    break;
            }
            // GI Outlook Bar
            _autoit.MouseClick("left", 1449, 347);
            // Procedure documentation
            _autoit.MouseClick("left", 1449, -146);

            // Click into procedure documentation
            _autoit.MouseClick("left", 1388, -61);
            CommonGestures old = x;
           
            while (true)
            {
                x = controller.getGesture();
                if (x != old)
                {
                    if (x != CommonGestures.Unknown)
                        Console.WriteLine(x);
                    Console.WriteLine(controller.getDistance());
                    if (x == CommonGestures.SwipeRight)
                        _autoit.Send("{UP}");
                    if (x == CommonGestures.SwipeLeft)
                        _autoit.Send("{DOWN}");
                    if (x == CommonGestures.Okay)
                        _autoit.Send("{ENTER}");
                    if (x == CommonGestures.CircleClockwise)
                    {
                        // Click on Image Capture
                        _autoit.MouseClick("left", 1840, -253);
                        break;
                    }

                       
                }
                
                old = x;
            }



            // Removing the error
            _autoit.Send("{ENTER}");

            old = x;
            while (true)
            {
                x = controller.getGesture();
                if (x != old)
                {
                    if (x != CommonGestures.Unknown)
                        Console.WriteLine(x);
                    Console.WriteLine(controller.getDistance());
                    if (x == CommonGestures.SwipeRight)
                    {
                        _autoit.MouseClick("left", 1499, -251);
                        break;
                    }
                        
                    if (x == CommonGestures.Okay)
                        _autoit.Send("P");

                }
                old = x;
            }

            while (true)
            {
                x = controller.getGesture();
                if (x != old)
                {
                    if (x != CommonGestures.Unknown)
                        Console.WriteLine(x);
                    Console.WriteLine(controller.getDistance());
                    if (x == CommonGestures.SwipeRight)
                        _autoit.Send("{UP}");
                    if (x == CommonGestures.SwipeLeft)
                        _autoit.Send("{DOWN}");
                    if (x == CommonGestures.Okay)
                        _autoit.Send("{ENTER}");
                    if (x == CommonGestures.CircleClockwise)
                        _autoit.Send("{LEFT}");


                }

                old = x;
            }

            /* 
             * Below Prototype is to capture within the Image Capture Screen then log back into the procedure note
             */

            
            /*
            _autoit.Send("p");
            _autoit.Send("p");
            _autoit.Send("p");
            _autoit.Send("p");
            _autoit.MouseClick("left", 125, 60);*/
           

            //Console.WriteLine("recieving input");
            //_autoit.

            /*
            _autoit.WinActivate("Image Capture:");
            while(true)
            {

                if ((_autoit.WinActive("Image Capture:") == 1) && !capture)
                {
                    _autoit.WinActivate("Image Capture:");
                    _autoit.Send("{ENTER}");
                    _autoit.MouseClick("left", 180, 33);
                    // input = Console.ReadLine();
   
                    _autoit.Send("p");  // capture the image

                    _autoit.MouseClick("left", 125, 60); // Edit procedure note

                    //capture = true;
                    
                }
             * 
             * */
                /*
                if ((_autoit.WinActive("ProVation MD") == 1) && !procedurenote)
                {
                    for (int i = 0; i < 5;i++ )
                        _autoit.Send("{ENTER}");

                    for (int i = 0; i < 10; i++)
                        _autoit.Send("{UP}");

                    for (int i = 0; i < 7; i++)
                        _autoit.Send("{ENTER}");

                    _autoit.Send("{DOWN}");

                    for (int i = 0; i < 4; i++)
                        _autoit.Send("{ENTER}");

                    _autoit.Send("{DOWN}");

                    for (int i = 0; i < 3; i++)
                        _autoit.Send("{ENTER}");

                    // 170, 60 [Print] button

                    procedurenote = true;
                }*/

          //  }

            
                    
        }
    }
}
