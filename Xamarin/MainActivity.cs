using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace DWGettingStartedXamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, LaunchMode =Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter(new[] { "com.darryncampbell.datawedge.xamarin.ACTION" }, Categories = new[] { Intent.CategoryDefault})]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            DWUtilities.CreateDWProfile(this);
            Button btnScan = (Button)FindViewById(Resource.Id.btnScan);
            btnScan.Touch += (s, e) =>
            {
                if (e.Event.Action == MotionEventActions.Down)
                {
                    //  Button pressed, start scan
                    Intent dwIntent = new Intent();
                    dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
                    dwIntent.PutExtra("com.symbol.datawedge.api.SOFT_SCAN_TRIGGER", "START_SCANNING");
                    SendBroadcast(dwIntent);
                }
                else if (e.Event.Action == MotionEventActions.Up)
                {
                    //  Button released, end scan
                    Intent dwIntent = new Intent();
                    dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
                    dwIntent.PutExtra("com.symbol.datawedge.api.SOFT_SCAN_TRIGGER", "STOP_SCANNING");
                    SendBroadcast(dwIntent);
                }
                e.Handled = true;
            };
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            DisplayScanResult(intent);
        }

        private void DisplayScanResult(Intent scanIntent)
        {
            String decodedSource = scanIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_source));
            String decodedData = scanIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_data));
            String decodedLabelType = scanIntent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_label_type));
            String scan = decodedData + " [" + decodedLabelType + "]\n\n";
            TextView output = FindViewById<TextView>(Resource.Id.txtOutput);
            output.Text = scan + output.Text;
        }


    }
}
