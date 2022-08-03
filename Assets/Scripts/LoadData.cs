using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON; 
using UnityEngine.Events;

public class LoadData : MonoBehaviour
{
    public List<GameObject> allMarkers;

    [SerializeField, Tooltip("Search radius for features, in meters")]
    public float markerDistance = 100;
    [SerializeField, Tooltip("..")]
    public float markerScale = 1;
    
    [SerializeField, Tooltip("..")]
    public float markerAltitude = 0;

    // DC Trees
    [SerializeField, Tooltip("URL to the ArcGIS Feature Layer")]
    public string markerLayerUrl = "https://services.arcgis.com/bkrWlSKcjUDFDtgw/ArcGIS/rest/services/DC_Trees_Republished/FeatureServer/0";
    [SerializeField, Tooltip("Feature Layer attribute to show on text overlay.")]
    public string markerLayerDisplay = "OBJECTID";
    [SerializeField, Tooltip("Geometry type of the feature layer (point, line, polygon)")]
    public string markerLayerType = "point";

    public GameObject signPost = null;


    public LocationController locationController;

    void Awake() {
        // Listen for location updates
        locationController.locationUpdated.AddListener(UpdateLocation);
    }

    void UpdateLocation()
    {    
        print("GetData: " + markerLayerUrl);

        GeoService.GetData(markerLayerUrl, locationController.viewerLocation, markerLayerType, markerDistance, addMarkers);

        // Debug - display lon/lat on screen
        // GetComponent<TextMesh>().text = viewerLocation.ToString();   
    }
    void addMarkers(JSONNode features) 
    {
        for (int i = 0; i <= features.Count; i++)
        {
            Vector2 markerLocation = new Vector2(features[i]["geometry"]["y"], features[i]["geometry"]["x"]);
            if(markerLayerType == "polygon")
            {
                markerLocation = new Vector2(features[i]["centroid"]["y"], features[i]["centroid"]["x"]);
            } 
            
            GameObject marker = addMarker(markerLocation, features[i]["attributes"][markerLayerDisplay]);
        }                                   
    }

    // TODO: change this to use ARAnchor or ARKitGeoAnchors (iOS only)
    // https://github.com/Unity-Technologies/arfoundation-samples/tree/main/Assets/Scenes/ARKit/ARKitGeoAnchors
    GameObject addMarker(Vector2 markerLocation, string label)
    {
        Vector3 ucsLocation = GPSEncoder.GPSToUCS(markerLocation, locationController.viewerLocation);
        ucsLocation.y += markerAltitude;

        // Let's choose a marker shape!
        GameObject marker = allMarkers[UnityEngine.Random.Range(0, allMarkers.Count)];

        GameObject markerObject = Instantiate(marker, ucsLocation, Quaternion.identity);

        // Add a signpost
        if(signPost != null) {
            GameObject sign = Instantiate(signPost, Vector3.zero, Quaternion.identity);
            sign.GetComponent<GeoMarkerController>().markerLabel = label;
            
            sign.transform.parent = markerObject.transform;
            
            // We need to put the gameobject coincident with its parent.
            // But potentially higher...
            sign.transform.localPosition = Vector3.zero;
        }

        markerObject.transform.localScale = new Vector3(markerScale, markerScale, markerScale);   
        return markerObject;
    }

    // Update is called once per frame
    void Update()
    {        
    }
}
