// Copyright © 2018 Shamim Keshani
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

            // test HMAC SHA256 conversion (examples are from Binance.com API documentations):
            string totalParams = "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000&timestamp=1499827319559";
            string apiKey = "vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A";
            string secretKey = "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j";
            // this should return signature = c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71
            string signature = Tools.ComputeHash_HMACSHA256(secretKey, totalParams);
            Console.WriteLine("the calculated signature is: {0}", signature);

        exit:
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
