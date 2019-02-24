using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SearchButtonHandler : MonoBehaviour
{
    private GameObject searchsHolder;
    private Button searchButton;
    private InputField inputText;
    private bool clicked;
    private GameObject placesHolder;
    private GameObject eventsHolder;
    

    // Use this for initialization
    void Start()
    {
        searchButton = gameObject.GetComponent<Button>();
        searchButton.onClick.AddListener(clickSearchBtn);

        inputText = FindObjectOfType<InputField>();
        placesHolder = GameObject.Find("PlacesHolder");
        eventsHolder = GameObject.Find("EventsHolder");
    }

    void clickSearchBtn()
    {
        Debug.Log(inputText.text);
        string query = inputText.text;

        // for places
        placesHolder.SetActive(true);
        for (int i = 0; i < placesHolder.transform.childCount; i++)
        {
            string toSearch= placesHolder.transform.GetChild(i).transform.GetChild(0).transform.GetChild(6).transform.GetComponent<Text>().text;

            if (toSearch.IndexOf(query, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log("PLACES: TRUE!!");
            } else
            {
                Debug.Log("PLACE:: Child did not satisfy");
                placesHolder.transform.GetChild(i).gameObject.SetActive(false);
            }
               
        }

        // for events
        eventsHolder.SetActive(true);
        for (int i = 0; i < eventsHolder.transform.childCount; i++)
        {
            string toSearch = eventsHolder.transform.GetChild(i).transform.GetChild(0).transform.GetChild(6).transform.GetComponent<Text>().text;

            if (toSearch.IndexOf(query, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Debug.Log("EVENTS: TRUE!!");
            } else
            {
                Debug.Log("EENT:: Child did not satisfy");
                eventsHolder.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
