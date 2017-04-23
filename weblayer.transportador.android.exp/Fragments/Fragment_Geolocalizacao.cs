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
using weblayer.transportador.android.core.Helpers;


namespace weblayer.transportador.android.exp.Fragments
{
    public class Fragment_Geolocalizacao : DialogFragment, ILocationListener
    {
        public event EventHandler<DialogEventArgs> DialogClosed;
        string latitude;
        string longitude;
        string mensagem;
        string endereco;
        Location currentLocation;
        LocationManager locationManager;

        string locationProvider;
        string locationText;

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

                        endereco = deviceAdress.ToString();
                        latitude = adress.Latitude.ToString();
                        longitude = adress.Longitude.ToString();

                        intent.PutExtra("Lat", adress.Latitude.ToString());
                        intent.PutExtra("Lon", adress.Longitude.ToString());
                        intent.PutExtra("Endereco", endereco);
                        //SetResult(Result.FirstUser, intent);
                        Dismiss();
                    }
                    else
                    {
                        mensagem = "Endereço não encontrado. Tente novamente, por favor.";
                        intent.PutExtra("mensagem", mensagem);
                        //SetResult(Result.FirstUser, intent);
                        Dismiss();
                    }
                }
            }
            catch
            {
                mensagem = "Endereço não encontrado. Tente novamente, por favor.";
                intent.PutExtra("mensagem", mensagem);
                Dismiss();
            }
        }

        public override void OnDismiss(IDialogInterface dialog)
        {
            base.OnDismiss(dialog);
            if (DialogClosed != null)
            {
                DialogClosed(this, new DialogEventArgs
                { ReturnValue = mensagem, ReturnValue1 = latitude, ReturnValue2 = longitude, ReturnValue3 = endereco });
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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Fragment_Geolocalizacao, container, false);

            locationManager = (LocationManager)this.Activity.GetSystemService(Context.LocationService);

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
            return view;
        }
    }
}