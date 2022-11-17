using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageConverter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] argumentFiles = Environment.GetCommandLineArgs().Skip(1).ToArray();
            //string argumentFile = string.Join(Environment.NewLine, argumentFiles);
            //MessageBox.Show(argumentFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(argumentFiles));
        }
    }
}
