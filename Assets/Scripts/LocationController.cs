using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LocationController : MonoBehaviour
{
    [SerializeField]
    private Text statusText;

    public Vector2 viewerLocation = new Vector2(38.89099f,-77.00037f);
    public UnityEvent locationUpdated;

    [SerializeField]
    private Button m_updateLocationButton;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateLocation());
        m_updateLocationButton.onClick.AddListener(RequestUpdate);
    }
    private void RequestUpdate() => StartCoroutine(UpdateLocation());

    // Get Location updates from Device
    IEnumerator UpdateLocation() {
        if (locationUpdated == null)
            locationUpdated = new UnityEvent();
        
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser) {
            
            print("Location: Not Enabled");
            // Debugging - used a fix location  
            
            BroadcastLocation();
            yield break;
        }
            

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Location: Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Location: Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            viewerLocation = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
            
            BroadcastLocation();
        }
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    void BroadcastLocation() {
        string locationText = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " (" + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + ") -> " + Input.compass.trueHeading + " @ " + Input.location.lastData.timestamp;
        print(locationText);

        statusText.text = locationText;
        locationUpdated.Invoke();
    }

}
