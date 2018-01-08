// Copyright © 2018 Shamim Keshani
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
