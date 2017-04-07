using Android.App;
using Android.Content;
using Android.Locations;
using Android.Net;
using Android.OS;
using Android.Util;
using System;
using System.Threading;


namespace weblayer.transportador.android
{
    [Service]
    [IntentFilter(new String[] { "weblayer.transportador.android.exp" })] // Copy and paste you package From Manifest.xml package="**************"
    public class ARLocationService : Service, ILocationListener
    {
        public const string TAG = "ARLocationService";

        ARLocationServiceBinder binder;
        Timer tmrBackgroundInitializer;
        long TotalMillisecondsTmrForBackground = 3000;

        private LocationManager _locMgr;

        private static Android.OS.Vibrator onMsgVibrator;
        long TotalMilliseconds = 2000;

        double lastLatitude = double.MinValue;
        double lastLongitude = double.MinValue;
        DateTime lastUpdated = DateTime.MinValue;
        bool IscalledLoactionMngr = false;

        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(TAG, "ARL Service starting");

            InitializeBackgroundWork();

            Log.Debug(TAG, "ARL Service started successfully.");
            return StartCommandResult.Sticky;
        }

        public override void OnCreate()
        {
            base.OnCreate();


        }

        public override void OnDestroy()
        {

            base.OnDestroy();
            Log.Debug(TAG, "ARL Service stopped");
        }

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            binder = new ARLocationServiceBinder(this);
            return binder;
        }

        private void InitializeBackgroundWork()
        {
            try
            {
                if (tmrBackgroundInitializer == null)
                    tmrBackgroundInitializer = new Timer(new TimerCallback(PerformBackgroundWork), null, TotalMillisecondsTmrForBackground, TotalMillisecondsTmrForBackground);
                else
                    tmrBackgroundInitializer.Change(TotalMillisecondsTmrForBackground, TotalMillisecondsTmrForBackground);

                if (_locMgr == null)
                {
                    _locMgr = GetSystemService(LocationService) as LocationManager;

                    var locationCriteria = new Criteria();

                    locationCriteria.Accuracy = Accuracy.NoRequirement;
                    locationCriteria.PowerRequirement = Power.NoRequirement;

                    string locationProvider = _locMgr.GetBestProvider(locationCriteria, true);

                    _locMgr.RequestLocationUpdates(locationProvider, 2000, 1, this);
                }

            }
            catch (Exception objErr)
            {
                Log.Debug(TAG, objErr.ToString());
            }



        }

        private void PerformBackgroundWork(object state)
        {
            try
            {
                tmrBackgroundInitializer.Change(50000, 50000);

                if (DetectNetwork())
                {
                    //Update your code here
                }


            }
            catch (Exception objErr)
            {
                Log.Debug(TAG, objErr.ToString());
            }
            finally
            {
                tmrBackgroundInitializer.Change(TotalMillisecondsTmrForBackground, TotalMillisecondsTmrForBackground);
            }

        }

        public bool DetectNetwork()
        {
            try
            {
                var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
                var wifiState = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
                var activeConnection = connectivityManager.ActiveNetworkInfo;

                if ((activeConnection != null) && activeConnection.IsConnected || wifiState == NetworkInfo.State.Connected)
                {
                    return true;

                }
                else
                    return false;

            }
            catch (Exception exe)
            {
                throw (exe);
            }
        }

        #region ILocationListener Members


        public void OnLocationChanged(Location location)
        {
            IscalledLoactionMngr = true;

            lastLatitude = location.Latitude;
            lastLongitude = location.Longitude;

            lastUpdated = DateTime.Now;
            // demo geocoder
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }

    public class ARLocationServiceBinder : Binder
    {
        ARLocationService service;

        public ARLocationServiceBinder(ARLocationService service)
        {
            this.service = service;
        }

        public ARLocationService GetARLocationServiceBinder()
        {
            return service;
        }
    }

}