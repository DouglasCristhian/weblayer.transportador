using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weblayer.transportador.android.exp.Fragments
{
    public class GeolocalizacaoFragment : Fragment, ILocationListener
    {
        Location currentLocation;
        LocationManager locationManager;
        string locationProvider;
        string locationText;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            locationManager = (LocationManager)Context.GetSystemService(Context.LocationService);

            Criteria criteriaForLorcationService = new Criteria
            {
                Accuracy = Accuracy.Coarse,
                PowerRequirement = Power.Medium
            };


            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLorcationService, true);

            if (acceptableLocationProviders.Any())
                locationProvider = acceptableLocationProviders.First();
            else
                locationProvider = string.Empty;

            return inflater.Inflate(Resource.Layout.Fragment_Geolocalizacao, container, false);
        }

        public void OnLocationChanged(Location location)
        {
            try
            {
                currentLocation = location;

                if (currentLocation == null)
                    locationText = "Location not found...";
                else
                {
                    locationText = String.Format("{0},{1}", currentLocation.Latitude, currentLocation.Longitude);

                    Geocoder geocoder = new Geocoder(this.Activity);


                    IList<Address> adressList = geocoder.GetFromLocation(currentLocation.Latitude, currentLocation.Longitude, 10);
                    Address adress = adressList.FirstOrDefault();

                    if (adress != null)
                    {
                        StringBuilder deviceAdress = new StringBuilder();

                        for (int i = 0; i < adress.MaxAddressLineIndex; i++)
                        {
                            deviceAdress.Append(adress.GetAddressLine(i)).AppendLine(",");
                        }

                        locationText = deviceAdress.ToString();
                    }
                    else
                        locationText = "Adress not found...";
                }
            }
            catch
            {
                locationText = "Adress not found...";
            }
        }

        public void OnProviderDisabled(string provider)
        {
            locationManager.RemoveUpdates(this);
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
            locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}