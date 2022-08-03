using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeoMarkerController : MonoBehaviour
{
    [SerializeField, Tooltip("Label to show on top of marker")]
    public string markerLabel = "Your Ad Here!";

    [SerializeField, Tooltip("Object with a TextMesh Pro component")]
    public GameObject textObject;

    // Start is called before the first frame update
    void Start()
    {
        TMP_Text mText = textObject.GetComponent<TextMeshPro>();
        mText.text = markerLabel;
        
    }

    // Update is called once per frame
    void Update()
    {

        // Text should face user
        // textObject.transform.LookAt(Camera.main.transform);
    }
}
