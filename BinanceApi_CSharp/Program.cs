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
            string url = "https://www.binance.com/api/v1/time";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            } catch (Exception ex) {
                Console.WriteLine("Exception occured: {0}", ex.Message);
                goto exit;
            }
            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                Console.WriteLine("Response from server:\n{0}", responseText);
            }

            exit:
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
