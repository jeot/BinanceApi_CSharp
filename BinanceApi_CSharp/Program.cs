using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;

namespace BinanceApi_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            BinanceApi.TestConnectivity();
            BinanceApi.GetServerTime();

            exit:
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
