//-----------------------------------------------------------------------
// Script to add map on the plane
//-----------------------------------------------------------------------
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;
using Mapbox.Unity.Utilities;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
//using Firebase;
//using Firebase.Database;
//using Firebase.Unity.Editor;

public class TagGenerator : MonoBehaviour
{
    [Serializable]
    public class Place
    {
        public string _id;
        public string name;
        public string imageSrc;
        public double latitude;
        public double longitude;
        public string desc;
    }
    [Serializable]
    public class Event
    {
        public string _id;
        public string name;
        public string imageSrc;
        public double latitude;
        public double longitude;
        public string desc;
    }
    [Serializable]
    public class UTDData
    {
        //public List<Place> places;
        public List<Place> places;
        public List<Event> events;
    }

    private ILocationProvider _locationProvider;
    private IEnumerator coroutine;

    public AbstractMap map;
    private UTDData utdData;
    private string dataAsJson;
    public object result;

    // Unity: Start
    void Start()
    {
        utdData = new UTDData();
        // Populate places and events data
        GetUTDData();
        // Get current location provider instance
        _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
        _locationProvider.OnLocationUpdated += LocationUpdated;
        // fixed update time: 1 second
        Time.fixedDeltaTime = 1;
    }

    // Unity: Update
    void FixedUpdate()
    {


        coroutine = UpdateDistance(_locationProvider.CurrentLocation);
        StartCoroutine(coroutine);
    }

    // Unity: Location Updated
    void LocationUpdated(Location location)
    {
        _locationProvider.OnLocationUpdated -= LocationUpdated;
        coroutine = GenerateTags(location);
        StartCoroutine(coroutine);
    }

    // Get places data from json to UTD Data list
    private void GetUTDData()
    {
        //string jsonPath = "/utd.json";
        //string filePath = Application.streamingAssetsPath + jsonPath;
        //WWW file = new WWW(filePath);
        //string dataAsJson = file.text;
        coroutine = DownloadDataAsync();
        StartCoroutine(coroutine);
    }

