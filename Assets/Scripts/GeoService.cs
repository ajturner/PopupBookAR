// Simple class to fetch and parse a GeoService from ArcGIS Server

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON; 
public class GeoService : MonoBehaviour
{
    private static GeoService _instance;
    void Awake() 
    {
        print("create GeoService instance");
        _instance = this;
    }
    public static GeoService Instance { get { return _instance; } }

    // location = location.longitude, location.latitude
    // Trees
    // string dataUrl = "https://services.arcgis.com/bkrWlSKcjUDFDtgw/ArcGIS/rest/services/DC_Trees_Republished/FeatureServer/0";
    public static void GetData(string layerUrl, Vector2 location, string geometryType, float bufferDistance, Action<JSONNode> callback = null) {

        string dataUrl = layerUrl;

        string queryParams = "/query?where=1%3D1&objectIds=&time=&geometryType=esriGeometryPoint&inSR=4326&spatialRel=esriSpatialRelIntersects&resultType=none&units=esriSRUnit_Meter&returnGeodetic=false&outFields=*&returnHiddenFields=false&featureEncoding=esriDefault&multipatchOption=xyFootprint&maxAllowableOffset=&geometryPrecision=&outSR=4326&datumTransformation=&applyVCSProjection=false&returnIdsOnly=false&returnUniqueIdsOnly=false&returnCountOnly=false&returnExtentOnly=false&returnQueryGeometry=false&returnDistinctValues=false&cacheHint=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&having=&resultOffset=&resultRecordCount=&returnZ=false&returnM=false&returnExceededLimitFeatures=true&quantizationParameters=&sqlFormat=none&f=json&geometry=";
        string requestUrl = dataUrl + queryParams + location.y + "," + location.x;
        requestUrl += "&distance=" + bufferDistance;
        
        if(geometryType == "polygon")
        {
            requestUrl += "&returnCentroid=true";
        } else {
            requestUrl += "&returnGeometry=true";
        }
            
        print("Query: " + requestUrl);
        GeoService.Instance.StartCoroutine(GetRequest(requestUrl, callback));
    }

            
    private static IEnumerator GetRequest(string uri, Action<JSONNode> callback = null)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                var json = JSON.Parse(System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data));

                if (json == null)
                {
                    print("---------------- NO DATA ----------------");
                }
                else
                {
                    var features = json["features"].AsArray;
                    if (callback != null)
                        callback(features);
                }                
            }
        }
    }

}
