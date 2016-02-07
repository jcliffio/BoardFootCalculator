using System;
using System.Collections.Generic;
using System.Linq;

namespace BoardFootCalculator
{
    internal class Program
    {
        private static void Main()
        {
            var cutList = CutRetreiver.GetCuts();
            var stockCounts = CalculateMinimumStock(ref cutList);

            PrintResults(stockCounts, cutList);
            Console.ReadKey();
        }

        private static Dictionary<double,List<List<double>>> CalculateMinimumStock(ref List<double> cutList)
        {
            var stockLengths = AppSettingsHelper.GetStockLengths();
            var stockCounts = stockLengths.ToDictionary(x => x, x => new List<List<double>>());
            var groupedCuts = new List<double>();

            // While we still have cuts that haven't been grouped, keep churning.
            while (cutList.Count > 0)
            {
                // We always want to grab the longest cuts first to "fill in the gaps" with shorter cuts.
                cutList = cutList.OrderByDescending(x => x).ToList();
                foreach (var cut in cutList)
                {
                    if (groupedCuts.Sum() + cut <= stockLengths.Max())
                    {
                        groupedCuts.Add(cut);
                    }
                }

                // No cuts were short enough to be added to the group.
                // At this point, we're left with boards that are too long to fit the stock sizes.
                // Bail out of the loop.
                if (groupedCuts.Count == 0) break;

                foreach (var cutToRemove in groupedCuts)
                {
                    cutList.Remove(cutToRemove);
                }

                // Check the total length of the grouped cuts to see what is the smallest board we can get away with using.
                foreach (var stockLength in stockLengths.OrderBy(x => x))
                {
                    // If we match on a board, don't keep checking larger boards!
                    if (groupedCuts.Sum() <= stockLength)
                    {
                        stockCounts[stockLength].Add(new List<double>(groupedCuts));
                        break;
                    }
                }

                groupedCuts.Clear();
            }

            return stockCounts;
        }

        private static void PrintResults(Dictionary<double, List<List<double>>> results, List<double> cutsRemaining)
        {
            foreach (var result in results)
            {
                // Only print results for a stock length if any exist
                if (result.Value.Any())
                {
                    var headerString = $"Length: {result.Key}\", Quantity: {result.Value.Count}";
                    Console.WriteLine(headerString);
                    Console.WriteLine(new string('-', headerString.Length));

                    foreach (var group in result.Value)
                    {
                        Console.Write(string.Join(",", group.Select(x => $"{x}\"")).PadRight(30));
                        Console.WriteLine($"Waste: {result.Key - group.Sum()}\"");
                    }

                    Console.WriteLine("");
                }
            }

            if (cutsRemaining.Count > 0)
            {
                Console.WriteLine($"Cuts that couldn't be matched: {string.Join(",", cutsRemaining)}");
            }
        }
    }
}