    // create tags to show on unity
    private IEnumerator DownloadDataAsync()
    {
        WWWForm form = new WWWForm();
        form.AddField("type", "place");

        using (UnityWebRequest www = UnityWebRequest.Post("http://8f81c4d4.ngrok.io/api/getItems", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
                Debug.Log(www.error);
            else
                continueData(www.downloadHandler.text);
                Debug.Log(www.downloadHandler.text);
        }
    }

    public void continueData(String res)
    {
        utdData = JsonUtility.FromJson<UTDData>(res);
    }

    // create tags to show on unity
    private IEnumerator GenerateTags(Location deviceLocation)
    {
        yield return null;

        // PLACES
        int count = utdData.places.Count;
        Mapbox.Utils.Vector2d placePosition, evntPosition;
        for (int i = 0; i<count; i++)
        {
            Place place = utdData.places[i];
            string imgSrc = place.imageSrc;
            string imgUrl = Application.streamingAssetsPath + imgSrc;
            WWW img = new WWW(imgUrl);
            // apply image to tag
            GameObject nameTag = Instantiate(Resources.Load("PlaceTag")) as GameObject;
            nameTag.name = place._id;
            placePosition = new Mapbox.Utils.Vector2d(place.latitude, place.longitude);
            nameTag.transform.position = map.GeoToWorldPosition(placePosition) + new Vector3(0, 15f, 0);
            // add dynamic height factor
            // Update distance from current location
            Mapbox.Utils.Vector2d diffInCoords = deviceLocation.LatitudeLongitude - placePosition;
            Mapbox.Utils.Vector2d actualDistance = Conversions.LatLonToMeters(diffInCoords);
            int mag = (int)actualDistance.magnitude;
            // Update cube's sides with info
            Transform transform = nameTag.transform.GetChild(0);
            //transform.GetChild(0).GetComponent<TextMeshPro>().text = place._id;
            transform.GetChild(1).GetComponent<TextMeshPro>().text = place.name;
            transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = img.texture;
            transform.GetChild(3).GetComponent<TextMeshPro>().text = place.desc;
            transform.GetChild(4).GetComponent<TextMeshPro>().text = mag.ToString() + "m";
            transform.GetChild(6).GetComponent<Text>().text = place.name;

            nameTag.transform.SetParent(gameObject.transform.GetChild(0).gameObject.transform);

            // scaling
            Vector3 screenPos = Camera.main.WorldToScreenPoint(nameTag.transform.position);

        }


        // EVENTS
        count = utdData.events.Count;
        for (int i = 0; i < count; i++)
        {
            Event evnt = utdData.events[i];
            string imgSrc = evnt.imageSrc;
            string imgUrl = Application.streamingAssetsPath + imgSrc;
            WWW img = new WWW(imgUrl);
            // apply image to tag
            GameObject nameTag = Instantiate(Resources.Load("EventTag")) as GameObject;
            nameTag.name = evnt._id;
            evntPosition = new Mapbox.Utils.Vector2d(evnt.latitude, evnt.longitude);
            nameTag.transform.position = map.GeoToWorldPosition(evntPosition) + new Vector3(0, 5f, 0);
            // add dynamic height factor
            // Update distance from current location
            Mapbox.Utils.Vector2d diffInCoords = deviceLocation.LatitudeLongitude - evntPosition;
            Mapbox.Utils.Vector2d actualDistance = Conversions.LatLonToMeters(diffInCoords);
            int mag = (int)actualDistance.magnitude;
            // Update cube's sides with info
            Transform transform = nameTag.transform.GetChild(0);
            //transform.GetChild(0).GetComponent<TextMeshPro>().text = evnt._id;
            transform.GetChild(1).GetComponent<TextMeshPro>().text = evnt.name;
            transform.GetChild(2).GetComponent<MeshRenderer>().material.mainTexture = img.texture;
            transform.GetChild(3).GetComponent<TextMeshPro>().text = evnt.desc;
            transform.GetChild(4).GetComponent<TextMeshPro>().text = mag.ToString() + "m";
            transform.GetChild(6).GetComponent<Text>().text = evnt.name;

            nameTag.transform.SetParent(gameObject.transform.GetChild(1).gameObject.transform);
        }
    }

    // Update distance on each call
    public IEnumerator UpdateDistance(Location deviceLocation)
    {
        yield return null;
        
        // PLACES
        Mapbox.Utils.Vector2d placePosition;
        if (utdData != null || utdData.places.Count > 0)
        {
            int count = utdData.places.Count; // must be same as place data
            for (int i = 0; i < count; i++)
            {
                //Debug.Log(i);
                if (gameObject.transform.childCount > 0 && gameObject.transform.GetChild(0).transform.childCount > 0)
                {
                    GameObject tag = gameObject.transform.GetChild(0).transform.GetChild(i).gameObject;
                    Place place = utdData.places[i];
                    placePosition = new Mapbox.Utils.Vector2d(place.latitude, place.longitude);
                    // Update distance from current location
                    Mapbox.Utils.Vector2d diffInCoords = deviceLocation.LatitudeLongitude - placePosition;
                    Mapbox.Utils.Vector2d actualDistance = Conversions.LatLonToMeters(diffInCoords);
                    int mag = (int)actualDistance.magnitude;
                    // Update cube's sides with info
                    tag.transform.GetChild(0).GetChild(4).GetComponent<TextMeshPro>().text = mag.ToString() + "m";

                    tag.transform.rotation = Camera.main.transform.rotation;
                }
            }
        }

        // EVENTS
        Mapbox.Utils.Vector2d evntPosition;
        if (utdData != null || utdData.events.Count > 0)
        {
            int count = utdData.events.Count; // must be same as evnt data
            for (int i = 0; i < count; i++)
            {
                //Debug.Log(i);
                if (gameObject.transform.childCount > 0 && gameObject.transform.GetChild(1).transform.childCount > 0)
                {
                    GameObject tag = gameObject.transform.GetChild(1).transform.GetChild(i).gameObject;
                    Event evnt = utdData.events[i];
                    evntPosition = new Mapbox.Utils.Vector2d(evnt.latitude, evnt.longitude);
                    // Update distance from current location
                    Mapbox.Utils.Vector2d diffInCoords = deviceLocation.LatitudeLongitude - evntPosition;
                    Mapbox.Utils.Vector2d actualDistance = Conversions.LatLonToMeters(diffInCoords);
                    int mag = (int)actualDistance.magnitude;
                    // Update cube's sides with info
                    tag.transform.GetChild(0).GetChild(4).GetComponent<TextMeshPro>().text = mag.ToString() + "m";

                    tag.transform.rotation = Camera.main.transform.rotation;
                }
            }
        }
    }
}