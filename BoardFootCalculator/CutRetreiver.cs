using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BoardFootCalculator
{
    public class CutRetreiver
    {
        public static List<double> GetCuts()
        {
            CutList cutlist;
            using (var reader = new StreamReader("Cutlist.xml"))
            {
                var deserializer = new XmlSerializer(typeof(CutList));
                cutlist = (CutList)deserializer.Deserialize(reader);
            }

            var cutsList = new List<double>();

            foreach (var cut in cutlist.Cuts)
            {
                for (var i = 0; i < cut.Quantity; i++)
                {
                    cutsList.Add(cut.Length);
                }
            }

            return cutsList.OrderByDescending(x => x).ToList();
        }
    }
}
