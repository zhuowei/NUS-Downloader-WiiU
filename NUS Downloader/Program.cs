///////////////////////////////////////////
// NUS Downloader: Program.cs            //
// $Rev::                              $ //
// $Author::                           $ //
// $Date::                             $ //
///////////////////////////////////////////


using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NUS_Downloader
{
    static class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Console.Title = "NUSD";

            // hide the console window                   
            //setConsoleWindowVisibility(false, Console.Title);
            
            if (args.Length != 0)
            {
                // hide the console window                   
                //setConsoleWindowVisibility(true, Console.Title);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1(args));
            }
            else
            {
                // hide the console window                   
                //setConsoleWindowVisibility(false, Console.Title);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }

        public static void setConsoleWindowVisibility(bool visible, string title)
        {
            //Sometimes System.Windows.Forms.Application.ExecutablePath works for the caption depending on the system you are running under.          
            IntPtr hWnd = FindWindow(null, title);

            if (hWnd != IntPtr.Zero)
            {
                if (!visible)
                    //Hide the window                   
                    ShowWindow(hWnd, 0); // 0 = SW_HIDE               
                else
                    //Show window again                   
                    ShowWindow(hWnd, 1); //1 = SW_SHOWNORMA          
            }
        }
    }
}
