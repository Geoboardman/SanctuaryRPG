using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CellGo1
{
    public class MapObjects
    {
        public int Id { get; set; }
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public Dictionary<string,string> AttributesDict { get; set; }
        public DateTime TimeStamp { get; set; }
        public string markerID { get; set; }
    }
}