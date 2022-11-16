using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    [SerializeField] string latitude;
    [SerializeField] string longitude;
    [SerializeField] string altitude;
    [SerializeField] string horizontalAccuracy;
    [SerializeField] string timestamp;

    bool GpsIsActivating = false;
    Coroutine ActivateGPSCoroutine;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.location.status != LocationServiceStatus.Running)
            return;
        latitude = "xxx." + Input.location.lastData.latitude.ToString().Split('.');
        longitude = "xxx." + Input.location.lastData.longitude.ToString().Split('.');
        altitude = "xxx." + Input.location.lastData.altitude.ToString().Split('.');
        horizontalAccuracy = "xxx." + Input.location.lastData.horizontalAccuracy.ToString().Split('.');
        timestamp = Input.location.lastData.timestamp.ToString();
    }

    private void OnEnable()
    {

        if (ActivateGPSCoroutine == null)
            ActivateGPSCoroutine = StartCoroutine(ActivateGPS());
    }
    private void OnDisable()
    {
        StopCoroutine(ActivateGPSCoroutine);

        if (Input.location.status == LocationServiceStatus.Running)
            Input.location.Stop();
    }

    IEnumerator ActivateGPS()
    {
        Debug.Log("Unity Remote Connecting");
        while (UnityEditor.EditorApplication.isRemoteConnected == false)
            yield return new WaitForSecondsRealtime(1);

        Debug.Log("Unity Remote Connected");

        if (Input.location.isEnabledByUser == false)
        {
            Debug.Log("Location service is not enabled by usedr");
            yield break;
        }

        Debug.Log("Start Location Services");
        Input.location.Start();

        int maxWait = 15;
        while (Input.location.status == LocationServiceStatus.Stopped
        || Input.location.status == LocationServiceStatus.Initializing
        || maxWait > 0)

        {
            Debug.Log("Location Serivce Status Check:" + Input.location.status);
            yield return new WaitForSecondsRealtime(1);
            maxWait -= 1;
        }

        if (maxWait < 1)
        {
            Debug.Log("Location Service Failed Time Out");
            yield break;
        }
    }
}
