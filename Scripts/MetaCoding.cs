using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using SimpleJSON;

public class MetaCoding : MonoBehaviour {

    int counter = 1;

    IEnumerator DownloadWebService()
    {
        while (true) { 
            WWW w = new WWW("http://localhost:5000/?command"); // change address here.
            yield return w;

            print("Waiting for webservice\n");

            yield return new WaitForSeconds(1f);

            print("Received webservice\n");
        
            ExtractCommand(w.text);

            print("Extracted information");

            WWW y = new WWW("http://localhost:5000/?command=empty");
            yield return y;

            print("Cleaned webservice");

            yield return new WaitForSeconds(5);
        }
    }

    void ExtractCommand(string json)
    {
        var jsonstring = JSON.Parse(json);
        string command = jsonstring["command"];
        print(command);
        if (command == null) { return;  }
        string[] commands_array = command.Split(" "[0]);
        if(commands_array.Length < 3)
        {
            return;
        }
        if (commands_array[0] == "create")
        {
            CreateObject(commands_array[1], commands_array[2]);
        }
    }

    void CreateObject(string color, string shape)
    {

        string name = "NewObject_" + counter;
        counter += 1;
        GameObject NewObject = new GameObject(name);

        switch (shape)
        {
            case "cube":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case "sphere":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            case "cylinder":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                break;
            case "capsule":
                NewObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                break;
        }
        NewObject.transform.position = new Vector3(0, 5, 0);
        NewObject.AddComponent<Rigidbody>();
        switch (color)
        {
            case "red":
                NewObject.GetComponent<Renderer>().material.color = Color.red;
                break;
            case "yellow":
                NewObject.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case "green":
                NewObject.GetComponent<Renderer>().material.color = Color.green;
                break;
            case "blue":
                NewObject.GetComponent<Renderer>().material.color = Color.blue;
                break;
            case "black":
                NewObject.GetComponent<Renderer>().material.color = Color.black;
                break;
            case "white":
                NewObject.GetComponent<Renderer>().material.color = Color.white;
                break;
        }
    }

        // Use this for initialization
    void Start () {
        print("Started webservice import...\n");

        StartCoroutine(DownloadWebService());
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}