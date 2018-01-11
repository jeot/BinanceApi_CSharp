using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinanceApi_CSharp
{
	public class MarketData
	{
		// member variables
		public long OpenTime = 0;
		public double Open = 0;
		public double High = 0;
		public double Low = 0;
		public double Close = 0;
		public double Volume = 0;
		public long CloseTime = 0;
		public double QuoteAssetVolume = 0;
		public long NumberOfTrades = 0;
		public double TakerBuyBaseAssetVolume = 0;
		public double TakerBuyQuoteAssetVolume = 0;
		public double Ignore = 0;

		public MarketData() { }

		public override string ToString()
		{
			return string.Format("OpenTime: {3}, High: {0}, Low: {1}, Average: {2}", High, Low, (High+Low)/2, OpenTime);
		}

		private string ToCsvString()
		{
			return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
				OpenTime, Open, High, Low, Close, Volume, CloseTime, QuoteAssetVolume, NumberOfTrades, TakerBuyBaseAssetVolume, TakerBuyQuoteAssetVolume, Ignore);
		}

		public static List<MarketData> GetMarketDataArray(string jsonArrayString) 
		{
			try
			{
				JArray jArray = JArray.Parse(jsonArrayString);
				int count = jArray.Count;
				var marketDataList = new List<MarketData>();
				foreach (var eachData in jArray)
				{
					JArray eachDataArray = (JArray)eachData;
					var newMarketData = new MarketData();
					newMarketData.OpenTime = eachDataArray[0].ToObject<long>();
					newMarketData.Open = eachDataArray[1].ToObject<double>();
					newMarketData.High = eachDataArray[2].ToObject<double>();
					newMarketData.Low = eachDataArray[3].ToObject <double>();
					newMarketData.Close = eachDataArray[4].ToObject <double>();
					newMarketData.Volume = eachDataArray[5].ToObject <double>();
					newMarketData.CloseTime = eachDataArray[6].ToObject <long>();
					newMarketData.QuoteAssetVolume = eachDataArray[7].ToObject <double>();
					newMarketData.NumberOfTrades = eachDataArray[8].ToObject <long>();
					newMarketData.TakerBuyBaseAssetVolume = eachDataArray[9].ToObject <double>();
					newMarketData.TakerBuyQuoteAssetVolume = eachDataArray[10].ToObject <double>();
					newMarketData.Ignore = eachDataArray[11].ToObject <double>();
					marketDataList.Add(newMarketData);
				}
				return marketDataList;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Can not parse json array string into JArray! Exception message: {0}", ex.Message);
				return null;
			}
		}
		
		static string MarketDataListToCsvString(List<MarketData> marketDataList)
		{
			string text = "";
			foreach (MarketData md in marketDataList) {
				text += md.ToCsvString();
				text += "\r\n";
			}
			return text;
		}

		public static void MarketDataListToCsvFile(List<MarketData> marketDataList, string filePath) {
			File.WriteAllText(filePath, MarketDataListToCsvString(marketDataList));
		}

		
	}
}
