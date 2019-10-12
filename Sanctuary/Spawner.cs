using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CellGo1
{
    public class Spawner
    {
        int minCount = 1;
        int maxCount = 10;
        double latitude;
        double longitude;
        int radius = 100; //in meters
        GoogleMap map;
        List<Marker> markers;

        public Spawner(GoogleMap map, double lat, double lon, int minCount, int maxCount, int radius)
        {
            latitude = lat;
            longitude = lon;
            this.map = map;
            this.minCount = minCount;
            this.maxCount = maxCount;
            this.radius = radius;
            markers = new List<Marker>();
            Spawn();
        }

        // Generate a random number between two numbers  
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }

        private void Spawn()
        {
            MarkerOptions markOpt;
            int spawnCount = RandomNumber(minCount, maxCount);
            for(int i = 0; i < spawnCount; i++)
            {
                markOpt = new MarkerOptions();
                markOpt.SetPosition(getLocation(latitude, longitude, radius));
                markOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.monster_07));
                Marker tempM = map.AddMarker(markOpt);
                markers.Add(tempM);
            }
        }

        public LatLng getLocation(double x0, double y0, int radius)
        {
            Random random = new Random();

            // Convert radius from meters to degrees
            double radiusInDegrees = radius / 111000f;

            double u = random.NextDouble();
            double v = random.NextDouble();
            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;
            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            // Adjust the x-coordinate for the shrinking of the east-west distances
            double new_x = x / Math.Cos(ConvertToRadians(y0));

            double foundLongitude = new_x + x0;
            double foundLatitude = y + y0;
            return new LatLng(foundLongitude, foundLatitude);
        }
    }
}