package com.darryncampbell.dwgettingstartedkotlin

import android.content.Context
import android.content.Intent
import android.os.Bundle

object DWUtilities {
    private const val PROFILE_NAME = "DWGettingStartedKotlin"
    private const val ACTION_DATAWEDGE = "com.symbol.datawedge.api.ACTION"
    private const val EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE"
    private const val EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG"
    fun CreateDWProfile(context: Context) {
        sendDataWedgeIntentWithExtra(
            context,
            ACTION_DATAWEDGE,
            EXTRA_CREATE_PROFILE,
            PROFILE_NAME
        )

        //  Requires DataWedge 6.4

        //  Now configure that created profile to apply to our application
        val profileConfig = Bundle()
        profileConfig.putString("PROFILE_NAME", PROFILE_NAME)
        profileConfig.putString("PROFILE_ENABLED", "true") //  Seems these are all strings
        profileConfig.putString("CONFIG_MODE", "UPDATE")
        val barcodeConfig = Bundle()
        barcodeConfig.putString("PLUGIN_NAME", "BARCODE")
        barcodeConfig.putString("RESET_CONFIG", "true")
        val barcodeProps = Bundle()
        barcodeProps.putString("configure_all_scanners", "true")
        barcodeProps.putString("scanner_input_enabled", "true")
        barcodeConfig.putBundle("PARAM_LIST", barcodeProps)
        profileConfig.putBundle("PLUGIN_CONFIG", barcodeConfig)
        val appConfig = Bundle()
        appConfig.putString(
            "PACKAGE_NAME",
            context.packageName
        ) //  Associate the profile with this app
        appConfig.putStringArray("ACTIVITY_LIST", arrayOf("*"))
        profileConfig.putParcelableArray("APP_LIST", arrayOf(appConfig))
        sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_SET_CONFIG, profileConfig)

        //  You can only configure one plugin at a time, we have done the barcode input, now do the intent output
        profileConfig.remove("PLUGIN_CONFIG")
        val intentConfig = Bundle()
        intentConfig.putString("PLUGIN_NAME", "INTENT")
        intentConfig.putString("RESET_CONFIG", "true")
        val intentProps = Bundle()
        intentProps.putString("intent_output_enabled", "true")
        intentProps.putString(
            "intent_action",
            context.resources.getString(R.string.activity_intent_filter_action)
        )
        intentProps.putString("intent_delivery", "0") //  StartActivity
        intentConfig.putBundle("PARAM_LIST", intentProps)
        profileConfig.putBundle("PLUGIN_CONFIG", intentConfig)
        sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_SET_CONFIG, profileConfig)

        //  Disable keyboard output
        profileConfig.remove("PLUGIN_CONFIG")
        val keystrokeConfig = Bundle()
        keystrokeConfig.putString("PLUGIN_NAME", "KEYSTROKE")
        keystrokeConfig.putString("RESET_CONFIG", "true")
        val keystrokeProps = Bundle()
        keystrokeProps.putString("keystroke_output_enabled", "false")
        keystrokeConfig.putBundle("PARAM_LIST", keystrokeProps)
        profileConfig.putBundle("PLUGIN_CONFIG", keystrokeConfig)
        sendDataWedgeIntentWithExtra(context, ACTION_DATAWEDGE, EXTRA_SET_CONFIG, profileConfig)
    }

    private fun sendDataWedgeIntentWithExtra(
        context: Context,
        action: String,
        extraKey: String,
        extraValue: String
    ) {
        val dwIntent = Intent()
        dwIntent.action = action
        dwIntent.putExtra(extraKey, extraValue)
        context.sendBroadcast(dwIntent)
    }

    private fun sendDataWedgeIntentWithExtra(
        context: Context,
        action: String,
        extraKey: String,
        extras: Bundle
    ) {
        val dwIntent = Intent()
        dwIntent.action = action
        dwIntent.putExtra(extraKey, extras)
        context.sendBroadcast(dwIntent)
    }
}
