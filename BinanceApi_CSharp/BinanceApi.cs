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
        public enum SymbolType { SPOT }
        public enum OrderStatus { NEW, PARTIALLY_FILLED, FILLED, CANCELED, PENDING_CANCEL, REJECTED, EXPIRED }
        public enum OrderType { LIMIT, MARKET }
        public enum OrderSide { BUY, SELL }
        public enum TimeInForse { GTC, IOC }
        public enum Interval { _1m, _3m, _5m, _15m, _30m, _1h, _2h, _4h, _6h, _8h, _12h, _1d, _3d, _1w, _1M }



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

        public static readonly string exchangeInfoUrl = binanceUrl + "/api/v1/exchangeInfo";
        public static string GetExchangeInfo()
        {
            request = (HttpWebRequest)WebRequest.Create(exchangeInfoUrl);
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

        public static readonly string klinesUrl = binanceUrl + "/api/v1/klines";
        public static List<MarketData> GetMarketDataList(string exchangeSymbols, Interval timeInterval, int limit = 500, long startTime = -1, long endtime = -1)
        {
            string query = string.Format("{0}?symbol={1}&interval={2}&limit={3}",
                                         klinesUrl, exchangeSymbols, timeInterval.ToString().Remove(0, 1), limit.ToString());
            if (startTime >= 0) query += "&startTime=" + startTime.ToString();
            if (endtime >= 0) query += "&endTime=" + endtime.ToString();
            try
            {
                using (StreamReader reader = new StreamReader(WebRequest.Create(query).GetResponse().GetResponseStream(), Encoding.ASCII))
                {
                    return MarketData.GetMarketDataArray(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed @GetMarketData. Exception: {0}", ex.Message);
                return null;
            }
        }

        public static readonly string priceUrl = binanceUrl + "/api/v3/ticker/price";
        public static double GetRealTimePrice(string exchangeSymbols)
        {
            string query = string.Format("{0}?symbol={1}", priceUrl, exchangeSymbols);
            try
            {
                using (StreamReader reader = new StreamReader(WebRequest.Create(query).GetResponse().GetResponseStream(), Encoding.ASCII))
                {
                    string jsonString = reader.ReadToEnd();
                    if (JObject.Parse(jsonString)["symbol"].ToString() == exchangeSymbols)
                        return double.Parse(JObject.Parse(jsonString)["price"].ToString());
                    else
                    {
                        Console.WriteLine("The returned symbol didn't match the requested one!");
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                return -1;
            }
        }
    }
}
