﻿// Copyright © 2018 Shamim Keshani
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
            // test connectivity and get server time
            BinanceApi.TestConnectivity();
            BinanceApi.GetServerTime();

            // test HMAC SHA256 conversion (examples are from Binance.com API documentations):
            string totalParams = "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000&timestamp=1499827319559";
            string apiKey = "vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A";
            string secretKey = "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j";
            // this should return signature = c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71
            string signature = Tools.ComputeHash_HMACSHA256(secretKey, totalParams);
            Console.WriteLine("the calculated signature is: {0}", signature);

            // get order book
            //BinanceApi.GetOrderBook("ETHBTC", 5);
            //BinanceApi.GetOrderBook("ADAETH");
            //BinanceApi.GetOrderBook("ETHUSDT", 500);

            // get exchange info
            //BinanceApi.GetExchangeInfo();

            // get market data (candlestick data)
            //var eachDayMarketDataList_ETHUSDT = BinanceApi.GetMarketDataList("ETHUSDT", BinanceApi.Interval._1d, 10);
            //foreach (var d in eachDayMarketDataList_ETHUSDT)
            //    Console.WriteLine(d.ToString());

			// write to csv file
			//var each5MinMarketDataList_ADAETH = BinanceApi.GetMarketDataList("ADAETH", BinanceApi.Interval._5m);
			//MarketData.MarketDataListToCsvFile(each5MinMarketDataList_ADAETH, "each5MinMarketDataList.csv");
			var each1MinMarketDataList_ETHUSDT = BinanceApi.GetMarketDataList("ETHUSDT", BinanceApi.Interval._1m);
			MarketData.MarketDataListToCsvFile(each1MinMarketDataList_ETHUSDT, "each1MinMarketDataList.csv");

            exit:
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
