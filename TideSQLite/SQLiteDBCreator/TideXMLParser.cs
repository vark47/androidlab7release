using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using DAL;

namespace SQLiteDBCreator
{
    public static class TideXMLParser
    {
        public static List<Tide> Parse(string xmlfile)
        {
            var tideXML = File.ReadAllText(xmlfile);
            var root = XElement.Parse(tideXML);
            var data = root.Element("data");
            var tideList = new List<Tide>();
            var location = root.Element("stationname").Value.Trim();
            location += ", " + root.Element("state").Value.Trim();

            foreach (var item in data.Elements("item"))
            {
                var dateString = item.Element("date").Value.Trim();
                dateString += " " + item.Element("time").Value.Trim();
                var time = DateTime.Parse(dateString);

                var tide = new Tide();
                tide.Location = location;
                tide.TimeTicks = time.Ticks;
                tide.FeetHeight = Convert.ToSingle(item.Element("predictions_in_ft").Value);
                tide.CentimeterHeight = Convert.ToInt32(item.Element("predictions_in_cm").Value.Trim());
                tide.HighLow = item.Element("highlow").Value.Trim().Equals("H", StringComparison.CurrentCultureIgnoreCase) ? "High" : "Low";

                tideList.Add(tide);
            }

            return tideList;
        }
    }
}