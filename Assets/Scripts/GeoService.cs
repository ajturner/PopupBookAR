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

    // location = location.longitude + "," + location.latitude
    public static void GetData(string layerUrl, Vector2 location, string geometryType, float bufferDistance, Action<JSONNode> callback = null) {
        // string dataUrl = "https://maps2.dcgis.dc.gov/dcgis/rest/services/DCGIS_DATA/Environment_WebMercator/MapServer/23";

        // Trees
        // string dataUrl = "https://services.arcgis.com/bkrWlSKcjUDFDtgw/ArcGIS/rest/services/DC_Trees_Republished/FeatureServer/0";
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

        // Wards
        // string dataUrl = "https://maps2.dcgis.dc.gov/dcgis/rest/services/DCGIS_DATA/Administrative_Other_Boundaries_WebMercator/MapServer/31";
        // string queryParams = "/query?f=json&where=1=1&inSR=4326&spatialRel=esriSpatialRelIntersects&returnGeometry=false&geometryType=esriGeometryPoint&geometry=";
        // string requestUrl = dataUrl + queryParams + location.ToString();
        
        // Explicitly get a tile
        // string requestUrl = "https://maps2.dcgis.dc.gov/dcgis/rest/services/DCGIS_DATA/Environment_WebMercator/MapServer/23/query?f=json&geometry=%7B%22spatialReference%22%3A%7B%22latestWkid%22%3A3857%2C%22wkid%22%3A102100%7D%2C%22xmin%22%3A-8571648.351900414%2C%22ymin%22%3A4705769.2093503475%2C%22xmax%22%3A-8571342.603787273%2C%22ymax%22%3A4706074.957463488%7D&maxRecordCountFactor=3&outFields=FACILITYID%2COBJECTID&&returnExceededLimitFeatures=false&spatialRel=esriSpatialRelIntersects&where=1%3D1&geometryType=esriGeometryEnvelope&inSR=102100&outSR=4326";
        // StartCoroutine(GetRequest(requestUrl));

        // requestUrl = "https://maps2.dcgis.dc.gov/dcgis/rest/services/DCGIS_DATA/Environment_WebMercator/MapServer/23/query?f=json&geometry=%7B%22spatialReference%22%3A%7B%22latestWkid%22%3A3857%2C%22wkid%22%3A102100%7D%2C%22xmin%22%3A-8571648.351900414%2C%22ymin%22%3A4706074.957463488%2C%22xmax%22%3A-8571342.603787273%2C%22ymax%22%3A4706380.705576628%7D&maxRecordCountFactor=3&outFields=FACILITYID%2COBJECTID&outSR=4326&returnExceededLimitFeatures=false&spatialRel=esriSpatialRelIntersects&where=1%3D1&geometryType=esriGeometryEnvelope&inSR=102100";
        // StartCoroutine(GetRequest(requestUrl));

        // requestUrl = "https://maps2.dcgis.dc.gov/dcgis/rest/services/DCGIS_DATA/Environment_WebMercator/MapServer/23/query?f=json&geometry=%7B%22spatialReference%22%3A%7B%22latestWkid%22%3A3857%2C%22wkid%22%3A102100%7D%2C%22xmin%22%3A-8571342.603787273%2C%22ymin%22%3A4705769.2093503475%2C%22xmax%22%3A-8571036.855674133%2C%22ymax%22%3A4706074.957463488%7D&maxRecordCountFactor=3&outFields=FACILITYID%2COBJECTID&outSR=4326&returnExceededLimitFeatures=false&spatialRel=esriSpatialRelIntersects&where=1%3D1&geometryType=esriGeometryEnvelope&inSR=102100";
        // StartCoroutine(GetRequest(requestUrl));

        // requestUrl = "https://maps2.dcgis.dc.gov/dcgis/rest/services/DCGIS_DATA/Environment_WebMercator/MapServer/23/query?f=json&geometry=%7B%22spatialReference%22%3A%7B%22latestWkid%22%3A3857%2C%22wkid%22%3A102100%7D%2C%22xmin%22%3A-8572259.848126695%2C%22ymin%22%3A4705769.2093503475%2C%22xmax%22%3A-8571954.100013554%2C%22ymax%22%3A4706074.957463488%7D&maxRecordCountFactor=3&outFields=FACILITYID%2COBJECTID&outSR=4326&returnExceededLimitFeatures=false&spatialRel=esriSpatialRelIntersects&where=1%3D1&geometryType=esriGeometryEnvelope&inSR=102100";
        // StartCoroutine(GetRequest(requestUrl));
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
