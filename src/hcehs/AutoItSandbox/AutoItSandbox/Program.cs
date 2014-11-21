using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoItX3Lib;

namespace AutoItSandbox
{
    class Program
    {
        
        static void Main(string[] args)
        {
            bool capture = false;
            bool procedurenote = false;

            AutoItX3 _autoit = new AutoItX3();
            //_autoit.Run("notepad.exe");

           // _autoit.WinWaitActive("ProVation MD");
           /*
            * Prototype to log into ProVation using AutoIt
            
            _autoit.WinActivate("ProVation MD");
            _autoit.MouseClick("left",490,23);
            _autoit.Send("backdoor");
            _autoit.Send("{TAB}");
            _autoit.Send("hoyLake");
            _autoit.Send("{ENTER}");

            */

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

            }

            
                    
        }
    }
}
