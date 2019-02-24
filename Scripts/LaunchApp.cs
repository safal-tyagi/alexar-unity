using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LaunchApp : MonoBehaviour
{
    private Button launchButton;


    // Use this for initialization
    void Start()
    {
        launchButton = gameObject.GetComponent<Button>();
        launchButton.onClick.AddListener(launchApp);
    }

    void launchApp()
    {
        Debug.Log("AAA");
        bool fail = false;
        string bundleId = "com.amazon.dee.app"; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            fail = true;
        }
        string storeLink = "https://play.google.com/store/apps/details?id=com.amazon.dee.app";
        if (fail && storeLink != null)
        { //open app in store
            Application.OpenURL(storeLink);
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }
}
