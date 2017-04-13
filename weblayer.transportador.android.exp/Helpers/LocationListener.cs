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
using Android.Locations;

namespace weblayer.transportador.android.exp.Helpers
{
    public class LocationListener : Java.Lang.Object, ILocationListener
    {
        protected LocationManager locationManager;
        Location location;
        Location gpsLocation, nwLocation;
        public static double lonI, latI;
        private static long MIN_DISTANCE_FOR_UPDATE = 10;
        private static long MIN_TIME_FOR_UPDATE = 1000 * 60 * 2;
        private static String TAG = "location";
        Context context;



        public LocationListener()
        {
            locationManager = Application.Context.GetSystemService(Context.LocationService) as LocationManager;
        }

        public Location getLocation(String provider)
        {
            var locationCriteria = new Criteria();

            locationCriteria.Accuracy = Accuracy.Coarse;
            locationCriteria.PowerRequirement = Power.Medium;

            var locationProvider = locationManager.GetBestProvider(locationCriteria, true);
            if (locationManager.IsProviderEnabled(provider))
            {
                locationManager.RequestLocationUpdates(provider, MIN_TIME_FOR_UPDATE, MIN_DISTANCE_FOR_UPDATE, this);
                if (locationManager != null)
                {
                    location = locationManager.GetLastKnownLocation(provider);
                    return location;
                }
            }
            return null;
        }


        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                latI = location.Latitude;
                lonI = location.Longitude;
                String longitude = "Longitude: " + location.Longitude;
                String latitude = "Latitude: " + location.Latitude;
            }
        }

        public void OnProviderDisabled(string provider)
        {
            // throw new NotImplementedException();



        }

        public void OnProviderEnabled(string provider)
        {
            //     throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }
    }
}