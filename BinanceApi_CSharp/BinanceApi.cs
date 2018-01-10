// Copyright © 2018 Shamim Keshani
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

namespace BinanceApi_CSharp
{
    class BinanceApi
    {
        // the base URL
        public static readonly string binanceUrl = "https://api.binance.com";

        // enumerations
        enum SymbolType { SPOT }
        enum OrderStatus { NEW, PARTIALLY_FILLED, FILLED, CANCELED, PENDING_CANCEL, REJECTED, EXPIRED }
        enum OrderType { LIMIT, MARKET }
        enum OrderSide { BUY, SELL }
        enum TimeInForse { GTC, IOC }
        enum Interval { _1m, _3m, _5m, _15m, _30m, _1h, _2h, _4h, _6h, _8h, _12h, _1d, _3d, _1w, _1M }



        static HttpWebRequest request;
        static HttpWebResponse response;

        // General Endpoints
        public static readonly string pingApiUrl = binanceUrl + "/api/v1/ping";
        public static bool TestConnectivity()
        {
            request = (HttpWebRequest)WebRequest.Create(pingApiUrl);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    Console.WriteLine("Response from server:\n{0}", responseText);
                    if (responseText == "{}")
                    {
                        Console.WriteLine("Connectivity OK.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Connectivity failed. The server respond is not equal to {}.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connectivity test failed. Exception occured: {0}", ex.Message);
                return false;
            }
        }

        public static readonly string timeApiUrl = binanceUrl + "/api/v1/time";
        public static long GetServerTime()
        {
            request = (HttpWebRequest)WebRequest.Create(timeApiUrl);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    Console.WriteLine("Response from server:\n{0}", responseText);
                    dynamic data = JObject.Parse(responseText);
                    Console.WriteLine("return value: {0}", (long)data.serverTime);
                    return (long)data.serverTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting time from server failed. Exception occured: {0}", ex.Message);
                Console.WriteLine("return value: -1");
                return -1;
            }
        }

        public static readonly string orderBookUrl = binanceUrl + "/api/v1/depth";
        public static void GetOrderBook(string exchangeSymbols, int limit = 100)
        {
            string query = string.Format("{0}?symbol={1}", orderBookUrl, exchangeSymbols);
            query += "&limit=" + limit.ToString();

            request = (HttpWebRequest)WebRequest.Create(query);
            Console.WriteLine("Request Query: {0}", query);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    Console.WriteLine("Response from server:\n{0}", responseText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting order book from server failed. Exception occured: {0}", ex.Message);
            }
        }

        public static readonly string exchangeInfo = binanceUrl + "/api/v1/exchangeInfo";
        public static string GetExchangeInfo()
        {
            request = (HttpWebRequest)WebRequest.Create(exchangeInfo);
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    Console.WriteLine("Response from server:\n{0}", responseText);
                    return responseText;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Getting exchange info failed. Exception occured: {0}", ex.Message);
                return "";
            }
        }
    }
}
