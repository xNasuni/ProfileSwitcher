using System;
using System.Windows.Forms;

namespace ProfileSwitcher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // ??????? this is useless but hi now that you are here dm me on discord Miaa#6983 if i changed it by then get it with my user id 515356922585677834
        }
    }
}
