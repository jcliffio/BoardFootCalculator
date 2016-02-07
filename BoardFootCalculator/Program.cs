using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BoardFootCalculator
{
    internal class Program
    {
        private static List<double> _stockLengths;

        private static void Main()
        {
            _stockLengths = AppSettingsHelper.GetStockLengths();

            var cutList = CutRetreiver.GetCuts();
            var groupedCuts = new List<double>();
            var stockCounts = _stockLengths.ToDictionary(x => x, x => new List<List<double>>());

            while (cutList.Count > 0)
            {
                cutList = cutList.OrderByDescending(x => x).ToList();
                foreach(var cut in cutList)
                {
                    if (groupedCuts.Sum(x => x) + cut <= _stockLengths.Max())
                    {
                        groupedCuts.Add(cut);
                    }
                }

                if (groupedCuts.Count == 0) break;

                foreach(var cutToRemove in groupedCuts)
                {
                    cutList.Remove(cutToRemove);
                }

                foreach(var stockLength in _stockLengths.OrderBy(x => x))
                {
                    if (groupedCuts.Sum(x => x) <= stockLength)
                    {
                        stockCounts[stockLength].Add(new List<double>(groupedCuts));
                    }
                }

                groupedCuts.Clear();
            }

            PrintResults(stockCounts, cutList);
            Console.ReadKey();
        }

        private static void PrintResults(Dictionary<double, List<List<double>>> results, List<double> cutsRemaining)
        {
            foreach (var result in results)
            {
                if (result.Value.Any())
                {
                    var headerString = $"Length: {result.Key}inches, Quantity: {result.Value.Count}";
                    Console.WriteLine(headerString);
                    Console.WriteLine(new string('-', headerString.Length));

                    foreach (var group in result.Value)
                    {
                        Console.WriteLine(string.Join(",", group));
                    }

                    Console.WriteLine("");
                }
            }

            if (cutsRemaining.Count > 0)
            {
                Console.WriteLine($"Cuts that couldn't be matched: {string.Join(",", cutsRemaining.Select(x => x))}");
            }
        }
    }
}