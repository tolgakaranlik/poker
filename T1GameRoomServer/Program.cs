using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T1GameRoomServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string Version = "0.1.31";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormMain f = new FormMain();
            for (int i = 0; i < args.Length; i++)
            {
                if(args[i].StartsWith("-casinoid="))
                {
                    try
                    {
                        f.CasinoId = int.Parse(args[i].Substring("-casinoid=".Length));
                    } catch
                    {
                        f.CasinoId = 1;
                    }
                } else if(args[i].StartsWith("-host="))
                {
                    f.Host = args[i].Substring("-host=".Length);
                }
                else if (args[i] == "-testmode")
                {
                    f.TestMode = true;
                }
            }

            f.Version = Version;

            Application.Run(f);
        }
    }
}
