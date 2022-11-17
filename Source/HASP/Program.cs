using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HASP
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    bool isLicensed = HaspHelper.Instance().CheckLicense();
                    Console.Write($"Licensed : {isLicensed}");
                    if (isLicensed)
                    {
                        int fCode = HaspHelper.Instance().GetFeatureCode();
                        Console.Write($", Code: {fCode}");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.GetType().Name} : {ex.Message}");
                }
            }
        }
    }
}
