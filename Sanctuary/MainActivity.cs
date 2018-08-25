using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Gms.Maps;
using Android.Support.V4.Content;
using Android;
using Android.Locations;
using System.Runtime.Remoting.Contexts;
using Android.Runtime;
using Android.Gms.Maps.Model;
using System.Threading;
using System.Collections.Generic;
using Android.Animation;
using static Android.Gms.Maps.GoogleMap;
using Android.Content;

namespace CellGo1
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity, IOnMapReadyCallback, IOnMarkerClickListener
    {

        public MapFragment mapFragment = null;
        public GoogleMap map = null;
        Location location = null;
        LocationListener locationListener = null;
        MapClickListener mapClickListener = null;
         

        Marker myMarker = null;
        MapObjectManager mapObjectManager;
        string playerRegion;
        bool clickMovement = false;
        LatLng clickPos;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            mapObjectManager = new MapObjectManager();

            SetContentView(Resource.Layout.activity_main);

			Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

			FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            

            GoogleMapOptions mapOptions = new GoogleMapOptions()
                .InvokeMapType(GoogleMap.MapTypeNormal)
                .InvokeZoomControlsEnabled(true)
                .InvokeCompassEnabled(true);



            mapFragment = MapFragment.NewInstance(mapOptions);

            FragmentTransaction fragTx = FragmentManager.BeginTransaction().Add(Resource.Id.mapContainer,mapFragment);                     
            fragTx.Commit();
            mapFragment.GetMapAsync(this);

            LocationManager locationManager = (LocationManager) GetSystemService(Android.Content.Context.LocationService);

            Criteria criteria = new Criteria();
            string mprovider = locationManager.GetBestProvider(criteria, false);
            location = locationManager.GetLastKnownLocation(mprovider);

            LocationListener locationListener = new LocationListener(this);
            locationManager.RequestLocationUpdates( LocationManager.GpsProvider, 5000, 10, locationListener);
        }

        protected override void OnResume()
        {
            base.OnResume();


        }

        private void StartBattle()
        {
            Intent intent = new Intent(this, typeof(battle_activity));
            intent.PutExtra("name", "wurm");
            intent.PutExtra("hitpoints", 100);
            StartActivity(intent);
        }

        class LocationListener : Java.Lang.Object, ILocationListener
        {
            MainActivity _this = null;

            public LocationListener(MainActivity thisIn)
            {
                _this = thisIn;
            }

            public void OnLocationChanged(Location location)
            {
                if (_this.map != null)
                {
                    if (_this.myMarker == null)
                    {
                        MarkerOptions markerOpt1 = new MarkerOptions();
                        markerOpt1.SetPosition(new LatLng(location.Latitude, location.Longitude));
                        markerOpt1.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.player_01));
                        
                        _this.myMarker = _this.map.AddMarker(markerOpt1);
                    }
                    else
                    {
                        if (!_this.clickMovement)
                        {
                            _this.myMarker.Position = new LatLng(location.Latitude, location.Longitude);
                        }
                    }
                    
                }
                //Update region
                _this.playerRegion = _this.CalculateRegion((decimal)location.Latitude, (decimal)location.Longitude);
            }

            public void OnProviderDisabled(string provider)
            {
            }

            public void OnProviderEnabled(string provider)
            {
            }

            public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
            {
            }
        }

        private string CalculateRegion(decimal lat, decimal lon)
        {
            lat = Math.Floor(lat * 100) / 100;
            lon = Math.Floor(lon * 100) / 100;
            //Add the width and height of a cell to get center point for region
            if (lat > 0)
                lat += .005m;
            else
                lat -= .005m;
            if (lon > 0)
                lon += .005m;
            else
                lon -= .005m;
            return lat.ToString() + "," + lon.ToString();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            clickMovement = !clickMovement;
            /*
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                */         
        }

        class MapClickListener : Java.Lang.Object, GoogleMap.IOnMapClickListener
        {
            MainActivity _this = null;

            public MapClickListener(MainActivity thisIn)
            {
                _this = thisIn;
            }

            public void OnMapClick(LatLng point)
            {
                //Move the player
                if(_this.clickMovement)
                {
                    _this.myMarker.Position = point;
                    if(_this.clickPos != point)
                    {
                        _this.clickPos = point;

                    }
                }
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;

            bool success = map.SetMapStyle(
                    MapStyleOptions.LoadRawResourceStyle(
                            this, Resource.Raw.style_json));


            LatLng latLng = new LatLng(location.Latitude, location.Longitude);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(latLng);
            builder.Zoom(18);
            builder.Bearing(0);
            builder.Tilt(0);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            map.MoveCamera(cameraUpdate);
            map.SetOnMarkerClickListener(this);
            mapClickListener = new MapClickListener(this);
            map.SetOnMapClickListener(mapClickListener);
            map.SetOnMarkerClickListener(this);

            playerRegion = CalculateRegion((decimal)location.Latitude, (decimal)location.Longitude);
            //mapObjectManager.CreateTestObject(playerRegion);
            mapObjectManager.GetMapObjects(map, playerRegion, null);
        }

        public bool OnMarkerClick(Marker marker)
        {
            StartBattle();
            return true;
        }
    }
}

