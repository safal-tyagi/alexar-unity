using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsButtonHandler : MonoBehaviour
{
    private GameObject eventsHolder;
    private Button eventButton;
    private bool clicked;

    // Use this for initialization
    void Start()
    {
        clicked = false;
        eventButton = gameObject.GetComponent<Button>();
        eventButton.onClick.AddListener(clickEventBtn);
        Debug.Log(eventsHolder);

        eventsHolder = GameObject.Find("EventsHolder");
    }

    void clickEventBtn()
    {
        Debug.Log("Starting button click");

        // GameObject eventsHolder = GameObject.Find("EventsHolder");
        // set everything true before toggle
        for (int i = 0; i < eventsHolder.transform.childCount; i++)
        {
            eventsHolder.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (clicked == true)
        {
            eventsHolder.SetActive(true);
            clicked = false;
        }
        else
        {
            eventsHolder.SetActive(false);
            clicked = true;
        }
    }
}
