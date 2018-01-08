// Copyright © 2018 Shamim Keshani
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace BinanceApi_CSharp
{
    class BinanceApi
    {
        // the base URL
        public static readonly string binanceUrl = "https://www.binance.com/";

        static HttpWebRequest request;
        static HttpWebResponse response;

        // General Endpoints
        public static readonly string pingApiUrl = binanceUrl + "api/v1/ping";
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

        public static readonly string timeApiUrl = binanceUrl + "api/v1/time";
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

        // generate HMAC SHA265
        public static string ComputeHash_HMACSHA256(string secretKey, string message)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(secretKey);
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            string hashString;
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(messageBytes);
                hashString = Tools.Bytes2Hex(hashBytes).ToLower(); 
            }
            return hashString;
        }
    }
}
