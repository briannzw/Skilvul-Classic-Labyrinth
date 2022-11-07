using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    public string latitude;
    public string longitude;
    public string altitude;
    public string horizontalAccuracy;
    public string timestamp;

    Coroutine ActivateGPSCoroutine;

    private void OnEnable()
    {
        if (ActivateGPSCoroutine != null) return;

        ActivateGPSCoroutine = StartCoroutine(ActivateGPS());
    }

    private void Update()
    {
        if (Input.location.status != LocationServiceStatus.Running)
            return;

        latitude = Input.location.lastData.latitude.ToString();
        longitude = Input.location.lastData.longitude.ToString();
        altitude = Input.location.lastData.altitude.ToString();
        horizontalAccuracy = Input.location.lastData.horizontalAccuracy.ToString();
        timestamp = Input.location.lastData.timestamp.ToString();

        transform.rotation = Quaternion.Euler(0, -Input.compass.magneticHeading, 0);
    }

    private void OnDisable()
    {
        StopCoroutine(ActivateGPSCoroutine);
        
        if(Input.location.status == LocationServiceStatus.Running)
        {
            Input.location.Stop();
        }
    }

    IEnumerator ActivateGPS()
    {
#if UNITY_EDITOR
        Debug.Log("Unity Remote Connecting");
        while (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            yield return new WaitForSecondsRealtime(1);
        }
#endif

        Debug.Log("Unity Remote Connected");

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location is not enabled");
            yield break;
        }

        Input.location.Start();

        int maxWait = 15;
        while (Input.location.status == LocationServiceStatus.Stopped
            || Input.location.status == LocationServiceStatus.Initializing
            && maxWait > 0)
        {
            yield return new WaitForSecondsRealtime(1);
            maxWait--;
        }

        if(maxWait < 1)
        {
            Debug.Log("Location Service Timeout");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Location Service Failed");
            yield break;
        }

        Input.compass.enabled = true;
    }
}
