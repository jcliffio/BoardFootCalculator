using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BoardFootCalculator
{
    public static class AppSettingsHelper
    {
        public static List<double> GetStockLengths()
        {
            var stockLengthsString = ConfigurationManager.AppSettings["StockLengths"];
            return stockLengthsString.Split(',').Select(double.Parse).ToList();
        }
    }
}
