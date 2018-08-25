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
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace CellGo1
{
    public class MapObjectManager
    {
        List<MapObjects> mapObjects;
        HttpClient httpClient;
        string url = "http://192.168.0.13:7500/api/MapObjects/";
        GoogleMap gMap;

        public MapObjectManager()
        {
            mapObjects = new List<MapObjects>();
            httpClient = new HttpClient();            
        }

        public void CreateTestObject(string region)
        {
            MapObjects mo = new MapObjects();
            mo.Lat = 47.6750829m;
            mo.Lon = -122.1140707m;
            mo.Type = "monster";
            mo.Region = region;
            mo.AttributesDict = new Dictionary<string, string>();
            mo.AttributesDict.Add("name", "rat");
            mo.AttributesDict.Add("hitpoints", "100");
            mo.TimeStamp = DateTime.UtcNow;
            PostMapObject(mo);
        }

        async public void GetMapObjects(GoogleMap gMap, string region, DateTime? timestamp)
        {
            string queryString;
            if(timestamp != null)
            { 
                var query = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "region", region},
                    { "timestamp", DateTime.UtcNow.ToString() },
                }).ReadAsStringAsync().Result;
                queryString = query.ToString();
            }
            else
            {
                var query = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "region", region}
                }).ReadAsStringAsync().Result;
                queryString = query.ToString();
            }
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url + "byRegion?" + queryString);
            mapObjects = await httpResponse.Content.ReadAsJsonAsync<List<MapObjects>>();
            AddObjectsToMap(gMap);
        }

        async public void PostMapObject(MapObjects mo)
        {
            string json = JsonConvert.SerializeObject(mo);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, stringContent);
            string result = response.Content.ToString();
        }

        private void AddObjectsToMap(GoogleMap map)
        {
            foreach(MapObjects mo in mapObjects)
            {
                MarkerOptions markOpt = new MarkerOptions();
                markOpt.SetPosition(new LatLng((double)mo.Lat,(double)mo.Lon));
                switch(mo.AttributesDict["name"])
                {
                    case "wurm":
                        markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_07));
                        break;
                    case "bee":
                        markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_09));
                        break;
                    case "crab":
                        markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_13));
                        break;
                    case "rat":
                        markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_03));
                        break;
                    case "snake":
                        markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_15));
                        break;
                    default:
                        markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_07));
                        break;
                }
                Marker tempM = map.AddMarker(markOpt);
                mo.markerID = tempM.Id;
            }
        }
    }
}