using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Vibration
{
    public static void Vibrate(long milliseconds)
    {
        if (IsAndroid())
        {
            // Access the Android vibrator service
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            // Check if the vibrator service exists
            if (vibrator != null)
            {
                // Vibrate the device with the specified duration
                vibrator.Call("vibrate", milliseconds);
            }
        }
    }

    // Check if the current platform is Android
    private static bool IsAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }
}
