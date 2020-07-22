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

namespace DWGettingStartedXamarin
{
    static class DWUtilities
    {
        private const String PROFILE_NAME = "DWGettingStartedXamarin";
        private const String ACTION_DATAWEDGE = "com.symbol.datawedge.api.ACTION";
        private const String EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE";
        private const String EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG";

    public static void CreateDWProfile(Context context)
        {
            sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_CREATE_PROFILE, PROFILE_NAME);

            //  Requires DataWedge 6.4

            //  Now configure that created profile to apply to our application
            Bundle profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", PROFILE_NAME);
            profileConfig.PutString("PROFILE_ENABLED", "true"); //  Seems these are all strings
            profileConfig.PutString("CONFIG_MODE", "UPDATE");

            Bundle barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true");
            Bundle barcodeProps = new Bundle();
            barcodeProps.PutString("configure_all_scanners", "true");
            barcodeProps.PutString("scanner_input_enabled", "true");
            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);
            Bundle appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", context.PackageName);      //  Associate the profile with this app
            appConfig.PutStringArray("ACTIVITY_LIST", new String[] { "*" });
            profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });
            sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_SET_CONFIG, profileConfig);

            //  You can only configure one plugin at a time, we have done the barcode input, now do the intent output
            profileConfig.Remove("PLUGIN_CONFIG");
            Bundle intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");
            Bundle intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            intentProps.PutString("intent_action", context.Resources.GetString(Resource.String.activity_intent_filter_action));
            intentProps.PutString("intent_delivery", "0");  //  StartActivity
            intentConfig.PutBundle("PARAM_LIST", intentProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);
            sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_SET_CONFIG, profileConfig);

            //  Disable keyboard output
            profileConfig.Remove("PLUGIN_CONFIG");
            Bundle keystrokeConfig = new Bundle();
            keystrokeConfig.PutString("PLUGIN_NAME", "KEYSTROKE");
            keystrokeConfig.PutString("RESET_CONFIG", "true");
            Bundle keystrokeProps = new Bundle();
            keystrokeProps.PutString("keystroke_output_enabled", "false");
            keystrokeConfig.PutBundle("PARAM_LIST", keystrokeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", keystrokeConfig);
            sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_SET_CONFIG, profileConfig);
        }

        private static void sendDataWedgeIntentWithExtra(Context context, String action, String extraKey, String extraValue)
        {
            Intent dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extraValue);
            context.SendBroadcast(dwIntent);
        }

        private static void sendDataWedgeIntentWithExtra(Context context, String action, String extraKey, Bundle extras)
        {
            Intent dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extras);
            context.SendBroadcast(dwIntent);
        }
    }
}