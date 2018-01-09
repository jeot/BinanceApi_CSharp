// Copyright © 2018 Shamim Keshani
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BinanceApi_CSharp
{
    public class Tools
    {
        public static string Bytes2Hex(byte[] byteArray)
        {
            if (byteArray.Length == 0)
                return "";
            else
            {
                string hexStr = "";
                foreach (byte b in byteArray)
                {
                    hexStr += b.ToString("X2");
                }
                return hexStr;
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
