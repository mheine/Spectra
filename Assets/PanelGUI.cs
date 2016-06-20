using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class PanelGUI : MonoBehaviour {

    private bool isShowing;
    private string dir;


	// Use this for initialization
	void Start () {

        isShowing = true;
        int counter = 0;
        dir = "C:\\Users\\Marcus\\Music\\Pendulum";



        string[] listOfFiles = Directory.GetFiles(dir);
        List<string> filesAsList = new List<string>();


        var button = GameObject.Find("B1");
        GameObject grid = GameObject.Find("Grid");

        foreach (string filename in listOfFiles)
        {
            if(filename.EndsWith(".mp3"))
            {
                filesAsList.Add(filename);
            }
        }


            foreach (string filename in filesAsList)
        {
     
            var btn = (GameObject)Instantiate(button, transform.position, Quaternion.identity);
            btn.name = "B_" + counter;
            btn.transform.parent = grid.transform;
            btn.GetComponentInChildren<Text>().text = filename.Substring(dir.Length+1, filename.Length-(dir.Length + 5));
            counter++;
        }
        button.SetActive(false);





    }

    // Update is called once per frame
    void Update () {

        GameObject canvasObject = GameObject.Find("Canvas");
        GameObject list = GameObject.Find("ListField");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isShowing = !isShowing;
            canvasObject.GetComponent<Canvas>().enabled = isShowing;
        }

    }
}
