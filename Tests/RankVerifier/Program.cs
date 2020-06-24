using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RankVerifier
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string Version = "1";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 f = new Form1();
            f.Version = Version;
            
            Application.Run(f);
        }
    }
}
