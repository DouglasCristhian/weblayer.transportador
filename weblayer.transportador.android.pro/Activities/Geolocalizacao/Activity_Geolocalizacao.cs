using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace weblayer.transportador.android.pro.Activities
{
    [Activity()]
    public class Activity_Geolocalizacao : Activity, ILocationListener
    {
        string mensagem;
        string endereco;
        Location currentLocation;
        LocationManager locationManager;

        string locationProvider;
        string locationText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Geolocalizacao);

            locationManager = (LocationManager)GetSystemService(LocationService);

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
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (locationProvider == "")
            {
                Intent intent = new Intent();
                SetResult(Result.FirstUser, intent);
                Finish();
            }
            else
                locationManager.RequestLocationUpdates(locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }

        public void OnLocationChanged(Location location)
        {
            Intent intent = new Intent();
            try
            {
                currentLocation = location;

                if (currentLocation == null)
                    mensagem = "Endereço não encontrado. Tente novamente, por favor.";
                else
                {
                    locationText = String.Format("{0},{1}", currentLocation.Latitude, currentLocation.Longitude);

                    Geocoder geocoder = new Geocoder(this);


                    IList<Address> adressList = geocoder.GetFromLocation(currentLocation.Latitude, currentLocation.Longitude, 10);
                    Address adress = adressList.FirstOrDefault();

                    if (adress != null)
                    {
                        StringBuilder deviceAdress = new StringBuilder();

                        for (int i = 0; i < adress.MaxAddressLineIndex; i++)
                        {
                            deviceAdress.Append(adress.GetAddressLine(i)).AppendLine(",");
                        }

                        endereco = deviceAdress.ToString();

                        intent.PutExtra("Lat", adress.Latitude.ToString());
                        intent.PutExtra("Lon", adress.Longitude.ToString());
                        intent.PutExtra("Endereco", endereco);

                        SetResult(Result.FirstUser, intent);
                        Finish();
                    }
                    else
                    {
                        SetResult(Result.FirstUser, intent);
                        Finish();
                    }
                }
            }
            catch
            {
                mensagem = "Endereço não encontrado. Tente novamente, por favor.";
                intent.PutExtra("mensagem", mensagem);
                SetResult(Result.Ok, intent);
                Finish();
            }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }
    }
}