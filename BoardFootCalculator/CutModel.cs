using System.Collections.Generic;
using System.Xml.Serialization;

namespace BoardFootCalculator
{
    [XmlRoot("CutList")]
    public class CutList
    {
        [XmlElement("Cut")]
        public List<Cut> Cuts { get; set; }
    }

    public class Cut
    {
        [XmlElement("Length")]
        public double Length { get; set; }

        [XmlElement("Quantity")]
        public int Quantity { get; set; }
    }
}
