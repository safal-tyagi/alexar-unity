using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlacesButtonHandler : MonoBehaviour
{
    private GameObject placesHolder;
    private Button placesButton;
    private bool clicked;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Here1");
        clicked = false;
        placesButton = gameObject.GetComponent<Button>();
        placesButton.onClick.AddListener(clickPlaceBtn);
        Debug.Log(placesHolder);

        placesHolder = GameObject.Find("PlacesHolder");
        Debug.Log("Here2");
    }

    void clickPlaceBtn()
    {
        Debug.Log("Starting button click, clicked = " + true);

        // GameObject placesHolder = GameObject.Find("PlacesHolder");

        // set everything true before toggle
        for (int i = 0; i < placesHolder.transform.childCount; i++)
        {
            placesHolder.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (clicked == true)
        {
            placesHolder.SetActive(true);
            clicked = false;
            Debug.Log("Now clicked is false");
        }
        else
        {
            placesHolder.SetActive(false);
            clicked = true;
            Debug.Log("Now clicked is true");
        }
    }
}
